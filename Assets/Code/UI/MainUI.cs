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



public interface RestorableUIElement
{
    void RestoreFromGamestate(RuleManager.RuleManager RestoredGamestate);
}
public class MainUI : MonoBehaviour, RuleManager.RuleEventHandler , ClickReciever, ActionRetriever,AnimationPlayer
{

    public static MainUI instance = null;

    public static MainUI GetStaticInstance()
    {
        return instance;
    }
    public RuleManager.RuleManager GetRuleState()
    {
        return ruleManager;
    }
    public ResourceManager.UnitResource UnitIDToResource(int UnitID)
    {
        ResourceManager.UnitResource ReturnValue = null;
        var UnitInfo = ruleManager.GetUnitInfo(UnitID);
        if(UnitInfo != null)
        {
            if(g_ResourceManager.HasResourceWithID(UnitInfo.OpaqueInteger))
            {
                return g_ResourceManager.GetUnitResource(UnitInfo.OpaqueInteger);
            }
        }
        return ReturnValue;
    }
    public RuleManager.UnitInfo GetUnitInfo(int UnitID)
    {
        var UnitInfo = ruleManager.GetUnitInfo(UnitID);
        return UnitInfo;
    }

    float m_DefaultCameraSpeed = 0.4f;
    private void MoveCamera(Vector3 NewPosition,float Speed)
    {
        m_ActiveAnimations.Enqueue(new MoveCameraAnimation(m_ActiveCamera.gameObject, NewPosition, m_ActiveCamera.transform.position, Speed));
    }
    public void MoveCamera(Vector3 NewPosition)
    {
        MoveCamera(NewPosition, m_DefaultCameraSpeed);
    }

    public Vector3 TileToWorldSpace(RuleManager.Coordinate TilePosition)
    {
        return gridManager.GetTilePosition(TilePosition);
    }

    public void PlayAnimation(int Unit,object AnimationToAnime)
    {
        if(AnimationToAnime is ResourceManager.Animation && ((ResourceManager.Animation)AnimationToAnime).VisualInfo is ResourceManager.Visual_Animation)
        {
            ResourceManager.Visual_Animation VisualAnimation = (ResourceManager.Visual_Animation) ((ResourceManager.Animation)AnimationToAnime).VisualInfo;
            m_ActiveAnimations.Enqueue(new UnitAnimation(Unit,this,VisualAnimation));
        }
    }
    public void PlayAnimation(Coordinate TileCoordinate,object AnimationToAnime)
    {
        if (AnimationToAnime is ResourceManager.Animation && ((ResourceManager.Animation)AnimationToAnime).VisualInfo is ResourceManager.Visual_Animation)
        {
            ResourceManager.Visual_Animation VisualAnimation = (ResourceManager.Visual_Animation)((ResourceManager.Animation)AnimationToAnime).VisualInfo;
            m_ActiveAnimations.Enqueue(new LocationAnimation(VisualAnimation, gridManager.GetTilePosition(TileCoordinate)));
        }
    }

    void p_SetUnitVisual(int UnitID, ResourceManager.Visual VisualToSet)
    {
        GameObject ObjectToModify = listOfImages[UnitID].objectInScene;
        SpriteRenderer spriteRenderer = ObjectToModify.GetComponent<SpriteRenderer>();
        if (VisualToSet is ResourceManager.Visual_Image)
        {
            spriteRenderer.sprite = ((ResourceManager.Visual_Image)VisualToSet).Sprite;
        }
        spriteRenderer.transform.localScale = new Vector3(1, 1, 1) * (VisualToSet.Width) / (spriteRenderer.sprite.texture.width / spriteRenderer.sprite.pixelsPerUnit);
        Vector3 NewPosition = new Vector3(0, 0, 0);
        UnitInfo Info = ruleManager.GetUnitInfo(UnitID);
        foreach(Coordinate Offset in Info.UnitTileOffsets)
        {
            NewPosition += gridManager.GetTilePosition(Offset + Info.TopLeftCorner);
        }
        NewPosition /= Info.UnitTileOffsets.Count;
        //take offset into account
        Vector2 OffsetDiff = new Vector2(0.5f - VisualToSet.XCenter, 0.5f - VisualToSet.YCenter);
        OffsetDiff = OffsetDiff * spriteRenderer.transform.localScale;
        OffsetDiff.x *= (spriteRenderer.sprite.texture.width / spriteRenderer.sprite.pixelsPerUnit);
        OffsetDiff.y *= (spriteRenderer.sprite.texture.height / spriteRenderer.sprite.pixelsPerUnit);
        OffsetDiff.x *= Info.Direction.X;
        spriteRenderer.transform.localPosition = NewPosition+(Vector3) OffsetDiff;
        listOfImages[UnitID].AnimationOffset = OffsetDiff;
    }

