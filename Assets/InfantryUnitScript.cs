using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryUnitScript :  Unit
{
    public Sprite sidewaySprite;
    public Sprite forwardSprite;
    public Sprite backWardSprite;

 //   public Sprite unitCardSprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override UnitSprites GetUnitSidewaySprite()
    {
        UnitSprites unitSpritesToReturn = new UnitSprites();
        unitSpritesToReturn.sidewaySprite = sidewaySprite;
        unitSpritesToReturn.forwardSprite = forwardSprite;
        unitSpritesToReturn.backwardSprite = backWardSprite;



        return unitSpritesToReturn;
    }
    public override RuleManager.UnitInfo CreateUnitInfo()
    {
        return Militarium.GetFootSoldier();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
