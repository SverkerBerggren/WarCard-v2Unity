using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    static GameState m_GlobalGamestate = null;
    // Start is called before the first frame update
    private RuleManager.RuleManager ruleManager = new RuleManager.RuleManager((uint)10, (uint)10);
    private List<ActionRetriever> m_PlayerActionRetrievers = new List<ActionRetriever>();
    int m_LocalPlayerIndex = 0;
    void Awake()
    {
        if(m_GlobalGamestate != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_GlobalGamestate = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void SetLocalPlayerIndex(int NewIndex)
    {
        m_LocalPlayerIndex = NewIndex;
    }
    int GetLocalPlayerIndex()
    {
        return (m_LocalPlayerIndex);
    }
    public void SetActionRetriever(int PlayerIndex,ActionRetriever NewRetriever)
    {
        for(int i = m_PlayerActionRetrievers.Count; i <= PlayerIndex;i++)
        {
            m_PlayerActionRetrievers.Add(null);
        }
        m_PlayerActionRetrievers[PlayerIndex] = NewRetriever;
    }

    // Update is called once per frame
    void Update()
    {

        int PlayerIndex = ruleManager.GetPlayerActionIndex();
        if(m_PlayerActionRetrievers.Count <= PlayerIndex)
        {
            return;
        }
        if(m_PlayerActionRetrievers[PlayerIndex] == null)
        {
            return;
        }
        if(m_PlayerActionRetrievers[PlayerIndex].getAvailableActions() > 0)
        {
            print("Executing action");
            RuleManager.Action NewAction = m_PlayerActionRetrievers[PlayerIndex].PopAction();
            if(NewAction.PlayerIndex != PlayerIndex)
            {
                print("Invalid player index, abort");
            }
            ruleManager.ExecuteAction(NewAction);
            int CurrentIndex = 0;
            foreach(ActionRetriever Retriever in m_PlayerActionRetrievers)
            {
                if(CurrentIndex == PlayerIndex)
                {
                    continue;
                }
                Retriever.SendAction(NewAction);
                CurrentIndex++;
            }
        }
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
    void SendAction(RuleManager.Action ActionToSend);

}