using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyJoined : MonoBehaviour
{
    // Start is called before the first frame update


    void Start()
    {
        
    }
    string m_LobbyID = "";
    public void Quit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/LobbyScene.unity");
    }
    void p_HandleQueryLobbyEvent(RuleServer.ServerMessage Response)
    {

    }
    void p_HandleSetReadyStatus(RuleServer.ServerMessage Response)
    {

    }
    public void SetLobbyID(string LobbyID)
    {
        m_LobbyID = LobbyID;
    }

    public void OnDestroy()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
