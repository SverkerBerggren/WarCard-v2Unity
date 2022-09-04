
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class CanvasUiScript : MonoBehaviour
{
    public Image topFrame; 
    public Sprite firstPlayerTopFrame;
    public Sprite secondPlayerTopFrame;
    private bool isFirstPlayerTopFrame = true;

    [SerializeField]
    private ErrorMessageScript errorMessageScript; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void changeTopFrame()
    {
        if(isFirstPlayerTopFrame)
        {
            isFirstPlayerTopFrame = false;
            topFrame.sprite = secondPlayerTopFrame;
        }
        else
        {
            isFirstPlayerTopFrame = true;
            topFrame.sprite = firstPlayerTopFrame; 
        }

    }

    public void errorMessage(string message)
    {
        errorMessageScript.timer = errorMessageScript.originalTimer;
        errorMessageScript.errorMessageTextMesh.text = message;
        errorMessageScript.gameObject.SetActive(true);

    }
}
