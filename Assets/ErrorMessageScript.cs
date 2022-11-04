using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ErrorMessageScript : MonoBehaviour, IPointerClickHandler
{
    public int timer;

    public int originalTimer;

    public TextMeshProUGUI errorMessageTextMesh;

   // public string errorMessageText;
   // public string errorMessageText;
    // Start is called before the first frame update
    void Start()
    {
        originalTimer = timer;


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            gameObject.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            gameObject.SetActive(false);
        }
    
    }
    private void FixedUpdate()
    {
        timer -= 1; 


        if(timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
