using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeaponInfo : Unit
{
    public override RuleManager.UnitInfo CreateUnitInfo()
    {
        return Militarium.GetHeavyWeapons();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
