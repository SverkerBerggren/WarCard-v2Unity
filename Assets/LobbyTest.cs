using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


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
    enum EventType
    {

    }

    Stack<Event> m_Events = new Stack<Event>();
    SemaphoreSlim m_Semaphore = new SemaphoreSlim(0);

    void LobbyServerConnector()
    {
        RuleServer.ClientConnection Connection = new RuleServer.ClientConnection("127.0.0.1",11337);
        while(true)
        {
            bool Succesfull = m_Semaphore.Wait(3000);
            if(Succesfull)
            {
                lock (m_Events)
                {
                    Event CurrentEvent = m_Events.Pop();
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
                    print(Event.ToString());
                }
            }
        }
    }

    void _RunServer()
    {
        RuleServer.RuleServer NewServer = new RuleServer.RuleServer(11337);
        NewServer.Run();
    }
    void Start()
    {
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
                print("Join string: LobbyString");
                Connect ConnectEvent = new Connect();
                ConnectEvent.LobbyCode = LobbyString;
                m_Events.Push(ConnectEvent);
            }
            m_Semaphore.Release();
        }
    }
}
