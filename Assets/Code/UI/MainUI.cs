using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System; 
public class MainUI : MonoBehaviour, RuleManager.RuleEventHandler , ClickReciever, ActionRetriever
{

    public CanvasUiScript canvasUIScript; 

    RuleManager.RuleManager ruleManager;

    public GridManager gridManager; 

    public GameObject prefabToInstaniate; 

    public Sprite[] theSprites;

    private Dictionary<int, UnitSprites> listOfImages = new Dictionary<int, UnitSprites>();

    private Dictionary<int, UIInfo> opaqueIntegerUIInfo = new Dictionary<int, UIInfo>();


    private GameObject unitCard;

    private GameObject unitActions; 


    private int unitEtt;

    private int unitTva;

    public GameObject MovementRange;

    private List<GameObject> CreatedMovementRange = new List<GameObject>();

    private Dictionary<int, Unit> m_OpaqueToUIInfo = new Dictionary<int, Unit>();


    public RuleManager.UnitInfo selectedUnit;

    public bool MoveActionSelected = false;

    public bool AttackActionSelected = false;

    public int m_playerid = 0;

    public bool isOnline = false; 

    private Queue<RuleManager.Action> ExecutedActions = new Queue<RuleManager.Action>();


    public GameObject genericAbilityButton = null;


    private List<GameObject> buttonDestroyList = new List<GameObject>();

    public bool abilitySelectionStarted = false;

    public GameObject errorMessage;

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

    private TextMeshProUGUI initiativeText;

    public Sprite firstPlayerTopFrameSprite;

    public Sprite secondPlayerTopFrameSprite;

    public TextMeshProUGUI currentPlayerTurnText;

    public TextMeshProUGUI currentPlayerPriority;
    public TextMeshProUGUI currentTurnText;
    public TextMeshProUGUI initiativePlayer0;
    public TextMeshProUGUI initiativePlayer1;


    public int unitPerformingAbility; 

    public List<UnitInArmy> firstPlayerArmy;
    public List<UnitInArmy> secondPlayerArmy;
    public List<RuleManager.Coordinate> listOfObjectives;

    private List<List<GameObject>> movementIndicatorObjectDictionary = new List<List<GameObject>>();//new Dictionary<RuleManager.Coordinate, GameObject>();

    TextMeshProUGUI Player1Score;
    TextMeshProUGUI Player2Score;

    [Serializable]
    public struct UnitInArmy
    {
        public Unit unit;

        public RuleManager.Coordinate cord; 
    }

    public void SendAction(RuleManager.Action ActionToSend)
    {

    }


    public void OnRoundChange(int CurrentPriority,int CurrentBattleRound)
    {
        print("Current battle round: " + CurrentBattleRound);
    }