    public static ResourceManager.ResourceManager g_ResourceManager = null;
    
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
            if(IsFinished())
            {
                return;
            }
            float DistanceToIncrease = Math.Min((m_TargetLocation -(Vector2) m_CameraToModify.transform.position).magnitude,m_Speed*DeltaTime);
            m_CameraToModify.transform.position = m_CameraToModify.transform.position + (Vector3) m_Direction * DistanceToIncrease;
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

    class SpriteAnimation : UIAnimation
    {
        double m_ElapsedTime = 0;
        int m_CurrentIndex = -1;
        int FPS = 0;
        SpriteRenderer m_AssociatedSpriteRenderer;
        ResourceManager.Visual_Animation m_Animation = null;


        public SpriteAnimation(SpriteRenderer Renderer,ResourceManager.Visual_Animation Animation)
        {
            m_AssociatedSpriteRenderer = Renderer;
            m_Animation = Animation;
        }
        public void Initialize()
        {
            FPS = m_Animation.FPS;
            m_AssociatedSpriteRenderer.sprite = m_Animation.AnimationContent[0];
        }
        public bool IsFinished()
        {
            return (m_AssociatedSpriteRenderer == null || (m_ElapsedTime >= (m_Animation.AnimationContent.Count / (float)FPS)));
        }
        public void Increment(float DeltaTime)
        {
            m_ElapsedTime += DeltaTime;
            if (m_AssociatedSpriteRenderer != null)
            {
                if (!IsFinished())
                {
                    int NewIndex = (int)(m_ElapsedTime * FPS);
                    if (NewIndex != m_CurrentIndex)
                    {
                        m_AssociatedSpriteRenderer.sprite = m_Animation.AnimationContent[(int)(m_ElapsedTime * FPS)];
                    }
                    m_CurrentIndex = NewIndex;
                }
            }
        }
        public void Finish()
        {

        }
    }

    class LocationAnimation : UIAnimation
    {
        SpriteAnimation m_SubAnimation;
        GameObject m_AssociatedObject;
        public LocationAnimation(ResourceManager.Visual_Animation Animation,Vector3 Location)
        {
            m_AssociatedObject = new GameObject();
            m_AssociatedObject.transform.position = Location;
            m_AssociatedObject.AddComponent<SpriteRenderer>();

            SpriteRenderer spriteRenderer = m_AssociatedObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Animation.AnimationContent[0];

            spriteRenderer.transform.localScale = new Vector3(1, 1, 1) * (Animation.Width) / (spriteRenderer.sprite.texture.width / spriteRenderer.sprite.pixelsPerUnit);
            Vector2 OffsetDiff = new Vector2(0.5f - Animation.XCenter, 0.5f - Animation.YCenter);
            OffsetDiff = OffsetDiff * spriteRenderer.transform.localScale;
            OffsetDiff.x *= (spriteRenderer.sprite.texture.width / spriteRenderer.sprite.pixelsPerUnit);
            OffsetDiff.y *= (spriteRenderer.sprite.texture.height / spriteRenderer.sprite.pixelsPerUnit);
            spriteRenderer.transform.localPosition = Location + (Vector3)OffsetDiff;

            m_SubAnimation = new SpriteAnimation(m_AssociatedObject.GetComponent<SpriteRenderer>(), Animation);
        }
        public void Initialize()
        {
            m_SubAnimation.Initialize();
        }
        public bool IsFinished()
        {
            return (m_SubAnimation.IsFinished());
        }
        public void Increment(float DeltaTime)
        {
            m_SubAnimation.Increment(DeltaTime);
        }
        public void Finish()
        {
            m_SubAnimation.Finish();
        }
    }

