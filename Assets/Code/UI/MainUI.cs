using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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


    public GameObject genericAbilityButton = null;


    private List<GameObject> buttonDestroyList = new List<GameObject>();

    public bool abilitySelectionStarted = false;

    private int currentTargetToSelect = 0; 

    public List<RuleManager.TargetCondition> requiredAbilityTargets = new List<RuleManager.TargetCondition>();

    private List<RuleManager.Target> selectedTargetsForAbilityExecution = new List<RuleManager.Target>();

    public int selectedAbilityIndex = -1;

    private List<GameObject> stackObjectsToDestroy = new List<GameObject>();

    //  public GameObject test; 
    // Start is called before the first frame update

    private Stack<RuleManager.StackEntity> localStack = new Stack<RuleManager.StackEntity>();

    public GameObject imageStackAbility;

    public int stackPadding = 40;

    public int unitPerformingAbility; 
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
        forstaUnit.Abilities.Add(Templars.GetKnight().Abilities[0]);
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

        andraUnit.Abilities.Add(Templars.GetKnight().Abilities[0]);

      //  print("unit id ancdsra  " + andraUnit.UnitID);
        unitTva = ruleManager.RegisterUnit(andraUnit, 1);

        GameObject andraUnitPaKartan = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(andraUnit.Position), new Quaternion());
        listOfImages.Add(unitTva, andraUnitPaKartan);
        andraUnitPaKartan.GetComponent<SpriteRenderer>().sprite = theSprites[1];


        RuleManager.UnitInfo officer = Militarium.GetOfficer();
        officer.Position = new RuleManager.Coordinate(3, 3);
        int officerInt = -1;
        officerInt = ruleManager.RegisterUnit(officer, 0);

        GameObject officerObj = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(officer.Position), new Quaternion());
        listOfImages.Add(officerInt, officerObj);
        officerObj.GetComponent<SpriteRenderer>().sprite = theSprites[0];


    }

    // Update is called once per frame
    void Update()
    {
        m_playerid = ruleManager.getPlayerPriority();
    }

    public void OnUnitMove(int UnitID, RuleManager.Coordinate PreviousPosition, RuleManager.Coordinate NewPosition)
    {
        listOfImages[UnitID].transform.position = gridManager.GetTilePosition(NewPosition);


    }

    public void OnStackPop(RuleManager.StackEntity PoppedEntity)
    {
        localStack.Pop();

        DestroyStackUI();

        if(localStack.Count != 0)
        {
            CreateStackUI();
        }
    }
    public void OnStackPush(RuleManager.StackEntity PushedEntity)
    {
        localStack.Push(PushedEntity);

        DestroyStackUI();

        CreateStackUI();
        
 
    }
    private void CreateStackUI()
    {
        int originalPadding = stackPadding;
        int increasingPadding = originalPadding;
        int amountOfItems = localStack.Count;
        int firstItemPosition;

        RectTransform canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        if(amountOfItems %2 == 0)
        {
            firstItemPosition = stackPadding * ((amountOfItems / 2));//Screen.width - (stackPadding * (amountOfItems / 2)) ;
        }
        else
        {
            firstItemPosition = stackPadding * ((amountOfItems / 2) - 1);//Screen.width - (stackPadding * (amountOfItems / 2)) + (stackPadding/2);
        }


        foreach (RuleManager.StackEntity entity in localStack)
        {
            print("Hur manga saker i stacken " + localStack.Count);
            GameObject createdImage = Instantiate(imageStackAbility,new Vector3() , new Quaternion());
          

            createdImage.GetComponent<ImageAbilityStackScript>().descriptionText.text = "Filler";
            stackObjectsToDestroy.Add(createdImage);
            createdImage.transform.parent = FindObjectOfType<Canvas>().gameObject.transform;

            createdImage.GetComponent<RectTransform>().position = new Vector3((canvas.rect.width/2) - (firstItemPosition - originalPadding + increasingPadding), canvas.rect.height/2);
            increasingPadding += originalPadding;
        }
    }

    private void DestroyStackUI()
    {
        foreach (GameObject obj in stackObjectsToDestroy)
        {
            Destroy(obj);
        }
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
    {   if(selectedUnit != null)
        {
            if (abilitySelectionStarted && selectedUnit.PlayerIndex == ruleManager.getPlayerPriority())
            {   
                if (requiredAbilityTargets.Count == 0)
                {

                    print("laggs abilities till med tom lista");
                    RuleManager.EffectAction abilityToExecute = new RuleManager.EffectAction();

                    abilityToExecute.EffectIndex = selectedAbilityIndex;
                    abilityToExecute.PlayerIndex = ruleManager.getPlayerPriority();
                    abilityToExecute.Targets = new List<RuleManager.Target>();

                    abilityToExecute.UnitID = selectedUnit.UnitID;

                    

                    ruleManager.ExecuteAction(abilityToExecute);



                    resetSelection();

                    return; 
                }
            }

            if (abilitySelectionStarted && selectedUnit.PlayerIndex == ruleManager.getPlayerPriority())
            {
                RuleManager.Target_Tile targetTile = new RuleManager.Target_Tile(cord);
             //   print();
                RuleManager.Target_Unit targetUnit = new RuleManager.Target_Unit();

                if (ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID != 0)
                {
                    targetUnit = new RuleManager.Target_Unit(ruleManager.GetUnitInfo(ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID).UnitID);
                }

                bool targetWasCorrect = false;
            //    print(currentTargetToSelect);
                //   if(ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID == 0 && requiredAbilityTargets[currentTargetToSelect]  == )
                if (ruleManager.p_VerifyTarget(requiredAbilityTargets[currentTargetToSelect], new RuleManager.EffectSource_Unit(selectedUnit.UnitID), targetTile))
                {
                    currentTargetToSelect += 1;
                    targetWasCorrect = true;
                    selectedTargetsForAbilityExecution.Add(targetTile);
                }
                if(currentTargetToSelect < requiredAbilityTargets.Count)
                {
                    if (ruleManager.p_VerifyTarget(requiredAbilityTargets[currentTargetToSelect], new RuleManager.EffectSource_Unit(selectedUnit.UnitID), targetUnit) && !targetWasCorrect)
                    {
                        currentTargetToSelect += 1;
                        targetWasCorrect = true;
                        selectedTargetsForAbilityExecution.Add(targetUnit);
                    }

                }

                if (requiredAbilityTargets.Count == currentTargetToSelect)
                {

                 //   print("Executas ability");

                    RuleManager.EffectAction abilityToExecute = new RuleManager.EffectAction();

                    abilityToExecute.EffectIndex = selectedAbilityIndex;
                    abilityToExecute.PlayerIndex = ruleManager.getPlayerPriority(); ;

                    List<RuleManager.Target> argumentList = new List<RuleManager.Target>(selectedTargetsForAbilityExecution);

                    abilityToExecute.Targets = argumentList;

                    abilityToExecute.UnitID = selectedUnit.UnitID;

                    ruleManager.ExecuteAction(abilityToExecute);

                    resetSelection();

                    print("den gor abiliten");
                    currentTargetToSelect = 0;
                    //
                    return;
                    // abilityToExecute.

                }

                if(targetWasCorrect)
                {
                    return; 
                }

                if (!targetWasCorrect)
                {
                    resetSelection();
                    return;
                }


               // return; 
                //List )
            }
        }
    
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
            if (MoveActionSelected && selectedUnit.PlayerIndex == ruleManager.getPlayerPriority())
            {
                AttackActionSelected = false;
                //  bool isActionValid = false; 
             //   print(ruleManager.PossibleMoves(selectedUnit.UnitID));
                foreach (RuleManager.Coordinate cords in ruleManager.PossibleMoves(selectedUnit.UnitID))
                {
                    if (cords.X == cord.X && cords.Y == cord.Y)
                    {
                        RuleManager.MoveAction moveAction = new RuleManager.MoveAction();

                        moveAction.NewPosition = cord;
                        moveAction.PlayerIndex = selectedUnit.PlayerIndex;
                        moveAction.UnitID = selectedUnit.UnitID;
                        MoveActionSelected = false;

                        print("excecutas move");
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

                        foreach(GameObject obj in buttonDestroyList)
                        {
                            Destroy(obj); // obj.SetActive(false);

                            
                        }
                        buttonDestroyList.Clear();
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
                if (AttackActionSelected && selectedUnit.PlayerIndex == ruleManager.getPlayerPriority())
                {
                    RuleManager.AttackAction attackAction = new RuleManager.AttackAction();

                    attackAction.AttackerID = selectedUnit.UnitID;
                    attackAction.DefenderID = unitInfo.UnitID;
                    attackAction.PlayerIndex = ruleManager.getPlayerPriority(); ;
                    string actionInfo;



                    if (ruleManager.ActionIsValid(attackAction, out actionInfo) && selectedUnit.PlayerIndex == ruleManager.getPlayerPriority())
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
                        foreach (GameObject obj in buttonDestroyList)
                        {
                            obj.SetActive(false);
                        }

                        DestroyMovementRange();
                        AttackActionSelected = false;
                     //   print(actionInfo + "det hnde");
                        return;
                    }

                    print(actionInfo);
                }
            }
     
            AttackActionSelected = false;
            if(selectedUnit != null)
            {
                if (!abilitySelectionStarted && unitInfo.UnitID != selectedUnit.UnitID)
                {
                    foreach (GameObject obj in buttonDestroyList)
                    {
                        obj.SetActive(false);
                    }
                }
            }

            selectedUnit = unitInfo;

            
           
            unitCard.SetActive(true);

            unitActions.SetActive(true);

            UnitCardScript unitCardInformation = unitCard.GetComponent<UnitCardScript>();

            unitCardInformation.DamageText.text = unitInfo.Stats.Damage.ToString();
            unitCardInformation.HpText.text = unitInfo.Stats.HP.ToString();
            unitCardInformation.ActivationCostText.text = unitInfo.Stats.ActivationCost.ToString();
            unitCardInformation.MovementText.text = unitInfo.Stats.Movement.ToString();
            unitCardInformation.RangeText.text = unitInfo.Stats.Range.ToString();
            int padding = 150;
            int ogPadding = padding; 
            int index = 0;

            print(padding);
            foreach(RuleManager.Ability ability in unitInfo.Abilities)
            {   
                //  insta
                GameObject attackButton = GameObject.Find("AttackButton");
                //      genericAbilityButton.SetActive(true);

                print("gar den igenom abilities");

                 GameObject newButton =  Instantiate(genericAbilityButton, new Vector3(attackButton.transform.position.x + padding,attackButton.transform.position.y), new Quaternion());
                padding += ogPadding;
                newButton.transform.parent = GameObject.Find("UnitActions").transform;

                AbilityButton abilityButton = newButton.GetComponent<AbilityButton>();

                abilityButton.abilityIndex = index;

                if(ability is RuleManager.Ability_Activated)
                {
                    RuleManager.Ability_Activated activatedAbility = (RuleManager.Ability_Activated)ability;

                    abilityButton.activatedAbility = true;


                    if(activatedAbility.ActivationTargets is RuleManager.TargetInfo_List)
                    {
                       RuleManager.TargetInfo_List listOfTargets = (RuleManager.TargetInfo_List)activatedAbility.ActivationTargets;

                        abilityButton.whichTargets = listOfTargets.Targets;
                    }

                //    abilityButton.whichTargets = activatedAbility.ActivationTargets.;
                }

                buttonDestroyList.Add(newButton);

                index += 1; 
                
            }
            index = 0; 
            

            ConstructMovementRange(unitInfo);


        }
        else
        {
            selectedUnit = null; 
            unitCard.SetActive(false);
            unitActions.SetActive(false);
            DestroyButtons();

            DestroyMovementRange();
        }
    }

    private void DestroyButtons()
    {
        foreach(GameObject obj in buttonDestroyList)
        {
            Destroy(obj);
            
        }
        buttonDestroyList.Clear();
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

//   private void ExecuteAbility(RuleManager.TargetInfo Info)
//   {
//       RuleManager.TargetInfo targetInfo = Info; 
//
//       
//
//       if(targetInfo is RuleManager.TargetInfo_List)
//       {
//           RuleManager.TargetInfo_List targetInfoList = (RuleManager.TargetInfo_List)targetInfo;
//
//           if(targetInfoList.Targets.Count == 1)
//           {
//               RuleManager.TargetCondition targetType = targetInfoList.Targets[0];        
//
//
//           }
//       }
//
//       
//
//    //   if()
//   }

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


        passAction.PlayerIndex = ruleManager.getPlayerPriority();
   //   if(m_playerid == 0)
   //   {
   //       passAction.PlayerIndex = 0;
   //       m_playerid = 1; 
   //   }
   //   else
   //   {
   //       passAction.PlayerIndex = 1;
   //       m_playerid = 0;
   //   }
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

    public void resetSelection()
    {
        selectedUnit = null;
        unitCard.SetActive(false);
        unitActions.SetActive(false);
        DestroyButtons();

        DestroyMovementRange();

        requiredAbilityTargets.Clear();

        MoveActionSelected = false;
        AttackActionSelected = false;
        abilitySelectionStarted = false;
        currentTargetToSelect = 0;
        selectedTargetsForAbilityExecution = new List<RuleManager.Target>();
    } 
}



public class UIInfo
{
    public Sprite WhichImage; 
}

