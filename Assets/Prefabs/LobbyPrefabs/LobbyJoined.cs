using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyJoined : MonoBehaviour
{
    // Start is called before the first frame update

    ClientConnectionHandler m_ServerConnection = null;
    public List<GameObject> LobyStatuses = new List<GameObject>();
    List<System.Tuple<System.Action<RuleServer.ServerMessage>, RuleServer.ServerMessage>> m_ActionList = new List<System.Tuple<System.Action<RuleServer.ServerMessage>, RuleServer.ServerMessage>>();

    void p_HandleLocalReadyClick(bool IsReady)
    {
        RuleServer.UpdateLobbyStatus UpdateMessage = new RuleServer.UpdateLobbyStatus();
        UpdateMessage.NewStatus.Ready = IsReady;
        //m_ServerConnection.SendMessage(UpdateMessage,new System.Action<RuleServer.ServerMessage>(p_HandleSetReadyStatusResponse));
        CallbackDelagator.SendMessageToMain(m_ServerConnection, m_ActionList, p_HandleSetReadyStatusResponse, UpdateMessage);

    }

    void Start()
    {
        m_ServerConnection = FindObjectOfType<ClientConnectionHandler>();
        PlayerStatus HostStatus = LobyStatuses[0].GetComponent<PlayerStatus>();
        PlayerStatus LocalStatus = LobyStatuses[1].GetComponent<PlayerStatus>();


        HostStatus.SetName("Host");
        HostStatus.SetInteractive(false);

        LocalStatus.SetName("You");
    }
    string m_LobbyID = "";
    public void Quit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/LobbyScene.unity");
    }

    void p_SetErrorString(string Error)
    {

    }

    void p_HandleQueryLobbyEvent(RuleServer.ServerMessage Response)
    {
        if(Response == null)
        {
            p_SetErrorString("Error handlig lobby events: server connection terminated");
            return;
        }
        if(Response is RuleServer.RequestStatusResponse)
        {
            RuleServer.RequestStatusResponse RequestStatus = (RuleServer.RequestStatusResponse)Response;
            p_SetErrorString(RequestStatus.ErrorString);    
            return;
        }
        RuleServer.LobbyEventResponse EventResponse = (RuleServer.LobbyEventResponse)Response;
        foreach(RuleServer.LobbyEvent Event in EventResponse.Events)
        {
            if(Event is RuleServer.LobbyEvent_GameStart)
            {
                //Start game
                UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/SverkerTestScene.unity");
            }
            else if(Event is RuleServer.LobbyEvent_PlayerJoined)
            {
                //sus, ony supports 2 players
            }
            else if(Event is RuleServer.LobbyEvent_StatusUpdated)
            {
                //ASSUMPTION only 2 players, has to be the host that is updated
                RuleServer.LobbyEvent_StatusUpdated StatusUpdateEvent = (RuleServer.LobbyEvent_StatusUpdated)Event;
                LobyStatuses[0].GetComponent<PlayerStatus>().SetReady(StatusUpdateEvent.NewStatus.Ready);
            }
        }
    }
    void p_HandleSetReadyStatusResponse(RuleServer.ServerMessage Response)
    {
        if(Response is RuleServer.RequestStatusResponse)
        {
            RuleServer.RequestStatusResponse Status = (RuleServer.RequestStatusResponse)Response;
            if(Status.ErrorString != "Ok")
            {
                print("Yikes");
            }
        }
        throw new System.Exception("Invalid response to server request");
    }
    public void SetLobbyID(string LobbyID)
    {
        m_LobbyID = LobbyID;
    }

    public void OnDestroy()
    {
        
    }
    // Update is called once per frame
    double m_LastPollDiff = 0;
    void Update()
    {
        lock (m_ActionList)
        {
            foreach (System.Tuple<System.Action<RuleServer.ServerMessage>, RuleServer.ServerMessage> Actions in m_ActionList)
            {
                Actions.Item1(Actions.Item2);
            }
        }
        m_LastPollDiff += Time.deltaTime;
        if(m_LastPollDiff >= 1 && m_ServerConnection.IsConnected())
        {
            m_LastPollDiff = 0;
            RuleServer.LobbyEventPoll EventPoll = new RuleServer.LobbyEventPoll();
            EventPoll.LobbyID = m_LobbyID;
            //m_ServerConnection.SendMessage(EventPoll,new System.Action<RuleServer.ServerMessage>(p_HandleQueryLobbyEvent));
            CallbackDelagator.SendMessageToMain(m_ServerConnection, m_ActionList, p_HandleQueryLobbyEvent, EventPoll);
        }
    }
}
