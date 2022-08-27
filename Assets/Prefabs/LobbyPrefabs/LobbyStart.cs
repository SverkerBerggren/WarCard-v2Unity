using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbyStart : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject JoinObject = null;
    public GameObject HostObject = null;

    void Start()
    {

    }

    public void Join()
    {
        gameObject.SetActive(false);
        JoinObject.SetActive(true);
    }
    public void Quit()
    {
        SceneManager.LoadScene("Assets/Prefabs/MainMenu/MainMenu.unity");
    }

    public void Host()
    {
        gameObject.SetActive(false);
        HostObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
