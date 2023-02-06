using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Xml;
using RuleManager;



public interface UIAnimation
{
    void Initialize();
    bool IsFinished();
    void Increment(float DeltaTime);
    void Finish();
}



public class MainUI : MonoBehaviour, RuleManager.RuleEventHandler , ClickReciever, ActionRetriever
{
    class DestroyUnitAnimation : UIAnimation
    {

    }
    
    class MoveCameraAnimation : UIAnimation
    {
        //unit vector
        GameObject m_CameraToModify = null;
        Vector2 m_Direction;
        Vector2 m_TargetLocation;
        float m_Margin = 0.05f;
        float m_Speed = 0;

        public void Increment(float DeltaTime)
        {
            float SpeedSize = 
        }
        public bool IsFinished()
        {
            return ((m_TargetLocation - (Vector2)m_CameraToModify.gameObject.transform.position).magnitude <= m_Margin);
        }
        public void Finish()
        {

        }
        public void Initialize()
        {

        }

        public MoveCameraAnimation(GameObject Camera, Vector2 TargetLocation,Vector2 CurrentLocation,float TargetDuration)
        {
            m_CameraToModify = Camera;
            m_Direction = (TargetLocation - CurrentLocation).normalized;
            m_TargetLocation = TargetLocation;
            m_Speed = (TargetLocation - CurrentLocation).magnitude / TargetDuration;
        }
    }
    class AttackAnimation : UIAnimation
    {
        MainUI m_AssociatedUI = null;

        double m_TotalTime = 0;
        double m_ElapsedTime = 0;
        int m_AttackingUnit = 0;
        int m_DefendingUnit = 0;
        UnityEngine.Video.VideoPlayer m_AssociatedRender = null;
        public AttackAnimation(MainUI AsssociatedUI, int AttackingUnit, int DefendingUnit)
        {
            m_AssociatedUI = AsssociatedUI;
            m_AttackingUnit = AttackingUnit;
            m_DefendingUnit = DefendingUnit;
        }
        public void Initialize()
        {
            UnitSceneUIInfo UnitInfo = m_AssociatedUI.m_UnitTypeUIInfo[m_AssociatedUI.listOfImages[m_AttackingUnit].Resource.Name];
            GameObject SceneObject = m_AssociatedUI.listOfImages[m_AttackingUnit].objectInScene;
            ResourceManager.UnitResource Resource = m_AssociatedUI.listOfImages[m_AttackingUnit].Resource;
            if (Resource.UIInfo.AttackAnimation == null)
            {
                return;
            }
            UnityEngine.Video.VideoPlayer Renderer = SceneObject.GetComponent<UnityEngine.Video.VideoPlayer>();
            if (Renderer == null)
            {
                Renderer = SceneObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            }
            Renderer.url = ((ResourceManager.Visual_Video)Resource.UIInfo.AttackAnimation.VisualInfo).VideoURL;
            Renderer.Play();
            m_AssociatedRender = Renderer;
            m_TotalTime = Renderer.length;
        }
        public void Finish()
        {
            UnitSceneUIInfo UnitInfo = m_AssociatedUI.m_UnitTypeUIInfo[m_AssociatedUI.listOfImages[m_AttackingUnit].Resource.Name];
            GameObject SceneObject = m_AssociatedUI.listOfImages[m_AttackingUnit].objectInScene;
            ResourceManager.UnitResource Resource = m_AssociatedUI.listOfImages[m_AttackingUnit].Resource;
            if (Resource.UIInfo.AttackAnimation == null)
            {
                return;
            }
            Destroy(SceneObject.GetComponent<UnityEngine.Video.VideoPlayer>());
        }
        public bool IsFinished()
        {
            return (m_TotalTime != 0 && m_ElapsedTime >= m_TotalTime);
        }
        public void Increment(float DeltaTime)
        {
            m_TotalTime = m_AssociatedRender.length;
            m_ElapsedTime += DeltaTime;
        }
    }
    private MapCamera m_ActiveCamera = null;
    public AudioClip PlaceUnitSound = null;
    public List<GameObject> clickHandlerPrefabs = new List<GameObject>();

    private List<ClickHandler> clickHandlers    = new List<ClickHandler>();

