using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Quit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Prefabs/MainMenu/MainMenu.unity");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
