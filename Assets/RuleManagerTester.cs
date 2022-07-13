using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleManagerTester : MonoBehaviour
{
    // Start is called before the first frame update

    RuleManager.RuleManager m_ManagerToTest = new RuleManager.RuleManager(42, 30);

    int m_Player1ID = 0;
    int m_Player2ID = 0;

    void Start()
    {
        RuleManager.UnitInfo Player1Knight = Templars.GetKnight();
        Player1Knight.Position = new RuleManager.Coordinate(0, 0);

        RuleManager.UnitInfo Player2Knight = Templars.GetKnight();
        Player2Knight.Position = new RuleManager.Coordinate(5, 5);


        m_Player1ID = m_ManagerToTest.RegisterUnit(Player1Knight, 0);
        m_Player2ID = m_ManagerToTest.RegisterUnit(Player2Knight, 1);
    }

    // Update is called once per frame
    void Update()
    {
        int PlayerIndex = 0;
        int Player1 = m_Player1ID;
        int Player2 = m_Player2ID;

        bool ActionExecuted = false;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Player1 = m_Player2ID;
            Player2 = m_Player1ID;
            PlayerIndex = 1;
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            RuleManager.UnitInfo UnitToMove = m_ManagerToTest.GetUnitInfo(Player1);
            RuleManager.Coordinate NewPosition = new RuleManager.Coordinate(UnitToMove.Position.X, UnitToMove.Position.Y+2);
            RuleManager.MoveAction MoveAction = new RuleManager.MoveAction();
            MoveAction.PlayerIndex = PlayerIndex;
            MoveAction.UnitID = Player1;
            MoveAction.NewPosition = NewPosition;
            m_ManagerToTest.ExecuteAction(MoveAction);
            ActionExecuted = true;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            RuleManager.EffectAction ActionToExecute = new RuleManager.EffectAction();
            ActionToExecute.PlayerIndex = PlayerIndex;
            ActionToExecute.UnitID = Player1;
            ActionToExecute.EffectIndex = 0;
            ActionToExecute.Targets.Add(new RuleManager.Target_Unit(Player2));

            m_ManagerToTest.ExecuteAction(ActionToExecute);

            ActionExecuted = true;
        }
        if (ActionExecuted)
        {
            print("Player1 position: " + m_ManagerToTest.GetUnitInfo(m_Player1ID).Position.X + " " + m_ManagerToTest.GetUnitInfo(m_Player1ID).Position.Y);
            print("Player2 position: " + m_ManagerToTest.GetUnitInfo(m_Player2ID).Position.X + " " + m_ManagerToTest.GetUnitInfo(m_Player2ID).Position.Y);

            print("Player1 HP: " + m_ManagerToTest.GetUnitInfo(m_Player1ID).Stats.HP);
            print("Player2 HP: " + m_ManagerToTest.GetUnitInfo(m_Player2ID).Stats.HP);
        }
    }
}
