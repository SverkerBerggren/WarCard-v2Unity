using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // Start is called before the first frame update
    private RuleManager.RuleManager ruleManager = new RuleManager.RuleManager((uint)10, (uint)10);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public RuleManager.RuleManager GetRuleManager()
    {
        return ruleManager;
    }


}
public interface ActionRetriever
{
    int getAvailableActions();

    RuleManager.Action PopAction();

}