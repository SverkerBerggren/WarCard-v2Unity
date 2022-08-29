using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CallbackDelagator
{
    List<System.Tuple<System.Action<RuleServer.ServerMessage>,RuleServer.ServerMessage>> m_ActionList = new List<System.Tuple<System.Action<RuleServer.ServerMessage>, RuleServer.ServerMessage>>();
    System.Action<RuleServer.ServerMessage> m_ResultAction = null;
    public CallbackDelagator(List<System.Tuple<System.Action<RuleServer.ServerMessage>, RuleServer.ServerMessage>> ActionList,System.Action<RuleServer.ServerMessage> Result)
    {
        m_ActionList = ActionList;
        m_ResultAction = Result;
    }

    public void Callback(RuleServer.ServerMessage Result)
    {
        lock(m_ActionList)
        {
            m_ActionList.Add(new System.Tuple<System.Action<RuleServer.ServerMessage>, RuleServer.ServerMessage>(m_ResultAction,Result));
        }
    }
    public static void SendMessageToMain(ClientConnectionHandler Connection, List<System.Tuple<System.Action<RuleServer.ServerMessage>, RuleServer.ServerMessage>> ActionList,
        System.Action<RuleServer.ServerMessage> Result,RuleServer.ClientMessage MessageToSend)
    {
        CallbackDelagator Delagator = new CallbackDelagator(ActionList, Result);
        Connection.SendMessage(MessageToSend, Delagator.Callback);
    }
}

