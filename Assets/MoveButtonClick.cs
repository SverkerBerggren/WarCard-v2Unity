using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoveButtonClick : MonoBehaviour
{
    public KeyCode keyCodeToClick; 

    private Button button;

    private ClickHandlerUnitSelect unitSelectClickHandler; 
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        ClickHandlerUnitSelect clickhandlers = FindObjectOfType<ClickHandlerUnitSelect>();
        unitSelectClickHandler = clickhandlers;
        //    GetComponent<Button>().
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyCodeToClick))
        {

            FadeToColor(button.colors.pressedColor);

            button.onClick.Invoke();
        }
        else if(Input.GetKeyUp(keyCodeToClick))
        {

            FadeToColor(button.colors.normalColor);

          //  button.onClick.Invoke();
        }
    }

    private void FadeToColor(Color color)
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.CrossFadeColor(color, gameObject.GetComponent<Button>().colors.fadeDuration, true, true);
    }

    public void moveButtonClick()
    {
        unitSelectClickHandler.ActivateMovementSelection();
    }
}
