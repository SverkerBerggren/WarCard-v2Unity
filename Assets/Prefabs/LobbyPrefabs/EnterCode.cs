using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnterCode : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject LobbyStart = null;
    public GameObject LobbyJoined = null;

    public TextMeshProUGUI ErrorTextElement = null;
    public TextMeshProUGUI LobbyIDElement = null;

    private ClientConnectionHandler m_Connection = null;
    void Start()
    {
        m_Connection = FindObjectOfType<ClientConnectionHandler>();
    }
    
    void p_ClearErrorMessage()
    {
        ErrorTextElement.text = "";
    }
    void p_SetErrorMessage(string NewErrorMessage)
    {
        ErrorTextElement.color = new Color(1, 0, 0);
        ErrorTextElement.text = NewErrorMessage;
    }

    public void Cancel()
    {
        LobbyStart.SetActive(true);
        gameObject.SetActive(false);
        p_ClearErrorMessage();
    }
    string m_RequestLobbyID = "";
    bool m_RequestActive = false;

    string m_ResponseErrorMessage = "";
    void p_HandleJoinResponse(RuleServer.ServerMessage Response)
    {
        print("Response recieved");
        m_RequestActive = false;
        if(Response == null)
        {
            lock(m_ResponseErrorMessage)
            {
                m_ResponseErrorMessage = "Error joining room: error connecting so server";
            }
            return;
        }
        try
        {
            string Status = ((RuleServer.RequestStatusResponse)Response).ErrorString;
            if(Status != "Ok")
            {
                lock (m_ResponseErrorMessage)
                {
                    m_ResponseErrorMessage = "Error joining room: " + Status;
                }
                return;
            }
            LobbyJoined.GetComponent<LobbyJoined>().SetLobbyID(m_RequestLobbyID);
            LobbyJoined.SetActive(true);
            gameObject.SetActive(false);
        }
        catch(System.Exception e)
        {
            lock (m_ResponseErrorMessage)
            {
                m_ResponseErrorMessage = "Error joining room: unkown error";
            }
            print(e.Message);
        }

    }

    public void Join()
    {
        p_ClearErrorMessage();
        if(m_Connection.IsConnected() == false)
        {
            p_SetErrorMessage("Error joining room: error connecting to the server");
            return;
        }
        ErrorTextElement.color = new Color(0, 0, 0);
        ErrorTextElement.text = "Sending message to server...";
        m_RequestActive = true;
        m_RequestLobbyID = LobbyIDElement.text;
        print(m_RequestLobbyID);
        RuleServer.JoinLobby JoinMessage = new RuleServer.JoinLobby();
        JoinMessage.LobbyID = m_RequestLobbyID;
        m_Connection.SendMessage(JoinMessage, new System.Action<RuleServer.ServerMessage>(p_HandleJoinResponse));
    }
    // Update is called once per frame
    void Update()
    {
        lock(m_ResponseErrorMessage)
        {
            if(m_ResponseErrorMessage != "")
            {
                p_SetErrorMessage(m_ResponseErrorMessage);
                m_ResponseErrorMessage = "";
            }
        }
    }
}