    class UnitAnimation : UIAnimation
    {
        MainUI m_AssociatedUI = null;
        SpriteAnimation m_SubAnimation = null;
        SpriteRenderer m_AssociatedSpriteRenderer;
        ResourceManager.Visual_Animation m_Animation = null;
        int m_UnitID;

        public UnitAnimation(int UnitID,MainUI UI,ResourceManager.Visual_Animation Animation)
        {
            m_UnitID = UnitID;
            m_AssociatedUI = UI;
            m_Animation = Animation;
        }
        public void Initialize()
        {
            GameObject SceneObject = m_AssociatedUI.listOfImages[m_UnitID].objectInScene;
            m_AssociatedSpriteRenderer = SceneObject.GetComponent<SpriteRenderer>();
            m_SubAnimation = new SpriteAnimation(m_AssociatedSpriteRenderer, m_Animation);
            m_AssociatedSpriteRenderer.sprite = m_Animation.AnimationContent[0];
            m_AssociatedUI.p_SetUnitVisual(m_UnitID, m_Animation);
            m_SubAnimation.Initialize();
        }
        public bool IsFinished()
        {
            return (m_SubAnimation.IsFinished());
        }
        public void Increment(float DeltaTime)
        {
            m_SubAnimation.Increment(DeltaTime);
        }
        public void Finish()
        {
            m_SubAnimation.Finish();    
            UnitSceneInfo UnitInfo = m_AssociatedUI.listOfImages[m_UnitID];
            GameObject SceneObject = m_AssociatedUI.listOfImages[m_UnitID].objectInScene;
            ResourceManager.UnitResource Resource = m_AssociatedUI.listOfImages[m_UnitID].Resource;
            if (Resource.UIInfo.AttackAnimation == null)
            {
                return;
            }
            RuleManager.Coordinate Direction = m_AssociatedUI.ruleManager.GetUnitInfo(m_UnitID).Direction;
            if (Direction.X == 1 || Direction.Y == -1)
            {
                //SceneObject.GetComponent<SpriteRenderer>().sprite = UnitInfo.DownSprite;
                m_AssociatedUI.p_SetUnitVisual(m_UnitID, UnitInfo.Resource.UIInfo.DownAnimation.VisualInfo);
            }
            if (Direction.X == 1 || Direction.Y == -1)
            {
                m_AssociatedUI.p_SetUnitVisual(m_UnitID, UnitInfo.Resource.UIInfo.UpAnimation.VisualInfo);
            }
        }
    }
    class AttackAnimation : UIAnimation
    {
        MainUI m_AssociatedUI = null;

        double m_ElapsedTime = 0;
        int m_AttackingUnit = 0;
        int m_CurrentIndex = -1;
        int FPS = 0;
        SpriteRenderer m_AssociatedSpriteRenderer;
        List<UnityEngine.Sprite> m_Frames = null;
        