    public CanvasUiScript canvasUIScript; 

    public RuleManager.RuleManager ruleManager;

    public GridManager gridManager;

    public GameObject objectiveContestionIndicator; 

    public GameObject prefabToInstaniate; 

    public GameObject activationIndicatorPrefab;

    private Dictionary<string, UnitSceneUIInfo> m_UnitTypeUIInfo = new Dictionary<string, UnitSceneUIInfo>();
    private Queue<UIAnimation> m_ActiveAnimations = new Queue<UIAnimation>();



    private Dictionary<int, UnitSceneInfo> listOfImages = new Dictionary<int, UnitSceneInfo>();

    private Dictionary<int, List<GameObject>> listOfActivationIndicators = new Dictionary<int, List<GameObject>>();



    public Dictionary<int, Unit> m_OpaqueToUIInfo = new Dictionary<int, Unit>();



    public int m_playerid = 0; 

    public Queue<RuleManager.Action> ExecutedActions = new Queue<RuleManager.Action>();



    private Dictionary<Coordinate,Objective> dictionaryOfObjectiveCords = new Dictionary<Coordinate, Objective>();

    public ClickHandler clickHandler; 



    private List<GameObject> stackObjectsToDestroy = new List<GameObject>();

    //  public GameObject test; 
    // Start is called before the first frame update

    private Stack<RuleManager.StackEntity> localStack = new Stack<RuleManager.StackEntity>();

    public GameObject imageStackAbility;

    public int stackPadding = 40;

    public TextMeshProUGUI currentPlayerPriority;
    public TextMeshProUGUI currentTurnText;
    public TextMeshProUGUI initiativePlayer0;
    public TextMeshProUGUI initiativePlayer1;



    public List<UnitInArmy> firstPlayerArmy;
    public List<UnitInArmy> secondPlayerArmy;
    public List<RuleManager.Coordinate> listOfObjectives;

   



    public bool alwaysPassPriority = false; 

    

    public GameObject EndScreenUI = null;
    public Sprite WinSprite = null;
    public Sprite DefeatSprite = null;
    public GameObject objectivePrefab; 

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
        gridManager = FindObjectOfType<GridManager>();

        //  initiativeText = GameObject.Find("InitiativeText").GetComponent<TextMeshProUGUI>();
        //  print(initiativeText);
        m_ActiveCamera = FindObjectOfType<MapCamera>();

        GameState GlobalState = FindObjectOfType<GameState>();

        canvasUIScript = FindObjectOfType<CanvasUiScript>();

        ruleManager = GlobalState.GetRuleManager();
         
        

        GlobalState.SetActionRetriever(GlobalNetworkState.LocalPlayerIndex, this);
        if(GlobalNetworkState.OpponentActionRetriever != null)
        {
            GlobalState.SetActionRetriever(GlobalNetworkState.LocalPlayerIndex == 0 ? 1 : 0, GlobalNetworkState.OpponentActionRetriever);
            GlobalNetworkState.OpponentActionRetriever = null;
        }
        else
        {
            GlobalNetworkState.IsLocal = true;
            GlobalState.SetActionRetriever(GlobalNetworkState.LocalPlayerIndex == 0 ? 1 : 0, this);
        }

            //    gridManager = FindObjectOfType<GridManager>();

 

        ruleManager.SetEventHandler(this);

        gridManager.SetInputReciever(this);

        foreach (GameObject obj in clickHandlerPrefabs)
        {
           GameObject newClickHandler =  Instantiate(obj, new Vector3(), new Quaternion());

            clickHandlers.Add(newClickHandler.GetComponent<ClickHandler>());
            newClickHandler.GetComponent<ClickHandler>().Setup(this);
        }


