using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

using UnityEngine.SceneManagement;
using TMPro;
public class LobbyTest : MonoBehaviour
{
    // Start is called before the first frame update
    string LobbyID = "";
    bool InLobby = false;
    public int ConnectionID = 0;
    class Event
    {

    }
    class Register : Event
    {

    }
    class Connect : Event
    {
        public string LobbyCode;
    }
    class StarEvent : Event
    {
        
    }
    enum EventType
    {

    }

    Stack<Event> m_Events = new Stack<Event>();
    SemaphoreSlim m_Semaphore = new SemaphoreSlim(0);

    GameState StateObject = null;

    bool LoadGameScene = false;
    void LobbyServerConnector()
    {
        RuleServer.ClientConnection Connection = new RuleServer.ClientConnection("192.168.0.16", 11337);
        //RuleServer.ClientConnection Connection = new RuleServer.ClientConnection("127.0.0.1", 11337);
        while(!LoadGameScene)
        {
            bool Succesfull = m_Semaphore.Wait(3000);
            if(Succesfull)
            {
                Event CurrentEvent = null;
                lock (m_Events)
                {
                    CurrentEvent = m_Events.Pop();
                }
                if (CurrentEvent is Register)
                {
                    RuleServer.RegisterLobby LobbyEvent = new RuleServer.RegisterLobby();
                    LobbyEvent.ConnectionIdentifier = ConnectionID;
                    LobbyEvent.LobbyID = "";
                    RuleServer.ServerMessage Response = Connection.SendMessage(new RuleServer.RegisterLobby());
                    if (Response is RuleServer.RequestStatusResponse)
                    {
                        print("Error in request: " + ((RuleServer.RequestStatusResponse)Response).ErrorString);
                    }
                    else if (Response is RuleServer.RegisterLobbyResponse)
                    {
                        LobbyID = ((RuleServer.RegisterLobbyResponse)Response).LobbyID;
                        print("Lobby code: " + LobbyID);
                    }
                    InLobby = true;

                    RuleServer.ConnectionLobbyStatus Status = new RuleServer.ConnectionLobbyStatus();
                    Status.Ready = true;
                    RuleServer.UpdateLobbyStatus StatusMessage = new RuleServer.UpdateLobbyStatus();
                    StatusMessage.LobbyID = LobbyID;
                    StatusMessage.NewStatus = Status;
                    Connection.SendMessage(StatusMessage);
                }
                if (CurrentEvent is Connect)
                {
                    RuleServer.JoinLobby JoinMessage = new RuleServer.JoinLobby();

                    JoinMessage.ConnectionIdentifier = ConnectionID;
                    JoinMessage.LobbyID = LobbyID;
                    RuleServer.ServerMessage JoinResult = Connection.SendMessage(JoinMessage);
                    if (JoinResult is RuleServer.RequestStatusResponse)
                    {
                        string Result = ((RuleServer.RequestStatusResponse)JoinResult).ErrorString;
                        if (Result != "Ok")
                        {
                            print("Error joining lobby: " + ((RuleServer.RequestStatusResponse)JoinResult).ErrorString);
                            continue;
                        }
                    }
                    InLobby = true;
                    RuleServer.ConnectionLobbyStatus Status = new RuleServer.ConnectionLobbyStatus();
                    Status.Ready = true;
                    RuleServer.UpdateLobbyStatus StatusMessage = new RuleServer.UpdateLobbyStatus();
                    StatusMessage.ConnectionIdentifier = ConnectionID;
                    StatusMessage.LobbyID = LobbyID;
                    StatusMessage.NewStatus = Status;
                    Connection.SendMessage(StatusMessage);
                }
                else if(CurrentEvent is StarEvent)
                {
                    RuleServer.SendLobbyEvent NewMessage = new RuleServer.SendLobbyEvent();
                    NewMessage.LobbyID = LobbyID;
                    RuleServer.LobbyEvent_GameStart NewEvent = new RuleServer.LobbyEvent_GameStart();
                    NewMessage.EventToSend = NewEvent;
                    RuleServer.ServerMessage Response = Connection.SendMessage(NewMessage);
                    if(Response is not RuleServer.RequestStatusResponse)
                    {
                        print("Sus");
                        break;
                    }
                    RuleServer.RequestStatusResponse Status = (RuleServer.RequestStatusResponse)Response;
                    if(Status.ErrorString != "Ok")
                    {
                        print("Error starting game: " + Status.ErrorString);
                        continue;
                    }
                }
            }
            else if(InLobby)
            {
                RuleServer.LobbyEventPoll PollMessage = new RuleServer.LobbyEventPoll();
                PollMessage.ConnectionIdentifier = ConnectionID;
                PollMessage.LobbyID = LobbyID;
                RuleServer.ServerMessage Response = Connection.SendMessage(PollMessage);
                if(Response is RuleServer.RequestStatusResponse)
                {
                    print("Error retrieving events: " + ((RuleServer.RequestStatusResponse)Response).ErrorString);
                    continue;
                }
                List<RuleServer.LobbyEvent> Events = ((RuleServer.LobbyEventResponse)Response).Events;
                foreach(RuleServer.LobbyEvent Event in Events)
                {
                    if(Event.Type == RuleServer.LobbyEventType.GameStart)
                    {
                        RuleServer.LobbyEvent_GameStart GameStartEvent = (RuleServer.LobbyEvent_GameStart)Event;
                        StateObject.SetActionRetriever(GameStartEvent.PlayerIndex == 0 ? 1 : 0, new NetworkActionRetriever(Connection, GameStartEvent.GameID, GameStartEvent.PlayerIndex == 0 ? 1 : 0));
                        StateObject.SetLocalPlayerIndex(GameStartEvent.PlayerIndex);
                        print("This is the local player index: "+GameStartEvent.PlayerIndex);
                        print("This is the opponent index: "+(GameStartEvent.PlayerIndex == 0 ? 1 : 0));
                        LoadGameScene = true;
                        
                    }
                    print(Event.ToString());
                }
            }
        }
        print("Exiting lobby thread");
    }

    void _RunServer()
    {
        RuleServer.RuleServer NewServer = new RuleServer.RuleServer(11337);
        NewServer.Run();
    }
    void Start()
    {
        StateObject = FindObjectOfType<GameState>();
        Thread MessageThread = new Thread(LobbyServerConnector);
        MessageThread.Start();
        Thread ServerThread = new Thread(_RunServer);
        ServerThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            lock(m_Events)
            {
                m_Events.Push(new Register());
            }
            m_Semaphore.Release();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            lock(m_Events)
            {
                string LobbyString = GetComponent<TextMeshPro>().text;
                print("Join string: "+ LobbyString);
                Connect ConnectEvent = new Connect();
                LobbyID = LobbyString;
                ConnectEvent.LobbyCode = LobbyString;
                m_Events.Push(ConnectEvent);
            }
            m_Semaphore.Release();
        }
        if(LoadGameScene)
        {
            SceneManager.LoadScene("Assets/Scenes/SverkerTestScene.unity");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            lock (m_Events)
            {
                m_Events.Push(new StarEvent());
            }
            m_Semaphore.Release();
        }
    }
}