public class LobbyHost : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI HostCodeButtonText = null;
    private ClientConnectionHandler ServerConnection = null;
    public GameObject StatusObject = null;
    public List<GameObject> LobyStatuses = new List<GameObject>();


    private List<System.Tuple<System.Action<RuleServer.ServerMessage>, RuleServer.ServerMessage>> m_ActionList = new List<System.Tuple<System.Action<RuleServer.ServerMessage>, RuleServer.ServerMessage>>();
    

    
    
    void Start()
    {
        ServerConnection = FindObjectOfType<ClientConnectionHandler>();
        RuleServer.RegisterLobby LobbyRequest = new RuleServer.RegisterLobby();
        //ServerConnection.SendMessage(LobbyRequest, new System.Action<RuleServer.ServerMessage>(p_HandleCreateLobbyReponse));
        CallbackDelagator.SendMessageToMain(ServerConnection, m_ActionList, p_HandleCreateLobbyReponse,LobbyRequest);
        PlayerStatus HostStatus = LobyStatuses[0].GetComponent<PlayerStatus>();
        HostStatus.gameObject.SetActive(true);
        HostStatus.SetName("You");
        HostStatus.SetFactionIndexCallback(p_LocalFactionIndexCallback);
        HostStatus.SetReadyCallback(p_LocalReadyCallback);
    }
    string m_LobbyID = "";
    string m_ErrorString = "";

    void p_HandleStatusUpdateResponse(RuleServer.ServerMessage Response)
    {
        if (Response == null)
        {
            m_ErrorString = "Error connecting to server";
            return;
        }
        string Status = ((RuleServer.RequestStatusResponse)Response).ErrorString;
        if (Status != "Ok")
        {
            m_ErrorString = "Error sending status update: " + m_ErrorString;
            return;
        }
    }

    void p_LocalReadyCallback(bool IsReady)
    {
        RuleServer.UpdateLobbyStatus UpdateMessage = new RuleServer.UpdateLobbyStatus();
        UpdateMessage.LobbyID = m_LobbyID;
        UpdateMessage.NewStatus = LobyStatuses[0].GetComponent<PlayerStatus>().GetLobbyStatus();
        CallbackDelagator.SendMessageToMain(ServerConnection, m_ActionList, p_HandleStatusUpdateResponse, UpdateMessage);
    }
    void p_LocalFactionIndexCallback(int FactionIndex)
    {
        RuleServer.UpdateLobbyStatus UpdateMessage = new RuleServer.UpdateLobbyStatus();
        UpdateMessage.LobbyID = m_LobbyID;
        UpdateMessage.NewStatus = LobyStatuses[0].GetComponent<PlayerStatus>().GetLobbyStatus();
        CallbackDelagator.SendMessageToMain(ServerConnection, m_ActionList, p_HandleStatusUpdateResponse,UpdateMessage);
    }

    void p_HandleQueryLobbyEvents(RuleServer.ServerMessage Response)
    {
        if(Response == null)
        {
            m_ErrorString = "Error connecting to server";
            return;
        }
        if(Response is RuleServer.RequestStatusResponse)
        {
            string Status = ((RuleServer.RequestStatusResponse)Response).ErrorString;
            if(Status != "Ok")
            {
                m_ErrorString = "Error retrieving updates: " + m_ErrorString;
                return;
            }
            //Should not happen
            return;
        }
        RuleServer.LobbyEventResponse EventResponse = ((RuleServer.LobbyEventResponse)Response);
        foreach(RuleServer.LobbyEvent Event in EventResponse.Events)
        {
            if(Event is RuleServer.LobbyEvent_PlayerJoined)
            {
                //add new player
                RuleServer.LobbyEvent_PlayerJoined JoinEvent = (RuleServer.LobbyEvent_PlayerJoined)Event;
                //hardcoded
                PlayerStatus JoinedStatus = LobyStatuses[1].GetComponent<PlayerStatus>();
                JoinedStatus.SetName("Guest 1");
                JoinedStatus.SetInteractive(false);
                JoinedStatus.gameObject.SetActive(true);
            }
            else if(Event is RuleServer.LobbyEvent_StatusUpdated)
            {
                RuleServer.LobbyEvent_StatusUpdated StatusEvent = (RuleServer.LobbyEvent_StatusUpdated)Event;
                LobyStatuses[1].GetComponent<PlayerStatus>().SetReady(StatusEvent.NewStatus.Ready);
                LobyStatuses[1].GetComponent<PlayerStatus>().SetFactionIndex(StatusEvent.NewStatus.FactionIndex);
            }
            else if(Event is RuleServer.LobbyEvent_GameStart)
            {
                //start game
                RuleServer.LobbyEvent_GameStart GameStartEvent = (RuleServer.LobbyEvent_GameStart)Event;
                GlobalNetworkState.LocalPlayerIndex = GameStartEvent.PlayerIndex;
                GlobalNetworkState.OpponentActionRetriever = new NetworkActionRetriever(ServerConnection.GetUnderlyingConnection(), GameStartEvent.PlayerIndex, GameStartEvent.PlayerIndex == 0 ? 1 : 0);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/SverkerTestScene.unity");
            }
        }
    }
    void p_HandleCreateLobbyReponse(RuleServer.ServerMessage Response)
    {
        if (Response == null)
        {
            m_ErrorString = "Error connecting to server";
            return;
        }
        if (Response is RuleServer.RequestStatusResponse)
        {
            string Status = ((RuleServer.RequestStatusResponse)Response).ErrorString;
            if(Status != "Ok")
            {
                m_ErrorString = "Error creating lobby: "+Status;
                return;
            }
        }
        RuleServer.RegisterLobbyResponse LobbyResponse = (RuleServer.RegisterLobbyResponse)Response;
        m_LobbyID = LobbyResponse.LobbyID;
    }
    public void LobbyCodeClick()
    {
        GUIUtility.systemCopyBuffer = HostCodeButtonText.text;
    }
    public void Quit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/LobbyScene.unity");
    }
    void p_HandleStartResponse(RuleServer.ServerMessage Response)
    {
        if(Response == null)
        {
            m_ErrorString = "Error starting game: Error connecting to server";
            return;
        }
        if(((RuleServer.RequestStatusResponse)Response).ErrorString != "Ok")
        {
            m_ErrorString = ((RuleServer.RequestStatusResponse)Response).ErrorString;
        }
    }
    public void StartGame()
    {
        RuleServer.LobbyEvent_GameStart StartGame = new RuleServer.LobbyEvent_GameStart();
        //StartGame.Lob = m_LobbyID;
        //StartGame.PlayerIndex
        RuleServer.SendLobbyEvent Message = new RuleServer.SendLobbyEvent();
        Message.LobbyID = m_LobbyID;
        Message.EventToSend = StartGame;
        CallbackDelagator.SendMessageToMain(ServerConnection, m_ActionList, p_HandleStartResponse, Message);
        print("Starting");
    }


    

    // Update is called once per frame
    double m_LastQueryTime = 0;
    void Update()
    {
        m_LastQueryTime += Time.deltaTime;
        if(m_LastQueryTime >= 1 && ServerConnection.IsConnected())
        {
            m_LastQueryTime = 0;
            RuleServer.LobbyEventPoll EventPoll = new RuleServer.LobbyEventPoll();
            EventPoll.LobbyID = m_LobbyID;
            //ServerConnection.SendMessage(EventPoll, new System.Action<RuleServer.ServerMessage>(p_HandleQueryLobbyEvents));
            CallbackDelagator.SendMessageToMain(ServerConnection, m_ActionList, p_HandleQueryLobbyEvents, EventPoll);

        }
        lock (m_ActionList)
        {
            foreach (System.Tuple<System.Action<RuleServer.ServerMessage>, RuleServer.ServerMessage> Actions in m_ActionList)
            {
                Actions.Item1(Actions.Item2);
            }
        }
        if (m_LobbyID != "" && m_LobbyID != HostCodeButtonText.text)
        {
            HostCodeButtonText.color = new Color(0, 0, 0);
            HostCodeButtonText.text = m_LobbyID;
        }
        if(m_ErrorString != "")
        {
            HostCodeButtonText.color = new Color(1, 0, 0);
            HostCodeButtonText.text = m_ErrorString;
        }
    }
}
