using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour, RuleManager.RuleEventHandler , ClickReciever, ActionRetriever
{

    RuleManager.RuleManager ruleManager;

    public GridManager gridManager; 

    public GameObject prefabToInstaniate; 

    public Sprite[] theSprites;

    private Dictionary<int, GameObject> listOfImages = new Dictionary<int, GameObject>();

    private Dictionary<int, UIInfo> opaqueIntegerUIInfo = new Dictionary<int, UIInfo>();


    private GameObject unitCard;

    private GameObject unitActions; 


    private int unitEtt;

    private int unitTva;

    public GameObject MovementRange;

    private List<GameObject> CreatedMovementRange = new List<GameObject>();

    public RuleManager.UnitInfo selectedUnit;

    public bool MoveActionSelected = false;

    public bool AttackActionSelected = false;

    public int m_playerid = 0;

    public bool isOnline = false; 

    private Queue<RuleManager.Action> ExecutedActions = new Queue<RuleManager.Action>(); 

  //  public GameObject test; 
    // Start is called before the first frame update

    public void SendAction(RuleManager.Action ActionToSend)
    {

    }

    void Start()
    {
        GameState GlobalState = FindObjectOfType<GameState>();
        ruleManager = GlobalState.GetRuleManager();
        GlobalState.SetActionRetriever(GlobalState.GetLocalPlayerIndex(), this);
        //    gridManager = FindObjectOfType<GridManager>();

        unitCard = GameObject.FindGameObjectWithTag("UnitCard");
        unitCard.SetActive(false);
        unitActions = GameObject.FindGameObjectWithTag("UnitActions");
        unitActions.SetActive(false);


        ruleManager.SetEventHandler(this);
        
        gridManager.SetInputReciever(this);

        UIInfo forstaUIInfo = new UIInfo();
        forstaUIInfo.WhichImage = theSprites[0];
        UIInfo andraUIInfo = new UIInfo();
        forstaUIInfo.WhichImage = theSprites[1];
        opaqueIntegerUIInfo.Add(0, forstaUIInfo);
        opaqueIntegerUIInfo.Add(1, andraUIInfo);

        

        RuleManager.UnitInfo forstaUnit = new RuleManager.UnitInfo();

        forstaUnit.Stats.HP = 100;

        forstaUnit.Stats.Movement = 5;

        forstaUnit.Stats.Range = 3;
        forstaUnit.Stats.ActivationCost = 1;
        forstaUnit.Stats.Damage = 10;

        forstaUnit.Position = new RuleManager.Coordinate(0, 0);

        UIInfo UIForsta = new UIInfo();

        UIForsta.WhichImage = theSprites[0];

        forstaUnit.OpaqueInteger = 0;

        unitEtt = ruleManager.RegisterUnit(forstaUnit, 0);


        GameObject forstaUnitPaKartan =  Instantiate(prefabToInstaniate, gridManager.GetTilePosition(forstaUnit.Position), new Quaternion());
        listOfImages.Add(unitEtt, forstaUnitPaKartan);
        forstaUnitPaKartan.GetComponent<SpriteRenderer>().sprite = theSprites[0];

//        print("unit id forsta  " + forstaUnit.UnitID);

        //    test.transform.position = gridManager.GetTilePosition(forstaUnit.Position);


        RuleManager.UnitInfo andraUnit = new RuleManager.UnitInfo();


        andraUnit.Stats.HP = 1000;

        andraUnit.Stats.Movement = 5;

        andraUnit.Stats.Range = 3;
        andraUnit.Stats.ActivationCost = 1;
        andraUnit.Stats.Damage = 10;

        andraUnit.Position = new RuleManager.Coordinate(2, 0);

        andraUnit.OpaqueInteger = 1; 

      //  print("unit id andsra  " + andraUnit.UnitID);
        unitTva = ruleManager.RegisterUnit(andraUnit, 1);

        GameObject andraUnitPaKartan = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(andraUnit.Position), new Quaternion());
        listOfImages.Add(unitTva, andraUnitPaKartan);
        andraUnitPaKartan.GetComponent<SpriteRenderer>().sprite = theSprites[1];

        
    


    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUnitMove(int UnitID, RuleManager.Coordinate PreviousPosition, RuleManager.Coordinate NewPosition)
    {
        listOfImages[UnitID].transform.position = gridManager.GetTilePosition(NewPosition);


    }


    public void OnUnitAttack(int AttackerID, int DefenderID)
    {

    }

    public void OnUnitDestroyed(int UnitID)
    {
        listOfImages[UnitID].SetActive(false);
    }

    public void OnTurnChange(int CurrentPlayerTurnIndex, int CurrentTurnCount)
    {

    }

    public void OnUnitCreate(RuleManager.UnitInfo NewUnit)
    {

    }

    public void OnClick(ClickType clickType, RuleManager.Coordinate cord)
    {     
     //   int unitId = ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID;
     //   print(" vad blir ubnit id et " + unitId);
     //   if(unitId != 0)
     //   {
     //
     //   }
     //
     //   RuleManager.MoveAction hej = new RuleManager.MoveAction();
     //
     //   hej.UnitID = unitId;
     //   RuleManager.Coordinate temp = ruleManager.GetUnitInfo(unitId).Position;
     //   temp.X += 1; 
     //   hej.NewPosition = temp;
     //   hej.PlayerIndex = 0; 
     //   
     //
     //   ruleManager.ExecuteAction(hej);
     //
     //   listOfImages[unitId].transform.position = gridManager.GetTilePosition(temp);
        if(ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID == 0)
        {
           // RuleManager.UnitInfo unitInfo = ruleManager.GetUnitInfo(ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID);
            if (MoveActionSelected && selectedUnit.PlayerIndex == m_playerid)
            {
                AttackActionSelected = false;
                //  bool isActionValid = false; 
                print(ruleManager.PossibleMoves(selectedUnit.UnitID));
                foreach (RuleManager.Coordinate cords in ruleManager.PossibleMoves(selectedUnit.UnitID))
                {
                    if (cords.X == cord.X && cords.Y == cord.Y)
                    {
                        RuleManager.MoveAction moveAction = new RuleManager.MoveAction();

                        moveAction.NewPosition = cord;
                        moveAction.PlayerIndex = selectedUnit.PlayerIndex;
                        moveAction.UnitID = selectedUnit.UnitID;
                        MoveActionSelected = false;

                        if(!isOnline)
                        {
                            ruleManager.ExecuteAction(moveAction);
                        }
                        else
                        {
                            ExecutedActions.Enqueue(moveAction);
                        }
                        
                        if(!isOnline)
                        {
                         //   listOfImages[selectedUnit.UnitID].transform.position = gridManager.GetTilePosition(cord);
                        }
                      

                        //selectedUnit = null;
                        selectedUnit = null;
                        unitCard.SetActive(false);
                        unitActions.SetActive(false);
                        DestroyMovementRange();
                        return;
                    }
                }

              
            }
        }
        MoveActionSelected = false;
        if (ruleManager.GetTileInfo(cord.X,cord.Y).StandingUnitID != 0)
        {   
            DestroyMovementRange();
            RuleManager.UnitInfo unitInfo = ruleManager.GetUnitInfo(ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID);
       
            if(selectedUnit != null)
            {
                if (AttackActionSelected && selectedUnit.PlayerIndex == m_playerid)
                {
                    RuleManager.AttackAction attackAction = new RuleManager.AttackAction();

                    attackAction.AttackerID = selectedUnit.UnitID;
                    attackAction.DefenderID = unitInfo.UnitID;
                    attackAction.PlayerIndex = m_playerid;
                    string actionInfo;



                    if (ruleManager.ActionIsValid(attackAction, out actionInfo) && selectedUnit.PlayerIndex == m_playerid)
                    {
                        if(!isOnline)
                        {
                            ruleManager.ExecuteAction(attackAction);
                        }
                        else
                        {
                            ExecutedActions.Enqueue(attackAction);
                        }
                        
                        selectedUnit = null;
                        unitCard.SetActive(false);
                        unitActions.SetActive(false);
                        DestroyMovementRange();
                        AttackActionSelected = false;
                        print(actionInfo + "det h�nde");
                        return;
                    }

                    print(actionInfo);
                }
            }
     
            AttackActionSelected = false;

            selectedUnit = unitInfo;
           
            unitCard.SetActive(true);

            unitActions.SetActive(true);

            UnitCardScript unitCardInformation = unitCard.GetComponent<UnitCardScript>();

            unitCardInformation.DamageText.text = unitInfo.Stats.Damage.ToString();
            unitCardInformation.HpText.text = unitInfo.Stats.HP.ToString();
            unitCardInformation.ActivationCostText.text = unitInfo.Stats.ActivationCost.ToString();
            unitCardInformation.MovementText.text = unitInfo.Stats.Movement.ToString();
            unitCardInformation.RangeText.text = unitInfo.Stats.Range.ToString();

            ConstructMovementRange(unitInfo);


        }
        else
        {
            selectedUnit = null; 
            unitCard.SetActive(false);
            unitActions.SetActive(false);
            DestroyMovementRange();
        }
    }
    private void ConstructMovementRange(RuleManager.UnitInfo info)
    {   


        int Height = info.Stats.Movement;

    //    float xPosition = gridManager.GetTilePosition(info.Position).x;
    //    float yPosition = gridManager.GetTilePosition(info.Position).y;
        

        foreach(RuleManager.Coordinate cord in ruleManager.PossibleMoves(info.UnitID))
        {
            GameObject newObject = Instantiate(MovementRange);

            newObject.transform.position = gridManager.GetTilePosition(cord);
            CreatedMovementRange.Add(newObject);
        }
         

    //   for (int YIndex = 0; YIndex < Height; YIndex++)
    //   {
    //       for (int XIndex = 0; XIndex < Height; XIndex++)
    //       {
    //           GameObject NewObject = Instantiate(MovementRange);
    //           //Assumes that tiles are quadratic
    //           float TileWidth = NewObject.GetComponent<SpriteRenderer>().size.x;
    //        //   m_TileWidth = TileWidth;
    //           Vector3 NewPosition = new Vector3(xPosition + XIndex * TileWidth, yPosition - YIndex * TileWidth, 0);
    //         //  GridClick ClickObject = NewObject.GetComponent<GridClick>();
    //         //  ClickObject.X = XIndex;
    //         //  ClickObject.Y = YIndex;
    //         //  ClickObject.AssociatedGrid = this;
    //           NewObject.transform.position = NewPosition;
    //
    //           CreatedMovementRange.Add(NewObject);
    //       }
    //   }
    }

    private void DestroyMovementRange()
    {
        if(CreatedMovementRange.Count != 0)
        {
            foreach(GameObject obj in CreatedMovementRange)
            {
                Destroy(obj);
            }
        }
    }

    private void CreateAbility()
    {

    }

    private void ExecuteAbility(RuleManager.TargetInfo Info)
    {
        RuleManager.TargetInfo targetInfo = Info; 


     //   if()
    }

    public void CreateFriendlyUnitInformation()
    {

    }

    public void DestroyFriendlyUnitInformation()
    {

    }

    public void CreateEnemyUnitInformation()
    {

    }

    public void SwitchPlayerPriority()
    {
        RuleManager.PassAction passAction = new RuleManager.PassAction();

        if(m_playerid == 0)
        {
            passAction.PlayerIndex = 0;
            m_playerid = 1; 
        }
        else
        {
            passAction.PlayerIndex = 1;
            m_playerid = 0;
        }
        if(!isOnline)
        {
            ruleManager.ExecuteAction(passAction);
        }
        else
        {
            ExecutedActions.Enqueue(passAction);
        }
        
    }
    public RuleManager.Action PopAction()
    {
        return ExecutedActions.Dequeue();
    }
    public int getAvailableActions()
    {
        return ExecutedActions.Count;
    }
}



public class UIInfo
{
    public Sprite WhichImage; 
}

