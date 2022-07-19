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



        print("Statik tests");

        RuleManager.RuleManager StaticTestManager = new RuleManager.RuleManager(42,30);
        RuleManager.UnitInfo Player1SoldierInfo = Militarium.GetFootSoldier();
        RuleManager.UnitInfo Player1OfficerInfo = Militarium.GetOfficer();
        RuleManager.UnitInfo Player1ArilleryInfos = Militarium.GetArtillery();

        RuleManager.UnitInfo Player2SoldierInfo = Militarium.GetFootSoldier();
        Player2SoldierInfo.Position = new RuleManager.Coordinate(10, 0);
        StaticTestManager.RegisterUnit(Player2SoldierInfo, 1);
        Player2SoldierInfo.Position = new RuleManager.Coordinate(10, 1);
        StaticTestManager.RegisterUnit(Player2SoldierInfo, 1);
        Player2SoldierInfo.Position = new RuleManager.Coordinate(10, 2);
        StaticTestManager.RegisterUnit(Player2SoldierInfo, 1);
        Player2SoldierInfo.Position = new RuleManager.Coordinate(10, 3);
        int EnemySoldier = StaticTestManager.RegisterUnit(Player2SoldierInfo, 1);
        Player2SoldierInfo.Position = new RuleManager.Coordinate(10, 4);

        Player1SoldierInfo.Position = new RuleManager.Coordinate(0, 0);
        int Player1Soldier = StaticTestManager.RegisterUnit(Player1SoldierInfo, 0);
        Player1OfficerInfo.Position = new RuleManager.Coordinate(0, 1);
        int Player1Officer = StaticTestManager.RegisterUnit(Player1OfficerInfo, 0);
        Player1ArilleryInfos.Position = new RuleManager.Coordinate(0, 2);
        int Player1Artillery = StaticTestManager.RegisterUnit(Player1ArilleryInfos, 0);

        print("Soldier attack: " + StaticTestManager.GetUnitInfo(Player1Soldier).Stats.Damage);
        StaticTestManager.ExecuteAction(new RuleManager.EffectAction(new RuleManager.Target_Unit(Player1Soldier),Player1Officer,1));
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        print("Soldier attack: " + StaticTestManager.GetUnitInfo(Player1Soldier).Stats.Damage);

        print("Soldier movement: " + StaticTestManager.GetUnitInfo(Player1Soldier).Stats.Movement);
        StaticTestManager.ExecuteAction(new RuleManager.EffectAction(new RuleManager.Target_Unit(Player1Soldier), Player1Officer, 0));
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        print("Soldier movement: " + StaticTestManager.GetUnitInfo(Player1Soldier).Stats.Movement);

        print("Soldier position: " + StaticTestManager.GetUnitInfo(Player1Soldier).Position);
        StaticTestManager.ExecuteAction(new RuleManager.EffectAction(
            new RuleManager.Target[] { new RuleManager.Target_Unit(Player1Soldier), new RuleManager.Target_Tile(new RuleManager.Coordinate(0, 3))}, Player1Officer, 2));
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        print("Soldier position: " + StaticTestManager.GetUnitInfo(Player1Soldier).Position);

        print("Enemy soldier hp: " + StaticTestManager.GetUnitInfo(EnemySoldier).Stats.HP);
        StaticTestManager.ExecuteAction(new RuleManager.EffectAction(
                new RuleManager.Target_Tile( new RuleManager.Coordinate(10,4)),Player1Artillery,0)
            );
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        //Still player 1 turn, pass 2 times on empty stack to end turn
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        print("Enemy soldier hp: " + StaticTestManager.GetUnitInfo(EnemySoldier).Stats.HP);

        //resolve trigger
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        StaticTestManager.ExecuteAction(new RuleManager.PassAction());
        print("Soldier HP: " + StaticTestManager.GetUnitInfo(EnemySoldier).Stats.HP);
        //now the temporary effects should have worn of
        print("Soldier Damage: " + StaticTestManager.GetUnitInfo(Player1Soldier).Stats.Damage);
        print("Soldier Movement: " + StaticTestManager.GetUnitInfo(Player1Soldier).Stats.Movement);
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
        if(Input.GetKeyDown(KeyCode.S))
        {
            m_ManagerToTest.ExecuteAction(new RuleManager.PassAction(PlayerIndex));
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
        if (Input.GetKeyDown(KeyCode.B))
        {
            RuleManager.EffectAction ActionToExecute = new RuleManager.EffectAction();
            ActionToExecute.PlayerIndex = PlayerIndex;
            ActionToExecute.UnitID = Player1;
            ActionToExecute.EffectIndex = 1;
            ActionToExecute.Targets.Add(new RuleManager.Target_Unit(Player1));

            m_ManagerToTest.ExecuteAction(ActionToExecute);

            ActionExecuted = true;
        }
        if (ActionExecuted)
        {
            print("Player1 position: " + m_ManagerToTest.GetUnitInfo(m_Player1ID).Position.X + " " + m_ManagerToTest.GetUnitInfo(m_Player1ID).Position.Y);
            print("Player2 position: " + m_ManagerToTest.GetUnitInfo(m_Player2ID).Position.X + " " + m_ManagerToTest.GetUnitInfo(m_Player2ID).Position.Y);

            print("Player1 HP: " + m_ManagerToTest.GetUnitInfo(m_Player1ID).Stats.HP);
            print("Player2 HP: " + m_ManagerToTest.GetUnitInfo(m_Player2ID).Stats.HP);

            print("Player1 HP: " + m_ManagerToTest.GetUnitInfo(m_Player1ID).Stats.Damage);
            print("Player2 HP: " + m_ManagerToTest.GetUnitInfo(m_Player2ID).Stats.Damage);
        }
    }
}