    void Start()
    {
      //  initiativeText = GameObject.Find("InitiativeText").GetComponent<TextMeshProUGUI>();
      //  print(initiativeText);

        GameState GlobalState = FindObjectOfType<GameState>();

        canvasUIScript = FindObjectOfType<CanvasUiScript>();

        ruleManager = GlobalState.GetRuleManager();
         

        GlobalState.SetActionRetriever(GlobalNetworkState.LocalPlayerIndex, this);
        if(GlobalNetworkState.OpponentActionRetriever != null)
        {
            GlobalState.SetActionRetriever(GlobalNetworkState.LocalPlayerIndex == 0 ? 1 : 0, GlobalNetworkState.OpponentActionRetriever);
            GlobalNetworkState.OpponentActionRetriever = null;
        }
        if(GlobalState)

        //    gridManager = FindObjectOfType<GridManager>();

        unitCard = GameObject.FindGameObjectWithTag("UnitCard");
        unitCard.SetActive(false);
        unitActions = GameObject.FindGameObjectWithTag("UnitActions");
        unitActions.SetActive(false);


        ruleManager.SetEventHandler(this);
        
        gridManager.SetInputReciever(this);

        errorMessage = GameObject.Find("ErrorMessage");
        errorMessage.SetActive(false);

        Player1Score = GameObject.Find("Player1Score").GetComponent<TextMeshProUGUI>();
        Player2Score = GameObject.Find("Player2Score").GetComponent<TextMeshProUGUI>();
        //     UIInfo forstaUIInfo = new UIInfo();
        //     forstaUIInfo.WhichImage = theSprites[0];
        //     UIInfo andraUIInfo = new UIInfo();
        //     forstaUIInfo.WhichImage = theSprites[1];
        //     opaqueIntegerUIInfo.Add(0, forstaUIInfo);
        //     opaqueIntegerUIInfo.Add(1, andraUIInfo);
        //
        //     
        //
        //     RuleManager.UnitInfo forstaUnit = new RuleManager.UnitInfo();
        //
        //     forstaUnit.Stats.HP = 100;
        //
        //     forstaUnit.Stats.Movement = 5;
        //
        //     forstaUnit.Stats.Range = 3;
        //     forstaUnit.Stats.ActivationCost = 1;
        //     forstaUnit.Stats.Damage = 10;
        //
        //     forstaUnit.Position = new RuleManager.Coordinate(0, 0);
        //
        //     UIInfo UIForsta = new UIInfo();
        //
        //     UIForsta.WhichImage = theSprites[0];
        //
        //     forstaUnit.OpaqueInteger = 0;
        //     forstaUnit.Abilities.Add(Templars.GetKnight().Abilities[0]);
        //     unitEtt = ruleManager.RegisterUnit(forstaUnit, 0);
        //
        //    
        //
        //
        //     GameObject forstaUnitPaKartan =  Instantiate(prefabToInstaniate, gridManager.GetTilePosition(forstaUnit.Position), new Quaternion());
        //     listOfImages.Add(unitEtt, forstaUnitPaKartan);
        //     forstaUnitPaKartan.GetComponent<SpriteRenderer>().sprite = theSprites[0];
        //
        // //       print("unit id forsta  " + forstaUnit.UnitID);
        //
        //     //    test.transform.position = gridManager.GetTilePosition(forstaUnit.Position);
        //
        //
        //     RuleManager.UnitInfo andraUnit = new RuleManager.UnitInfo();
        //
        //
        //     andraUnit.Stats.HP = 1000;
        //
        //     andraUnit.Stats.Movement = 5;
        //
        //     andraUnit.Stats.Range = 3;
        //     andraUnit.Stats.ActivationCost = 1;
        //     andraUnit.Stats.Damage = 10;
        //
        //    
        //
        //     andraUnit.Position = new RuleManager.Coordinate(2, 0);
        //
        //     andraUnit.OpaqueInteger = 1;
        //
        //     andraUnit.Abilities.Add(Templars.GetKnight().Abilities[0]);
        //
        //   //  print("unit id ancdsra  " + andraUnit.UnitID);
        //     unitTva = ruleManager.RegisterUnit(andraUnit, 1);
        //
        //     GameObject andraUnitPaKartan = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(andraUnit.Position), new Quaternion());
        //     listOfImages.Add(unitTva, andraUnitPaKartan);
        //     andraUnitPaKartan.GetComponent<SpriteRenderer>().sprite = theSprites[1];
        //
        //
        //     RuleManager.UnitInfo officer = Militarium.GetOfficer();
        //     officer.Position = new RuleManager.Coordinate(3, 3);
        //     int officerInt = -1;
        //     officerInt = ruleManager.RegisterUnit(officer, 0);
        //
        //     GameObject officerObj = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(officer.Position), new Quaternion());
        //     listOfImages.Add(officerInt, officerObj);
        //     officerObj.GetComponent<SpriteRenderer>().sprite = theSprites[0];

        CreateMovementObjects();
        CreateArmies();

    }

