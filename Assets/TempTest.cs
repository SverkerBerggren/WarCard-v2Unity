using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuleManager;

public class TempTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RuleManager.RuleManager Ruler = new RuleManager.RuleManager();
        print(Ruler.GetUnitInfo(new UnitID()).Temp.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