        CreateArmies();

    }

    // Update is called once per frame
    int m_PreviousUICount = 0;
    void Update()
    {
   //     m_playerid = ruleManager.getPlayerPriority();

        if(m_ActiveAnimations.Count != m_PreviousUICount && m_ActiveAnimations.Count > 0)
        {
            m_ActiveAnimations.Peek().Initialize();
        }
        m_PreviousUICount = m_ActiveAnimations.Count;
        if (m_ActiveAnimations.Count > 0 && m_ActiveAnimations.Peek().IsFinished())
        {
            m_ActiveAnimations.Peek().Finish();
            m_ActiveAnimations.Dequeue();
        }
        if (m_ActiveAnimations.Count  > 0)
        {
            m_ActiveAnimations.Peek().Increment(Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SwitchPlayerPriority();
        }

        foreach(int unitID in listOfActivationIndicators.Keys)
        { 
            if((ruleManager.GetUnitInfo(unitID).Flags & RuleManager.UnitFlags.IsActivated) != 0 && ruleManager.GetUnitInfo(unitID).PlayerIndex == 0)
            {
                foreach(GameObject Tile in listOfActivationIndicators[unitID])
                {
                    Tile.GetComponent<SpriteRenderer>().color = new Color(0f,0.7f,0f,0.7f);
                }
            }
            if ((ruleManager.GetUnitInfo(unitID).Flags & RuleManager.UnitFlags.IsActivated) == 0 && ruleManager.GetUnitInfo(unitID).PlayerIndex == 0)
            {
                foreach (GameObject Tile in listOfActivationIndicators[unitID])
                {
                    Tile.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 1f);
                }
            }  
            if((ruleManager.GetUnitInfo(unitID).Flags & RuleManager.UnitFlags.IsActivated) != 0 && ruleManager.GetUnitInfo(unitID).PlayerIndex == 1)
            {
                foreach (GameObject Tile in listOfActivationIndicators[unitID])
                {
                    Tile.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0f, 0f, 0.7f);
                }
            }
            if ((ruleManager.GetUnitInfo(unitID).Flags & RuleManager.UnitFlags.IsActivated) == 0 && ruleManager.GetUnitInfo(unitID).PlayerIndex == 1)
            {
                foreach (GameObject Tile in listOfActivationIndicators[unitID])
                {
                    Tile.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
                }
            }

        }


        foreach(Coordinate cord in dictionaryOfObjectiveCords.Keys)
        {
            if (ruleManager.GetObjectiveContestion(cord)[0] > ruleManager.GetObjectiveContestion(cord)[1])
            {
                dictionaryOfObjectiveCords[cord].setPlayer1Control();
            }
            if(ruleManager.GetObjectiveContestion(cord)[0] < ruleManager.GetObjectiveContestion(cord)[1])
            {
                dictionaryOfObjectiveCords[cord].setPlayer2Control();
            }
            if(ruleManager.GetObjectiveContestion(cord)[0] == ruleManager.GetObjectiveContestion(cord)[1])
            {
                dictionaryOfObjectiveCords[cord].setNeutralControl();
            }
        }
    }
    public void OnUnitRotation(int UnitID, RuleManager.Coordinate NewRotation)
    {
        UnitInfo MovedUnit = ruleManager.GetUnitInfo(UnitID);
        for (int i = 0; i < MovedUnit.UnitTileOffsets.Count; i++)
        {
            Coordinate TilePos = MovedUnit.TopLeftCorner + MovedUnit.UnitTileOffsets[i];
            listOfActivationIndicators[UnitID][i].transform.position = gridManager.GetTilePosition(TilePos);
        }
    }
    public void OnUnitMove(int UnitID, RuleManager.Coordinate PreviousPosition, RuleManager.Coordinate NewPosition)
    {
        //    listOfImages[UnitID].transform.position = gridManager.GetTilePosition(NewPosition);
        GetComponent<AudioSource>().PlayOneShot(PlaceUnitSound);
        GameObject visualObject = listOfImages[UnitID].objectInScene;
        SpriteRenderer spriteRenderer = visualObject.GetComponent<SpriteRenderer>();
        ResourceManager.UnitResource unitSprites = listOfImages[UnitID].Resource;

        spriteRenderer.sortingOrder = p_GetSortingOrder(PreviousPosition);

        int xChange = NewPosition.X - PreviousPosition.X;

        int yChange = NewPosition.Y - PreviousPosition.Y;

        if(Mathf.Abs(xChange) > 3 && Mathf.Abs(yChange) < 3)
        {
            if(xChange > 0)
            {
                spriteRenderer.flipX = false;
                
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }

        if (Mathf.Abs(yChange) > 3 && Mathf.Abs(xChange) < 3)
        {
            if (yChange > 0)
            {
                spriteRenderer.sprite = m_UnitTypeUIInfo[unitSprites.Name].UpSprite;
            }
            else
            {
                spriteRenderer.sprite = m_UnitTypeUIInfo[unitSprites.Name].DownSprite;
                //spriteRenderer.sprite = unitSprites.forwardSprite;
            }
        }
        bool xChangeIsBigEnough = Mathf.Abs(xChange) > 3;
        bool yChangeIsBigEnough = Mathf.Abs(yChange) > 3;
        bool xChangeIsBigger = Mathf.Abs(xChange) > Mathf.Abs(yChange) ;

        if(xChangeIsBigger && xChangeIsBigEnough)
        {
            if (xChange > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
        //else if(yChangeIsBigEnough)
        //{
        //    if (yChange > 0)
        //    {
        //        spriteRenderer.sprite = unitSprites.backwardSprite;
        //
        //    }
        //    else
        //    {
        //        spriteRenderer.sprite = unitSprites.forwardSprite;
        //
        //    }
        //}
        UnitInfo MovedUnit = ruleManager.GetUnitInfo(UnitID);
        visualObject.transform.position  = gridManager.GetTilePosition(NewPosition);
        for(int i = 0; i < MovedUnit.UnitTileOffsets.Count;i++)
        {
            Coordinate TilePos = NewPosition + MovedUnit.UnitTileOffsets[i];
            listOfActivationIndicators[UnitID][i].transform.position = gridManager.GetTilePosition(TilePos);
        }
        

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
        if(alwaysPassPriority && currentPlayerString == GlobalNetworkState.LocalPlayerIndex && ruleManager.GetPlayerTurn() != GlobalNetworkState.LocalPlayerIndex)
        {   
                canvasUIScript.changeReactionText(false, " nagonting sket sig");

                RuleManager.PassAction passAction = new RuleManager.PassAction();
                passAction.PlayerIndex = currentPlayerString;

                //ExecutedActions.Enqueue(passAction);
                EnqueueAction(passAction);
        }

        if(GlobalNetworkState.LocalPlayerIndex != currentPlayerString && ruleManager.GetPlayerTurn() == GlobalNetworkState.LocalPlayerIndex)
        {
            //    canvasUIScript.changeReactionText(true, "Waiting...");
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            StartCoroutine(canvasUIScript.changeReactionText(true, "Waiting..."));

        }
        if(GlobalNetworkState.LocalPlayerIndex != currentPlayerString && ruleManager.GetPlayerTurn() != GlobalNetworkState.LocalPlayerIndex)
        {
          //  canvasUIScript.changeReactionText(false, "Waiting...");
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            StartCoroutine(canvasUIScript.changeReactionText(false, "Waiting..."));
        }
        if (GlobalNetworkState.LocalPlayerIndex == currentPlayerString && ruleManager.GetPlayerTurn() == GlobalNetworkState.LocalPlayerIndex)
        {

            //  canvasUIScript.changeReactionText(false, "Waiting...");
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            StartCoroutine(canvasUIScript.changeReactionText(false, "Waiting..."));

        }
        if (GlobalNetworkState.LocalPlayerIndex == currentPlayerString && ruleManager.GetPlayerTurn() != GlobalNetworkState.LocalPlayerIndex)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            StartCoroutine(canvasUIScript.changeReactionText(true, "Reaction?"));
         //   canvasUIScript.changeReactionText(true, "Reaction?");
            
    
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
     //   if (localStack.Count == 2) ;

        foreach (RuleManager.StackEntity entity in localStack)
        {
            print("Hur manga saker i stacken " + localStack.Count);
            GameObject createdImage = Instantiate(imageStackAbility,new Vector3() , new Quaternion());
          

            createdImage.GetComponent<ImageAbilityStackScript>().abilityEffectString = entity.EffectToResolve.GetText();
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
          //  createdImage.GetComponent<AbilityButton>().abilityDescriptionText = 
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


        canvasUIScript.ChangePlayerScore(PlayerIndex, NewScore);
    }

    public void OnUnitAttack(int AttackerID, int DefenderID)
    {
        //UnitSceneUIInfo UnitInfo = m_UnitTypeUIInfo[listOfImages[AttackerID].Resource.Name];
        //GameObject SceneObject  = listOfImages[AttackerID].objectInScene;
        //ResourceManager.UnitResource Resource = listOfImages[AttackerID].Resource;
        //if(Resource.UIInfo.AttackAnimation == null)
        //{
        //    return;
        //}
        //UnityEngine.Video.VideoPlayer Renderer = SceneObject.GetComponent<UnityEngine.Video.VideoPlayer>();
        //if(Renderer == null)
        //{
        //    Renderer = SceneObject.AddComponent<UnityEngine.Video.VideoPlayer>();
        //}
        //
        //Renderer.url = ((ResourceManager.Visual_Video)Resource.UIInfo.AttackAnimation.VisualInfo).VideoURL;
        //Renderer.Play();
        m_ActiveAnimations.Enqueue(new AttackAnimation(this, AttackerID, DefenderID));
    }

    public void OnUnitDestroyed(int UnitID)
    {
        listOfImages[UnitID].objectInScene.SetActive(false);
        foreach(GameObject Tile in listOfActivationIndicators[UnitID])
        {
            Tile.SetActive(false);
        }
        listOfActivationIndicators.Remove(UnitID);
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

    public void OnPlayerWin(int WinningPlayerIndex)
    {
        GameObject EndScreen = Instantiate(EndScreenUI);
        if(WinningPlayerIndex == GlobalNetworkState.LocalPlayerIndex)
        {
            EndScreen.transform.Find("WinSprite").GetComponent<UnityEngine.UI.Image>().sprite = WinSprite;
        }
        else
        {
            EndScreen.transform.Find("WinSprite").GetComponent<UnityEngine.UI.Image>().sprite = DefeatSprite;
        }
        gameObject.SetActive(false);
    }

    public void OnClick(ClickType clickType, RuleManager.Coordinate cord)
    {
        if(clickType == ClickType.rightClick)
        {
            return;
        }
        if(ruleManager.GetTileInfo(cord.X,cord.Y).HasObjective)
        {
            objectiveContestionIndicator.SetActive(true);
            objectiveContestionIndicator.transform.position = gridManager.GetTilePosition(cord);
            objectiveContestionIndicator.transform.position += new Vector3(0, 20, 0);
            objectiveContestionIndicator.GetComponent<ObjectiveContestionIndicator>().setPoints(0, ruleManager.GetObjectiveContestion(cord)[0]);
            objectiveContestionIndicator.GetComponent<ObjectiveContestionIndicator>().setPoints(1, ruleManager.GetObjectiveContestion(cord)[1]);
           
        }
        else
        {
            objectiveContestionIndicator.SetActive(false);
        }
        if(clickHandler != null)
        {
            clickHandler.OnClick(clickType, cord);
        }
        else if(clickHandler == null)
        {
            foreach(ClickHandler obj in clickHandlers)
            {
                if(obj.OnHandleClick(clickType,cord))
                {
                    clickHandler = obj;
                    clickHandler.OnClick(clickType, cord);
                    break; 
                }
                
            }
        }

        
    }

//    private void DestroyButtons()
//    {
//        foreach(GameObject obj in buttonDestroyList)
//        {   
//            obj.transform.SetParent(null);
//            Destroy(obj);
//            
//        }
//        buttonDestroyList.Clear();
//    }

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

            canvasUIScript.errorMessage(errorMessageText);

        }
        else
        {

            //ExecutedActions.Enqueue(passAction);
            EnqueueAction(passAction);
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

 //   public void resetSelection()
 //   {
 //       selectedUnit = null;
 //       unitCard.SetActive(false);
 //       unitActions.SetActive(false);
 //       canvasUIScript.DestroyButtons();
 //
 //       DestroyMovementRange();
 //
 //       requiredAbilityTargets.Clear();
 //
 //       MoveActionSelected = false;
 //       AttackActionSelected = false;
 //       abilitySelectionStarted = false;
 //       currentTargetToSelect = 0;
 //       selectedTargetsForAbilityExecution = new List<RuleManager.Target>();
 //   } 

    int p_GetSortingOrder(Coordinate position)
    {
        Vector3 LowestPos = gridManager.GetTilePosition(new Coordinate(gridManager.Width-1, gridManager.Height-1));
        return ((int)((-gridManager.GetTilePosition(position).y+Math.Abs(LowestPos.y))));
    }
    void p_CreateArmy(Dictionary<string,int> UnitOpaqueIDMap,int CurrentUnitOpaqueID,out int UnitOpaqueID,int PlayerIndex,int FactionIndex)
    {
        UnitOpaqueID = 1337;
        List<UnitInArmy> ArmyToInstantiate = firstPlayerArmy;
        if(FactionIndex == 0)
        {
            ArmyToInstantiate = firstPlayerArmy;
        }
        else if(FactionIndex == 1)
        {
            ArmyToInstantiate = secondPlayerArmy;
        }
        foreach (UnitInArmy unitFromList in ArmyToInstantiate)
        {
        
            ResourceManager.UnitResource unitToCreate = unitFromList.unit.CreateUnitInfo();
        
            if (!UnitOpaqueIDMap.ContainsKey(unitFromList.unit.GetType().Name))
            {
                UnitOpaqueIDMap.Add(unitFromList.unit.GetType().Name, CurrentUnitOpaqueID);
                m_OpaqueToUIInfo[CurrentUnitOpaqueID] = unitFromList.unit;
                CurrentUnitOpaqueID += 1;
            }
            unitToCreate.GameInfo.OpaqueInteger = UnitOpaqueIDMap[unitFromList.unit.GetType().Name];
        
        
            unitToCreate.GameInfo.TopLeftCorner = unitFromList.cord;
            if(PlayerIndex == 1)
            {
                unitToCreate.GameInfo.TopLeftCorner.X = (gridManager.Width-1)- unitToCreate.GameInfo.TopLeftCorner.X;
            }
            //unitToCreate.Position.Add(new Coordinate(unitToCreate.TopLeftCorner));


            int unitInt = ruleManager.RegisterUnit(unitToCreate.GameInfo, PlayerIndex);
            //UnitSprites unitSprites = unitFromList.unit.GetUnitSidewaySprite();

            UnitSceneInfo SceneUnit = new UnitSceneInfo();
            SceneUnit.Resource = unitToCreate;

            GameObject unitToCreateVisualObject = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(unitToCreate.GameInfo.TopLeftCorner), new Quaternion());

            SceneUnit.objectInScene = unitToCreateVisualObject;

            List<GameObject> ActivationIndicators = new List<GameObject>();
            foreach(Coordinate Offset in unitToCreate.GameInfo.UnitTileOffsets)
            {
                GameObject activationIndicator = Instantiate(activationIndicatorPrefab, gridManager.GetTilePosition(unitToCreate.GameInfo.TopLeftCorner+Offset), new Quaternion());
                activationIndicator.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
                activationIndicator.GetComponent<SpriteRenderer>().color = new Color(42, 254, 0);
                activationIndicator.GetComponent<SpriteRenderer>().color = Color.green;
                activationIndicator.transform.eulerAngles = gridManager.GetEulerAngle();
                ActivationIndicators.Add(activationIndicator);
            }        
            listOfActivationIndicators.Add(unitInt, ActivationIndicators);
        
        
            listOfImages.Add(unitInt, SceneUnit);

            if(!m_UnitTypeUIInfo.ContainsKey(unitToCreate.Name))
            {
                UnitSceneUIInfo NewInfo = new UnitSceneUIInfo();
                Texture2D Texture = ((ResourceManager.Visual_Image)unitToCreate.UIInfo.DownAnimation.VisualInfo).Sprite;
                NewInfo.DownSprite = Sprite.Create(Texture,
                    new Rect(0, 0, Texture.width, Texture.height), new Vector2(0.5f, 0));
                Texture = ((ResourceManager.Visual_Image)unitToCreate.UIInfo.UpAnimation.VisualInfo).Sprite;
                NewInfo.UpSprite = Sprite.Create(Texture,
                    new Rect(0, 0, Texture.width, Texture.height), new Vector2(0.5f, 0));
                m_UnitTypeUIInfo[unitToCreate.Name] = NewInfo;
            }
            SpriteRenderer spriteRenderer = unitToCreateVisualObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = m_UnitTypeUIInfo[unitToCreate.Name].UpSprite;
            //unitToCreateVisualObject.GetComponent<SpriteRenderer>().sprite = unitSprites.sidewaySprite;
            if (PlayerIndex == 1)
            {
                unitToCreateVisualObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            unitToCreateVisualObject.GetComponent<SpriteRenderer>().sortingOrder = p_GetSortingOrder(new Coordinate(unitToCreate.GameInfo.TopLeftCorner.X,unitToCreate.GameInfo.TopLeftCorner .Y));
        }
        UnitOpaqueID = CurrentUnitOpaqueID;
    }

    private void CreateArmies()
    {

        Dictionary<string, int> UnitOpaqueIDMap = new Dictionary<string, int>();
        int CurrentUnitOpaqueID = 0;
        p_CreateArmy(UnitOpaqueIDMap, CurrentUnitOpaqueID,out CurrentUnitOpaqueID,0,GlobalNetworkState.PlayerFactionIndex[0]);
        p_CreateArmy(UnitOpaqueIDMap, CurrentUnitOpaqueID, out CurrentUnitOpaqueID, 1,GlobalNetworkState.PlayerFactionIndex[1]);


        foreach(RuleManager.Coordinate Coord in new Coordinate[] {
            new Coordinate(20,10),
            new Coordinate(20,9),
            new Coordinate(20,8),
            new Coordinate(21,10),
            new Coordinate(21,9),
            new Coordinate(21,8),
        })
        {
            ruleManager.GetTileInfo(Coord.X, Coord.Y).Flags |= RuleManager.TileFlags.Impassable;
        }

        foreach(RuleManager.Coordinate cord in listOfObjectives)
        {
            ruleManager.GetTileInfo(cord.X,cord.Y).HasObjective = true;

            GameObject objectiveImage = Instantiate(objectivePrefab, gridManager.GetTilePosition(cord), new Quaternion());

            objectiveImage.GetComponent<Objective>().setNeutralControl();

            objectiveImage.GetComponent<SpriteRenderer>().sortingOrder =p_GetSortingOrder(cord);

            dictionaryOfObjectiveCords.Add(cord,objectiveImage.GetComponent<Objective>());

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



    public void enableAlwaysPass()
    {
        if(alwaysPassPriority)
        {
            alwaysPassPriority = false; 
        }
        else
        {
            alwaysPassPriority = true; 

            if(GlobalNetworkState.LocalPlayerIndex == ruleManager.getPlayerPriority() && ruleManager.GetPlayerTurn() == GlobalNetworkState.LocalPlayerIndex)
            {
                RuleManager.PassAction passAction = new RuleManager.PassAction();
                passAction.PlayerIndex = ruleManager.getPlayerPriority();

                //ExecutedActions.Enqueue(passAction);
                EnqueueAction(passAction);
            }
        }
    }
    public bool EnqueueAction(RuleManager.Action action)
    {
        string errorMessage; 

        if(ruleManager.ActionIsValid(action, out errorMessage))
        {
            ExecutedActions.Enqueue(action);
            return true;
        }
        else
        {
            canvasUIScript.errorMessage(errorMessage);
        }


        return false; 
    }
    public Unit GetUnitUIInfo(RuleManager.UnitInfo UnitToInspect)
    {
        return (m_OpaqueToUIInfo[UnitToInspect.OpaqueInteger]);
    }
    public void DeactivateClickHandler()
    {
        if(clickHandler != null)
        {
            clickHandler.Deactivate();

        }
        clickHandler = null; 
    }


    public void updateUnit(UnitInfo unitInfo)
    {

    }
}

class UnitSceneUIInfo
{
    public Sprite UpSprite;
    public Sprite DownSprite;
}

class UnitSceneInfo
{
    public ResourceManager.UnitResource Resource = null;
    public GameObject objectInScene;
}

//public struct UnitSprites
//{
//    public Sprite forwardSprite;
//
//    public Sprite backwardSprite;
//
//    public Sprite sidewaySprite;
//
//    public Sprite activationIndicator;
//
//    public GameObject objectInScene;
//}
//
//
//public class UIInfo
//{
//    public Sprite WhichImage; 
//}

