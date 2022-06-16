using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRuleManager : MonoBehaviour
{
    // Start is called before the first frame update

    public RuleManager.RuleManager ruleManager = new RuleManager.RuleManager((uint)10,(uint)10); 
    void Start()
    {
        RuleManager.UnitInfo forstaUnit = new RuleManager.UnitInfo();

        forstaUnit.Stats.HP = 10;

        forstaUnit.Position = new RuleManager.Coordinate(0, 0);


        RuleManager.UnitInfo andraUnit = new RuleManager.UnitInfo();

        forstaUnit.Stats.HP = 5;

        forstaUnit.Position = new RuleManager.Coordinate(1, 0);

        ruleManager.RegisterUnit(forstaUnit,0);
        ruleManager.RegisterUnit(andraUnit, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 //   public RuleManager.RuleManager GetRuleManager()
 //   {
 //       return ruleManager; 
 //   }
}