    // Update is called once per frame
    void Update()
    {
   //     m_playerid = ruleManager.getPlayerPriority();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SwitchPlayerPriority();
        }
    }

    public void OnUnitMove(int UnitID, RuleManager.Coordinate PreviousPosition, RuleManager.Coordinate NewPosition)
    {
        //    listOfImages[UnitID].transform.position = gridManager.GetTilePosition(NewPosition);

        GameObject visualObject = listOfImages[UnitID].objectInScene;
        SpriteRenderer spriteRenderer = visualObject.GetComponent<SpriteRenderer>();
        UnitSprites unitSprites = listOfImages[UnitID];


        int xChange = NewPosition.X - PreviousPosition.X;

        int yChange = NewPosition.Y - PreviousPosition.Y;

        if(Mathf.Abs(xChange) > 3 && Mathf.Abs(yChange) < 3)
        {
            if(xChange > 0)
            {
                spriteRenderer.sprite = unitSprites.sidewaySprite;
                spriteRenderer.flipX = false;
                
            }
            else
            {
                spriteRenderer.sprite = unitSprites.sidewaySprite;
                spriteRenderer.flipX = true;
            }
        }

        if (Mathf.Abs(yChange) > 3 && Mathf.Abs(xChange) < 3)
        {
            if (yChange > 0)
            {
                spriteRenderer.sprite = unitSprites.backwardSprite;
            
            }
            else
            {
                spriteRenderer.sprite = unitSprites.forwardSprite;
                
            }
        }
        bool xChangeIsBigEnough = Mathf.Abs(xChange) > 3;
        bool yChangeIsBigEnough = Mathf.Abs(yChange) > 3;
        bool xChangeIsBigger = Mathf.Abs(xChange) > Mathf.Abs(yChange) ;

        if(xChangeIsBigger && xChangeIsBigEnough)
        {
            if (xChange > 0)
            {
                spriteRenderer.sprite = unitSprites.sidewaySprite;
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.sprite = unitSprites.sidewaySprite;
                spriteRenderer.flipX = true;
            }
        }
        else if(yChangeIsBigEnough)
        {
            if (yChange > 0)
            {
                spriteRenderer.sprite = unitSprites.backwardSprite;

            }
            else
            {
                spriteRenderer.sprite = unitSprites.forwardSprite;

            }
        }

        visualObject.transform.position  = gridManager.GetTilePosition(NewPosition);

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

    public void OnPlayerPassPriority(int currentPlayerString)
    {
        currentPlayerPriority.text = "Current Player priority: " + (currentPlayerString+1);
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
        if (localStack.Count == 2) ;

        foreach (RuleManager.StackEntity entity in localStack)
        {
            print("Hur manga saker i stacken " + localStack.Count);
            GameObject createdImage = Instantiate(imageStackAbility,new Vector3() , new Quaternion());
          

            createdImage.GetComponent<ImageAbilityStackScript>().descriptionText.text = entity.EffectToResolve.GetText();
            stackObjectsToDestroy.Add(createdImage);
            createdImage.transform.SetParent(GameObject.Find("StackAbilityHolder").transform);//FindObjectOfType<Canvas>().gameObject.transform;

            if(entity.Source is RuleManager.EffectSource_Unit)
            {
                RuleManager.EffectSource_Unit Source = (RuleManager.EffectSource_Unit)entity.Source;
                if(Source.EffectIndex != -1)
                {
                    createdImage.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = m_OpaqueToUIInfo[ruleManager.GetUnitInfo(Source.UnitID).OpaqueInteger].AbilityIcons[Source.EffectIndex].texture;
                    
                }
                print("Stack entity effect: " + entity.EffectToResolve.GetText());
            }
            
           // createdImage.GetComponent<RectTransform>().position = new Vector3((canvas.rect.width/2) - (firstItemPosition - originalPadding + increasingPadding), canvas.rect.height/2);
            increasingPadding += originalPadding;
        }
        GameObject.Find("StackAbilityHolder").GetComponent<UnitActions>().sortChildren();
    }

    private void DestroyStackUI()
    {
        foreach (GameObject obj in stackObjectsToDestroy)
        {
            obj.transform.SetParent(null);
            Destroy(obj);
        }

        stackObjectsToDestroy.Clear();
    }

    public void OnScoreChange(int PlayerIndex,int NewScore)
    {
        print("Player index: " + PlayerIndex + " Player score: " + NewScore);

        if(PlayerIndex == 0)
        {
            Player1Score.text = "Player 1 Score: " + NewScore;
        }
        else
        {
            Player2Score.text = "Player 2 Score: " + NewScore;
        }
    }

    public void OnUnitAttack(int AttackerID, int DefenderID)
    {

    }

    public void OnUnitDestroyed(int UnitID)
    {
        listOfImages[UnitID].objectInScene.SetActive(false);
    }

    public void OnTurnChange(int CurrentPlayerTurnIndex, int CurrentTurnCount)
    {
        //    currentPlayerTurnText.text = "Current Player: " + (CurrentPlayerTurnIndex +1);

        canvasUIScript.changeTopFrame();

        currentTurnText.text = "Turn: " + CurrentTurnCount;
    }

    public void OnUnitCreate(RuleManager.UnitInfo NewUnit)
    {

    }

    public void OnClick(ClickType clickType, RuleManager.Coordinate cord)
    {
    //    if(GameObject.Find("UnitActions") != null)
    //    {
    //        GameObject.Find("UnitActions").GetComponent<UnitActions>().sortChildren();
    //    }
        if (selectedUnit != null)
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


                    string errorMessageText = "";

                    if (!ruleManager.ActionIsValid(abilityToExecute, out errorMessageText))
                    {
                        ErrorMessageScript script = errorMessage.GetComponent<ErrorMessageScript>();
                        script.timer = script.originalTimer;

                        script.errorMessageTextMesh.text = errorMessageText;

                        errorMessage.SetActive(true);


                    }
                    else
                    {
                        if (!isOnline)
                        {
                            ruleManager.ExecuteAction(abilityToExecute);
                        }
                        else
                        {
                            ExecutedActions.Enqueue(abilityToExecute);
                        }
                    }



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

                //FIX ME
                List<RuleManager.Target> EmptyTargets = new List<RuleManager.Target>(selectedTargetsForAbilityExecution);

                RuleManager.EffectSource_Unit EffectSource = new RuleManager.EffectSource_Unit(ruleManager.GetUnitInfo(selectedUnit.UnitID).PlayerIndex,selectedUnit.UnitID,selectedAbilityIndex);
                if (ruleManager.p_VerifyTarget(requiredAbilityTargets[currentTargetToSelect], EffectSource, EmptyTargets, targetTile))
                {
                    currentTargetToSelect += 1;
                    targetWasCorrect = true;
                    selectedTargetsForAbilityExecution.Add(targetTile);
                }
                if(currentTargetToSelect < requiredAbilityTargets.Count)
                {
                    if (ruleManager.p_VerifyTarget(requiredAbilityTargets[currentTargetToSelect], EffectSource, EmptyTargets, targetUnit) && !targetWasCorrect)
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


                    string errorMessageText = "";

                    if (!ruleManager.ActionIsValid(abilityToExecute, out errorMessageText))
                    {
                        ErrorMessageScript script = errorMessage.GetComponent<ErrorMessageScript>();
                        script.timer = script.originalTimer;

                        script.errorMessageTextMesh.text = errorMessageText;

                        errorMessage.SetActive(true);


                    }
                    else
                    {
                        if (!isOnline)
                        {
                            ruleManager.ExecuteAction(abilityToExecute);
                        }
                        else
                        {
                            ExecutedActions.Enqueue(abilityToExecute);
                         
                        }
                    }
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
                    canvasUIScript.errorMessage("Wrong target, you needed to select a " + requiredAbilityTargets[currentTargetToSelect]);
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
                        string errorMessageText = ""; 
                   
                        if(!ruleManager.ActionIsValid(moveAction,out errorMessageText))
                        {
                            ErrorMessageScript script = errorMessage.GetComponent<ErrorMessageScript>();
                            script.timer = script.originalTimer;

                            script.errorMessageTextMesh.text = errorMessageText;

                            errorMessage.SetActive(true);

                            
                        }
                        else
                        {
                            if (!isOnline)
                            {
                                ruleManager.ExecuteAction(moveAction);
                            }
                            else
                            {
                                ExecutedActions.Enqueue(moveAction);
                            }
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

                        resetSelection();
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
                        string errorMessageText = "";

                        if (!ruleManager.ActionIsValid(attackAction, out errorMessageText))
                        {
                            ErrorMessageScript script = errorMessage.GetComponent<ErrorMessageScript>();
                            script.timer = script.originalTimer;

                            script.errorMessageTextMesh.text = errorMessageText;

                            errorMessage.SetActive(true);


                        }
                        else
                        {
                            if (!isOnline)
                            {
                                ruleManager.ExecuteAction(attackAction);
                            }
                            else
                            {
                                ExecutedActions.Enqueue(attackAction);
                            }
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

            resetSelection();

            selectedUnit = unitInfo;

            
           
            unitCard.SetActive(true);

            unitActions.SetActive(true);

            UnitCardScript unitCardInformation = unitCard.GetComponent<UnitCardScript>();

            unitCardInformation.DamageText.text = unitInfo.Stats.Damage.ToString();
            unitCardInformation.HpText.text = unitInfo.Stats.HP.ToString();
            unitCardInformation.ActivationCostText.text = unitInfo.Stats.ActivationCost.ToString();
            unitCardInformation.MovementText.text = unitInfo.Stats.Movement.ToString();
            unitCardInformation.RangeText.text = unitInfo.Stats.Range.ToString();

            unitCardInformation.gameObject.GetComponent<Image>().sprite = m_OpaqueToUIInfo[unitInfo.OpaqueInteger].unitCardSprite; 

            

            int padding = 150;
            int ogPadding = padding; 
            int index = 0;

            print(padding);
         //   print("hur manga barn innan destroy " + GameObject.Find("UnitActions").transform.childCount);
            DestroyButtons();
            //   print("hur manga barn efter destroy " + GameObject.Find("UnitActions").transform.childCount);
        //    GameObject.Find("UnitActions").GetComponent<UnitActions>().clearAbilityButtons();
            foreach (RuleManager.Ability ability in unitInfo.Abilities)
            {   
                //  insta
                GameObject attackButton = GameObject.Find("AttackButton");
                //      genericAbilityButton.SetActive(true);

                print("gar den igenom abilities");

                GameObject newButton =  Instantiate(genericAbilityButton, new Vector3(attackButton.transform.position.x + padding,attackButton.transform.position.y,0), new Quaternion());
                padding += ogPadding;

                newButton.transform.SetParent(GameObject.Find("UnitActions").transform);
            //    newButton.transform.parent = GameObject.Find("UnitActions").transform;
                
                AbilityButton abilityButton = newButton.GetComponent<AbilityButton>();

                abilityButton.abilityIndex = index;

                abilityButton.abilityFlavour = ability.GetFlavourText();

                abilityButton.abilityDescription = ability.GetDescription();

                print("vad returnerar getName " + ability.GetName());

                abilityButton.abilityName = ability.GetName();


                if (ability is RuleManager.Ability_Activated)
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
            print("hur manga barn innan sort " + GameObject.Find("UnitActions").transform.childCount);
            GameObject.Find("UnitActions").GetComponent<UnitActions>().sortChildren();
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
            obj.transform.SetParent(null);
            Destroy(obj);
            
        }
        buttonDestroyList.Clear();
    }

    public void OnInitiativeChange(int newIntitiative, int whichPlayer)
    {
      //  currentInitiative.text = "Current Initiative " + newIntitiative + "/15";

        if(whichPlayer == 0)
        {
            initiativePlayer0.text = "Player 1 Initiative " + newIntitiative + "/100";
        }
        if (whichPlayer == 1)
        {
            initiativePlayer1.text = "Player 2 Initiative " + newIntitiative + "/100";
        }
    }
    private void ConstructMovementRange(RuleManager.UnitInfo info)
    {   


        int Height = info.Stats.Movement;

    //    float xPosition = gridManager.GetTilePosition(info.Position).x;
    //    float yPosition = gridManager.GetTilePosition(info.Position).y;
        

        foreach(RuleManager.Coordinate cord in ruleManager.PossibleMoves(info.UnitID))
        {
            //   GameObject newObject = Instantiate(MovementRange);
            //
            //   newObject.transform.position = gridManager.GetTilePosition(cord);
            //   CreatedMovementRange.Add(newObject);
            movementIndicatorObjectDictionary[cord.X][cord.Y].SetActive(true);
           // movementIndicatorObjectDictionary[cord].SetActive(true);

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
            
            
      
            foreach(List<GameObject> obj in movementIndicatorObjectDictionary)
            {
                //obj.SetActive(false);

                foreach(GameObject ob in obj)
                {
                    ob.SetActive(false);
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
        string errorMessageText = "";

        if (!ruleManager.ActionIsValid(passAction, out errorMessageText))
        {
            ErrorMessageScript script = errorMessage.GetComponent<ErrorMessageScript>();
            script.timer = script.originalTimer;

            script.errorMessageTextMesh.text = errorMessageText;

            errorMessage.SetActive(true);


        }
        else
        {
            if (!isOnline)
            {
                ruleManager.ExecuteAction(passAction);
            }
            else
            {
                ExecutedActions.Enqueue(passAction);
            }
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


    private void CreateArmies()
    {

        Dictionary<string, int> UnitOpaqueIDMap = new Dictionary<string, int>();
        int CurrentUnitOpaqueID = 0;
        foreach(UnitInArmy unitFromList in firstPlayerArmy)
        {
  
            RuleManager.UnitInfo unitToCreate = unitFromList.unit.CreateUnitInfo();
          
            if(!UnitOpaqueIDMap.ContainsKey(unitFromList.unit.GetType().Name))
            {
                UnitOpaqueIDMap.Add(unitFromList.unit.GetType().Name, CurrentUnitOpaqueID);
                m_OpaqueToUIInfo[CurrentUnitOpaqueID] = unitFromList.unit;
                CurrentUnitOpaqueID += 1;
            }
            unitToCreate.OpaqueInteger = UnitOpaqueIDMap[unitFromList.unit.GetType().Name];


            unitToCreate.Position = unitFromList.cord;
           
         

            int unitInt = ruleManager.RegisterUnit(unitToCreate, 0);
            UnitSprites unitSprites = unitFromList.unit.GetUnitSidewaySprite();

            GameObject unitToCreateVisualObject = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(unitToCreate.Position), new Quaternion());

            unitSprites.objectInScene = unitToCreateVisualObject;

            listOfImages.Add(unitInt, unitSprites);
            unitToCreateVisualObject.GetComponent<SpriteRenderer>().sprite = unitSprites.sidewaySprite;

        }
        foreach (UnitInArmy unitFromList in secondPlayerArmy)
        {
            RuleManager.UnitInfo unitToCreate = unitFromList.unit.CreateUnitInfo();

            if (!UnitOpaqueIDMap.ContainsKey(unitFromList.unit.GetType().Name))
            {
                UnitOpaqueIDMap.Add(unitFromList.unit.GetType().Name, CurrentUnitOpaqueID);
                m_OpaqueToUIInfo[CurrentUnitOpaqueID] = unitFromList.unit;
                CurrentUnitOpaqueID += 1;
            }
            unitToCreate.OpaqueInteger = UnitOpaqueIDMap[unitFromList.unit.GetType().Name];

            unitToCreate.Position = unitFromList.cord;

            int unitInt = ruleManager.RegisterUnit(unitToCreate, 1);

            UnitSprites unitSprites = unitFromList.unit.GetUnitSidewaySprite();

            GameObject unitToCreateVisualObject = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(unitToCreate.Position), new Quaternion());

            unitSprites.objectInScene = unitToCreateVisualObject;

            listOfImages.Add(unitInt, unitSprites);
            unitToCreateVisualObject.GetComponent<SpriteRenderer>().sprite = unitSprites.sidewaySprite;

        }


        foreach(RuleManager.Coordinate cord in listOfObjectives)
        {
            ruleManager.GetTileInfo(cord.X,cord.Y).HasObjective = true;

            GameObject objectiveImage = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(cord), new Quaternion());
            objectiveImage.GetComponent<SpriteRenderer>().sprite = theSprites[0];

        }
        //   RuleManager.UnitInfo officer = Militarium.GetOfficer();
        //   officer.Position = new RuleManager.Coordinate(3, 3);
        //   int officerInt = -1;
        //   officerInt = ruleManager.RegisterUnit(officer, 0);
        //
        //   GameObject officerObj = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(officer.Position), new Quaternion());
        //   listOfImages.Add(officerInt, officerObj); 
        //   officerObj.GetComponent<SpriteRenderer>().sprite = theSprites[0];

    }

    private void CreateMovementObjects()
    {
        for(int i = 0; i < gridManager.Width; i++)
        {
            movementIndicatorObjectDictionary.Add(new List<GameObject>());

            for(int z = 0; z < gridManager.Height; z++)
            {
                movementIndicatorObjectDictionary[i].Add(null);
            }
        }

        print(gridManager.Width);
        for(int i = 0; i < gridManager.Width; i++)
        {
            for(int z = 0; z < gridManager.Height; z++)
            {
                GameObject newObject = Instantiate(MovementRange);
                RuleManager.Coordinate tempCord = new RuleManager.Coordinate(i, z);

            //    print(tempCord.X + " " + tempCord.Y);
                newObject.transform.position = gridManager.GetTilePosition(tempCord);
                movementIndicatorObjectDictionary[i][z] = newObject;
                newObject.SetActive(false);
            }
        }
    }
}

public struct UnitSprites
{
    public Sprite forwardSprite;

    public Sprite backwardSprite;

    public Sprite sidewaySprite;

    public GameObject objectInScene;
}


public class UIInfo
{
    public Sprite WhichImage; 
}

