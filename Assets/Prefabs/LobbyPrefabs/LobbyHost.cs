using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class LobbyHost : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI HostCodeButtonText = null;
    private ClientConnectionHandler ServerConnection = null;
    void Start()
    {
        ServerConnection = FindObjectOfType<ClientConnectionHandler>();
        RuleServer.RegisterLobby LobbyRequest = new RuleServer.RegisterLobby();
        ServerConnection.SendMessage(LobbyRequest, new System.Action<RuleServer.ServerMessage>(p_HandleCreateLobbyReponse));
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
            }
            else if(Event is RuleServer.LobbyEvent_StatusUpdated)
            {
                RuleServer.LobbyEvent_StatusUpdated StatusEvent = (RuleServer.LobbyEvent_StatusUpdated)Event;
            }
            else if(Event is RuleServer.LobbyEvent_GameStart)
            {
                //start game
                RuleServer.LobbyEvent_GameStart GameStartEvent = (RuleServer.LobbyEvent_GameStart)Event;
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
    public void StartGame()
    {
        print("Starting");
    }


    
    // Update is called once per frame
    void Update()
    {
        if(m_LobbyID != "" && m_LobbyID != HostCodeButtonText.text)
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
