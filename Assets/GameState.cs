using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GlobalNetworkState
{
    public static int LocalPlayerIndex = 0;
    public static List<int> PlayerFactionIndex = new List<int>();
    public static ActionRetriever OpponentActionRetriever = null;

    static GlobalNetworkState()
    {
        PlayerFactionIndex.Add(0);
        PlayerFactionIndex.Add(0);
    }
}


public class GameState : MonoBehaviour
{
    //static GameState m_GlobalGamestate = null;
    // Start is called before the first frame update
    private RuleManager.RuleManager ruleManager = new RuleManager.RuleManager((uint)43, (uint)31);
    private List<ActionRetriever> m_PlayerActionRetrievers = new List<ActionRetriever>();
    int m_LocalPlayerIndex = 0;
    void Awake()
    {
        //if(m_GlobalGamestate != null)
        //{
        //    this.gameObject.SetActive(false);
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    m_GlobalGamestate = this;
        //    DontDestroyOnLoad(this.gameObject);
        //}
    }
    //public void SetLocalPlayerIndex(int NewIndex)
    //{
    //    m_LocalPlayerIndex = NewIndex;
    //}
    //public int GetLocalPlayerIndex()
    //{
    //    print("This is the local player index: " + m_LocalPlayerIndex);
    //    return (m_LocalPlayerIndex);
    //}
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

        int PlayerIndex = ruleManager.getPlayerPriority();
        if(m_PlayerActionRetrievers.Count <= PlayerIndex)
        {
            //print("Invalid number of action retrievers");
            return;
        }
        if(m_PlayerActionRetrievers[PlayerIndex] == null)
        {
            print("Invalid action retriever: was null");
            return;
        }
        if(m_PlayerActionRetrievers[PlayerIndex].getAvailableActions() > 0)
        {
            print("Executing action");
            RuleManager.Action NewAction = m_PlayerActionRetrievers[PlayerIndex].PopAction();
            if(NewAction.PlayerIndex != PlayerIndex)
            {
                print("Invalid player index, abort");
                return;
            }
            ruleManager.ExecuteAction(NewAction);
            int CurrentIndex = 0;
            foreach(ActionRetriever Retriever in m_PlayerActionRetrievers)
            {
                if(CurrentIndex == PlayerIndex)
                {
                    CurrentIndex++;
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

    public int t__GetRetrieverCount()
    {
        return (m_PlayerActionRetrievers.Count);
    }

}
public interface ActionRetriever
{
    int getAvailableActions();

    RuleManager.Action PopAction();
    void SendAction(RuleManager.Action ActionToSend);

}