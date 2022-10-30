using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeaponInfo : Unit
{
    public Sprite backwardSprite = null;
    public Sprite forwardSprite = null;
    public Sprite sideSprite = null;

    public override RuleManager.UnitInfo CreateUnitInfo()
    {
        return Militarium.GetHeavyWeapons();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override UnitSprites GetUnitSidewaySprite()
    {
        UnitSprites ReturnValue = new UnitSprites();

        ReturnValue.backwardSprite = backwardSprite;
        ReturnValue.forwardSprite = forwardSprite;
        ReturnValue.sidewaySprite = sideSprite;

        return (ReturnValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
