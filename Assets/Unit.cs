using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    public virtual RuleManager.UnitInfo CreateUnitInfo()
    {
        return null;
    }

    public virtual UnitSprites GetUnitSidewaySprite()
    {
        UnitSprites hej = new UnitSprites(); 
        return hej;
    }

     

    // Update is called once per frame
    void Update()
    {
        
    }
}