        //UnityEngine.Video.VideoPlayer m_AssociatedRender = null;
        public AttackAnimation(MainUI AsssociatedUI, int AttackingUnit, int DefendingUnit)
        {
            m_AssociatedUI = AsssociatedUI;
            m_AttackingUnit = AttackingUnit;
        }
        public void Initialize()
        {
            UnitSceneInfo UnitInfo = m_AssociatedUI.listOfImages[m_AttackingUnit];
            GameObject SceneObject = m_AssociatedUI.listOfImages[m_AttackingUnit].objectInScene;
            ResourceManager.UnitResource Resource = m_AssociatedUI.listOfImages[m_AttackingUnit].Resource;
            if (Resource.UIInfo.AttackAnimation == null)
            {
                return;
            }
            m_AssociatedSpriteRenderer = SceneObject.GetComponent<SpriteRenderer>();
            ResourceManager.Visual_Animation Animation = (ResourceManager.Visual_Animation)Resource.UIInfo.AttackAnimation.VisualInfo;
            FPS = Animation.FPS;
            m_Frames = Animation.AnimationContent;
            float OldPixelRatio = m_AssociatedSpriteRenderer.sprite.texture.width / m_AssociatedSpriteRenderer.size.x;
            m_AssociatedSpriteRenderer.sprite = m_Frames[0];
            m_AssociatedUI.p_SetUnitVisual(m_AttackingUnit, Animation);
        }
        public void Finish()
        {

            UnitSceneInfo UnitInfo = m_AssociatedUI.listOfImages[m_AttackingUnit];
            GameObject SceneObject = m_AssociatedUI.listOfImages[m_AttackingUnit].objectInScene;
            ResourceManager.UnitResource Resource = m_AssociatedUI.listOfImages[m_AttackingUnit].Resource;
            if (Resource.UIInfo.AttackAnimation == null)
            {
                return;
            }
            RuleManager.Coordinate Direction = m_AssociatedUI.ruleManager.GetUnitInfo(m_AttackingUnit).Direction;
            if (Direction.X == 1 || Direction.Y == -1)
            {
                //SceneObject.GetComponent<SpriteRenderer>().sprite = UnitInfo.DownSprite;
                m_AssociatedUI.p_SetUnitVisual(m_AttackingUnit, UnitInfo.Resource.UIInfo.DownAnimation.VisualInfo);
            }
            if (Direction.X == 1 || Direction.Y == -1)
            {
                m_AssociatedUI.p_SetUnitVisual(m_AttackingUnit, UnitInfo.Resource.UIInfo.UpAnimation.VisualInfo);
            }
        }
        public bool IsFinished()
        {
            return ( m_AssociatedSpriteRenderer == null || (m_ElapsedTime >= (m_Frames.Count / (float)FPS)));
        }
        public void Increment(float DeltaTime)
        {
            m_ElapsedTime += DeltaTime;
            if(m_AssociatedSpriteRenderer != null)
            {
                if(!IsFinished())
                {
                    int NewIndex = (int)(m_ElapsedTime * FPS);
                    if(NewIndex != m_CurrentIndex)
                    { 
                        m_AssociatedSpriteRenderer.sprite = m_Frames[(int) (m_ElapsedTime * FPS)];
                    }
                    m_CurrentIndex = NewIndex;
                }
            }
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

    //private Dictionary<string, UnitSceneUIInfo> m_UnitTypeUIInfo = new Dictionary<string, UnitSceneUIInfo>();
    private Queue<UIAnimation> m_ActiveAnimations = new Queue<UIAnimation>();



    private Dictionary<int, UnitSceneInfo> listOfImages = new Dictionary<int, UnitSceneInfo>();

    private Dictionary<int, List<GameObject>> listOfActivationIndicators = new Dictionary<int, List<GameObject>>();



    //public Dictionary<int, ResourceManager.UnitResource> m_OpaqueToUIInfo = new Dictionary<int, ResourceManager.UnitResource>();



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

    public struct RegisteredUnit
    {
        public ResourceManager.UnitResource Unit;
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

        ruleManager.SetEventHandler(this);
        ruleManager.SetScriptHandler(g_ResourceManager.GetScriptHandler());
        ruleManager.SetAnimationPlayer(this);
        gridManager.SetInputReciever(this);

        foreach (GameObject obj in clickHandlerPrefabs)
        {
           GameObject newClickHandler =  Instantiate(obj, new Vector3(), new Quaternion());
           clickHandlers.Add(newClickHandler.GetComponent<ClickHandler>());
           newClickHandler.GetComponent<ClickHandler>().Setup(this);
        }
        InitializeGamestate();
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
        else if (m_ActiveAnimations.Count  > 0)
        {
            m_ActiveAnimations.Peek().Increment(Time.deltaTime);
        }

        m_ActiveCamera.SetActive(m_ActiveAnimations.Count == 0);

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
        UnitSceneInfo UIInfo = listOfImages[UnitID];
        if(NewRotation.Y == 1 || NewRotation.X == -1)
        {
            p_SetUnitVisual(UnitID, UIInfo.Resource.UIInfo.UpAnimation.VisualInfo);
        }
        else
        {
            p_SetUnitVisual(UnitID, UIInfo.Resource.UIInfo.DownAnimation.VisualInfo);
        }
        UIInfo.objectInScene.GetComponent<SpriteRenderer>().flipX = (NewRotation.X == -1 || NewRotation.Y == -1);
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
            //if(xChange > 0)
            //{
            //    spriteRenderer.flipX = false;
            //    
            //}
            //else
            //{
            //    spriteRenderer.flipX = true;
            //}
        }

        if (Mathf.Abs(yChange) > 3 && Mathf.Abs(xChange) < 3)
        {
            if (yChange > 0)
            {
                //spriteRenderer.sprite = m_UnitTypeUIInfo[unitSprites.Name].UpSprite;
            }
            else
            {
                //spriteRenderer.sprite = m_UnitTypeUIInfo[unitSprites.Name].DownSprite;
                //spriteRenderer.sprite = unitSprites.forwardSprite;
            }
        }
        bool xChangeIsBigEnough = Mathf.Abs(xChange) > 3;
        bool yChangeIsBigEnough = Mathf.Abs(yChange) > 3;
        bool xChangeIsBigger = Mathf.Abs(xChange) > Mathf.Abs(yChange) ;

        if(xChangeIsBigger && xChangeIsBigEnough)
        {
            //if (xChange > 0)
            //{
            //    spriteRenderer.flipX = false;
            //}
            //else
            //{
            //    spriteRenderer.flipX = true;
            //}
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
        Vector3 NewWorldPosition = new Vector3(0, 0, 0);
        foreach (Coordinate Offset in MovedUnit.UnitTileOffsets)
        {
            NewWorldPosition += gridManager.GetTilePosition(Offset + MovedUnit.TopLeftCorner);
        }
        NewWorldPosition /= MovedUnit.UnitTileOffsets.Count;
        visualObject.transform.position = NewWorldPosition;
        visualObject.transform.position += listOfImages[UnitID].AnimationOffset;
        for(int i = 0; i < MovedUnit.UnitTileOffsets.Count;i++)
        {
            Coordinate TilePos = NewPosition + MovedUnit.UnitTileOffsets[i];
            listOfActivationIndicators[UnitID][i].transform.position = gridManager.GetTilePosition(TilePos);
        }
        

    }

    public void OnStackPop(RuleManager.StackEntity PoppedEntity)
    {
        //localStack.Pop();
        //
        //DestroyStackUI();
        //
        //if(localStack.Count != 0)
        //{
        //    CreateStackUI();
        //}
        foreach(var Handler in m_StackEventsHandlers)
        {
            Handler.OnStackPop(PoppedEntity);
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
        }

        
    }
    public void OnStackPush(RuleManager.StackEntity PushedEntity)
    {
        //localStack.Push(PushedEntity);
        //
        //DestroyStackUI();
        //
        //CreateStackUI();

        foreach (var Handler in m_StackEventsHandlers)
        {
            Handler.OnStackPush(PushedEntity);
        }
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
          

            createdImage.GetComponent<ImageAbilityStackScript>().abilityEffectString = entity.EffectToResolve.GetText();
            stackObjectsToDestroy.Add(createdImage);
            createdImage.transform.SetParent(GameObject.Find("StackAbilityHolder").transform);//FindObjectOfType<Canvas>().gameObject.transform;

            if(entity.Source is RuleManager.EffectSource_Unit)
            {
                RuleManager.EffectSource_Unit Source = (RuleManager.EffectSource_Unit)entity.Source;
                if(Source.EffectIndex != -1)
                {
                    var UnitResource = g_ResourceManager.GetUnitResource(ruleManager.GetUnitInfo(Source.UnitID).OpaqueInteger);
                    if (UnitResource.UIInfo.AbilityIcons.ContainsKey(Source.EffectIndex)) 
                    {
                        var Visual = UnitResource.UIInfo.AbilityIcons[Source.EffectIndex];
                        if(Visual.VisualInfo is ResourceManager.Visual_Image)
                        {
                            createdImage.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = ((ResourceManager.Visual_Image)Visual.VisualInfo).Sprite.texture;
                        }
                    } 
                    
                }
                print("Stack entity effect: " + entity.EffectToResolve.GetText());
            }
            increasingPadding += originalPadding;
        }
        GameObject.Find("StackAbilityHolder").GetComponent<UnitActions>().sortChildren();
    }
    void Awake()
    {
        g_ResourceManager = new ResourceManager.ResourceManager(Application.streamingAssetsPath);
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    void OnDestroy()
    {
        instance = null;
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
        m_ActiveAnimations.Enqueue(new MoveCameraAnimation(m_ActiveCamera.gameObject,gridManager.GetTilePosition(ruleManager.GetUnitInfo(AttackerID).TopLeftCorner) 
            ,m_ActiveCamera.gameObject.transform.position,0.4f));
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
        canvasUIScript.changeTopFrame(CurrentPlayerTurnIndex);
        currentTurnText.text = "Turn: " + CurrentTurnCount;
    }

    public void OnUnitCreate(RuleManager.UnitInfo NewUnit)
    {
        int unitInt = NewUnit.UnitID;
        UnitSceneInfo SceneUnit = new UnitSceneInfo();
        SceneUnit.Resource = GetUnitUIInfo(NewUnit);

        GameObject unitToCreateVisualObject = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(NewUnit.TopLeftCorner), new Quaternion());

        SceneUnit.objectInScene = unitToCreateVisualObject;

        List<GameObject> ActivationIndicators = new List<GameObject>();
        foreach (Coordinate Offset in NewUnit.UnitTileOffsets)
        {
            GameObject activationIndicator = Instantiate(activationIndicatorPrefab, gridManager.GetTilePosition(NewUnit.TopLeftCorner + Offset), new Quaternion());
            activationIndicator.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
            activationIndicator.GetComponent<SpriteRenderer>().color = new Color(42, 254, 0);
            activationIndicator.GetComponent<SpriteRenderer>().color = Color.green;
            activationIndicator.transform.eulerAngles = gridManager.GetEulerAngle();
            ActivationIndicators.Add(activationIndicator);
        }
        listOfActivationIndicators.Add(unitInt, ActivationIndicators);
        listOfImages.Add(unitInt, SceneUnit);
        SpriteRenderer spriteRenderer = unitToCreateVisualObject.GetComponent<SpriteRenderer>();
        p_SetUnitVisual(unitInt, SceneUnit.Resource.UIInfo.UpAnimation.VisualInfo);
        if (NewUnit.PlayerIndex == 1)
        {
            unitToCreateVisualObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        unitToCreateVisualObject.GetComponent<SpriteRenderer>().sortingOrder = p_GetSortingOrder(new Coordinate(NewUnit.TopLeftCorner.X, NewUnit.TopLeftCorner.Y));
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

    public void OnInitiativeChange(int newIntitiative, int whichPlayer)
    {
        if(whichPlayer == 0)
        {
            initiativePlayer0.text = "Player 1 Initiative " + newIntitiative + "/100";
        }
        if (whichPlayer == 1)
        {
            initiativePlayer1.text = "Player 2 Initiative " + newIntitiative + "/100";
        }
    }

    //Generic callbacks

    List<RuleManager.StackEventHandler> m_StackEventsHandlers = new();
    List<RestorableUIElement> m_RestorableUIElements = new();
    public void RegisterStackEventHandler(StackEventHandler NewHandler)
    {
        m_StackEventsHandlers.Add(NewHandler);
    }
    public void RegisterGameRestorableUIElement(RestorableUIElement NewHandler)
    {
        m_RestorableUIElements.Add(NewHandler);
    }


    public void SwitchPlayerPriority()
    {
        RuleManager.PassAction passAction = new RuleManager.PassAction();
        passAction.PlayerIndex = ruleManager.getPlayerPriority();
        string errorMessageText = "";
        if (!ruleManager.ActionIsValid(passAction, out errorMessageText))
        {
            canvasUIScript.errorMessage(errorMessageText);
        }
        else
        {
            EnqueueAction(passAction);
        }

    }
    public RuleManager.Action PopAction()
    {
        //DEBUG
        string SaveFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Warcards/Save.json";
        var OutFile = new System.IO.FileStream(SaveFile, System.IO.FileMode.Create);
        var ObjectToWrite = ruleManager.Serialize();
        OutFile.Write(System.Text.UTF8Encoding.UTF8.GetBytes( ObjectToWrite.ToString()));
        OutFile.Flush();
        OutFile.Close();
        var DeserializedFile = MBJson.JSONObject.DeserializeObject<RuleManager.RuleManager>(ObjectToWrite);
        return ExecutedActions.Dequeue();
    }
    public int getAvailableActions()
    {
        return ExecutedActions.Count;
    }

    int p_GetSortingOrder(Coordinate position)
    {
        Vector3 LowestPos = gridManager.GetTilePosition(new Coordinate(gridManager.Width-1, gridManager.Height-1));
        return ((int)((-gridManager.GetTilePosition(position).y+Math.Abs(LowestPos.y))));
    }
    void p_CreateArmy(int PlayerIndex,List<RegisteredUnit> Units,bool Mirror)
    {
        List<RegisteredUnit> ArmyToInstantiate = Units;
        foreach (RegisteredUnit unitFromList in ArmyToInstantiate)
        {
            ResourceManager.UnitResource unitToCreate = unitFromList.Unit;
            unitToCreate.GameInfo.TopLeftCorner = unitFromList.cord;
            if(PlayerIndex == 1 && Mirror)
            {
                unitToCreate.GameInfo.TopLeftCorner.X = (gridManager.Width-1)- unitToCreate.GameInfo.TopLeftCorner.X;
            }
            ruleManager.RegisterUnit(unitToCreate.GameInfo, PlayerIndex);
        }
    }

    static List<RegisteredUnit> p_Convert(List<UnitInArmy> Units)
    {
        List<RegisteredUnit> ReturnValue = new();
        foreach(var Unit in Units)
        {
            RegisteredUnit NewUnit = new();
            NewUnit.cord = Unit.cord;
            NewUnit.Unit = Unit.unit.CreateUnitInfo();
            ReturnValue.Add(NewUnit);
        }
        return ReturnValue;
    }

    void p_CreateObjective(Coordinate ObjectiveCoordinate)
    {
        ruleManager.GetTileInfo(ObjectiveCoordinate.X, ObjectiveCoordinate.Y).HasObjective = true;
        GameObject objectiveImage = Instantiate(objectivePrefab, gridManager.GetTilePosition(ObjectiveCoordinate), new Quaternion());
        objectiveImage.GetComponent<Objective>().setNeutralControl();
        objectiveImage.GetComponent<SpriteRenderer>().sortingOrder = p_GetSortingOrder(ObjectiveCoordinate);
        dictionaryOfObjectiveCords.Add(ObjectiveCoordinate, objectiveImage.GetComponent<Objective>());
    }
    void p_CreateImpassableTerrain(Coordinate ObjectiveCoordinate)
    {
        ruleManager.GetTileInfo(ObjectiveCoordinate.X, ObjectiveCoordinate.Y).Flags |= RuleManager.TileFlags.Impassable;
        GameObject activationIndicator = Instantiate(activationIndicatorPrefab, gridManager.GetTilePosition(ObjectiveCoordinate), new Quaternion());
        activationIndicator.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        activationIndicator.transform.eulerAngles = gridManager.GetEulerAngle();
    }

    private void InitilizeFromSave(MBJson.JSONObject SavedData)
    {
        ruleManager.RestoreState(g_ResourceManager, SavedData);
        int Height = ruleManager.GetHeight();
        int Width = ruleManager.GetHeight();

        HashSet<int> RegisteredUnits = new();
        for(int i = 0; i < Height;i++)
        {
            for(int j = 0; j < Width;j++)
            {
                Coordinate CurrentCoord = new Coordinate(j, i);
                var TileInfo = ruleManager.GetTileInfo(j, i);
                if(TileInfo.HasObjective)
                {
                    p_CreateObjective(CurrentCoord);
                }
                else if(TileInfo.Flags != 0)
                {
                    p_CreateImpassableTerrain(CurrentCoord);
                }
                else if(TileInfo.StandingUnitID != 0 && !RegisteredUnits.Contains(TileInfo.StandingUnitID))
                {
                    OnUnitCreate(ruleManager.GetUnitInfo(TileInfo.StandingUnitID));
                    RegisteredUnits.Add(TileInfo.StandingUnitID);
                }
            }
        }
        foreach(var Element in m_RestorableUIElements)
        {
            Element.RestoreFromGamestate(ruleManager);
        }

    }
    private void InitializeGamestate()
    {
        //if the file exists, use it instead
        List<List<UserTileInfo>> SavedTiles = new();
        string ArmyFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Warcards/Army.json";
        string SaveFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Warcards/Save.json";

        if(System.IO.File.Exists(SaveFile))
        {
            var File = new System.IO.StreamReader(SaveFile);
            var Content = System.Text.UTF8Encoding.UTF8.GetBytes(File.ReadToEnd());
            File.Close();
            var JsonObject = MBJson.JSONObject.ParseJSONObject(Content);
            InitilizeFromSave(JsonObject);
            return;
        }

        var Player1Army = p_Convert(firstPlayerArmy);
        var Player2Army = p_Convert(secondPlayerArmy);
        bool Mirror = true;

        List<Coordinate> ImpassableTerrain = new List<Coordinate> {
                new Coordinate(20,10),
                new Coordinate(20,9),
                new Coordinate(20,8),
                new Coordinate(21,10),
                new Coordinate(21,9),
                new Coordinate(21,8),

                new Coordinate(20,22),
                new Coordinate(20,21),
                new Coordinate(20,20),
                new Coordinate(21,22),
                new Coordinate(21,21),
                new Coordinate(21,20),
             };
        if (System.IO.File.Exists(ArmyFile))
        {
            SavedTiles =
             MBJson.JSONObject.DeserializeObject<List<List<UserTileInfo>>>(   
                 MBJson.JSONObject.ParseJSONObject( 
                     System.Text.Encoding.UTF8.GetBytes(
                         (new System.IO.StreamReader(System.IO.File.Open(ArmyFile, System.IO.FileMode.Open))).ReadToEnd())));
            List<Coordinate> NewObjectiveCoordinates = new();
            List<Coordinate> NewImpassableTerrain = new();
            List<RegisteredUnit> NewPlayer1Army = new();
            List<RegisteredUnit> NewPlayer2Army = new();
            int CurrentRow = 0;
            int CurrentCol = 0;
            foreach (var Row in SavedTiles)
            {
                foreach(var Tile in Row)
                {
                    var CurrentCoordinate = new Coordinate(CurrentRow, CurrentCol);
                    if(Tile.UnitName == "Impassable")
                    {
                        NewImpassableTerrain.Add(CurrentCoordinate);
                    }
                    else if (Tile.UnitName == "Objective")
                    {
                        NewObjectiveCoordinates.Add(CurrentCoordinate);
                    }
                    else if(Tile.UnitName != "")
                    {
                        RegisteredUnit NewUnit = new();
                        NewUnit.cord = CurrentCoordinate;
                        NewUnit.Unit = g_ResourceManager.GetUnitResource(Tile.UnitName);
                        if (Tile.PlayerIndex == 0)
                        {
                            NewPlayer1Army.Add(NewUnit);
                        }
                        else if (Tile.PlayerIndex == 1)
                        {
                            NewPlayer2Army.Add(NewUnit);
                        }
                    }
                    CurrentCol += 1;
                }
                CurrentCol = 0;
                CurrentRow += 1;
            }
            Player1Army = NewPlayer1Army;
            Player2Army = NewPlayer2Army;
            Mirror = false;
            listOfObjectives = NewObjectiveCoordinates;
            ImpassableTerrain = NewImpassableTerrain;
        }

        p_CreateArmy(0, Player1Army ,Mirror);
        p_CreateArmy(1, Player2Army,Mirror);
        foreach(RuleManager.Coordinate Coord in ImpassableTerrain)
        {
            p_CreateImpassableTerrain(Coord);
        }
        foreach (RuleManager.Coordinate cord in listOfObjectives)
        {
            p_CreateObjective(cord);
        }
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
    public ResourceManager.UnitResource GetUnitUIInfo(RuleManager.UnitInfo UnitToInspect)
    {
        return g_ResourceManager.GetUnitResource(UnitToInspect.OpaqueInteger);
    }
    public ResourceManager.UnitResource GetUnitUIInfo(int UnitID)
    {
        return g_ResourceManager.GetUnitResource(ruleManager.GetUnitInfo(UnitID).OpaqueInteger);
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


    int RegisterTileNotification(List<Coordinate> AffectedCoordinates,Func<string> Description,int UnitID)
    {
        return 0;
    }
    void RemoveTileNotification(int NotificationID)
    {
    }

    int RegisterImage(Coordinate AffectedCoordinate,ResourceManager.Visual VisualToShow)
    {
        return 0;
    }
    void RemoveImage(int ImageID)
    {

    }
}
[Serializable]
public class UserTileInfo
{
    [SerializeField]
    public string UnitName = "";
    [SerializeField]
    public int PlayerIndex = 0;
};

class UnitSceneUIInfo
{
    public Sprite UpSprite;
    public Sprite DownSprite;
}

class UnitSceneInfo
{
    public ResourceManager.UnitResource Resource = null;
    public GameObject objectInScene;
    public Vector3 AnimationOffset = new Vector3(0, 0, 0);
}