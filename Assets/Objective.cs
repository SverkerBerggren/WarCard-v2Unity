using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite neutralSprite;

    public Sprite player1Sprite;

    public Sprite player2Sprite;
    
    public SpriteRenderer spriteRenderer;

    void Start()
    {
    //    spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setNeutralControl()
    {
        spriteRenderer.sprite = neutralSprite;
    }    
    public void setPlayer1Control()
    {
        spriteRenderer.sprite = player1Sprite;
    }    
    public void setPlayer2Control()
    {
        spriteRenderer.sprite = player2Sprite; 
    }

}
