using RuleManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickHandlerUnitSelect : ClickHandler
{
    public Color AttackColor;
    public Color ValidAttackColor;

    private Color MovementColor;
    // private List<List<GameObject>> movementIndicatorObjectDictionary = new List<List<GameObject>>();//new Dictionary<RuleManager.Coordinate, GameObject>();
    // private List<List<GameObject>> attackIndicatorObjectDictionary = new List<List<GameObject>>();//new Dictionary<RuleManager.Coordinate, GameObject>();
    private int createdAttackRangedId = -1;
    private int createdMovementRangedId = -1;
    
    public UnitInfo selectedUnit;

    private bool abilityActionSelected = false;
    private bool AttackActionSelected = false;
    private bool moveActionSelected = false;
    private bool m_RotateActionSelected = false;

    public GameObject MovementRange;
    public GameObject attackRange;
    public GameObject ClickHandlerAbilityPrefab;
    private AbilityClickHandler ClickHandlerAbility;

    CanvasUiScript canvasUIScript;
    public void Start()
    {

    }





    public override void Setup(MainUI ui)
    {
        mainUi = ui;
    //    CreateMovementObjects();
    //    CreateAttackObjects();
        ruleManager = mainUi.ruleManager; 
        GameObject tempObject = Instantiate(ClickHandlerAbilityPrefab, new Vector3(), new Quaternion());
        ClickHandlerAbility = (AbilityClickHandler) tempObject.GetComponent<ClickHandler>();
        ClickHandlerAbility.Setup(ui);
        ClickHandlerAbility.clickHandlerUnitSelect = this;
        ClickHandlerAbility.ruleManager = ui.ruleManager;
        
    }

    void Update()
    {
            
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            resetSelection();
        }
        else if(m_RotateActionSelected)
        {

            if(Input.GetKeyDown(KeyCode.D))
            {
                m_CurrentRotation = new Coordinate(1, 0);
                p_CreateRotationObjects(selectedUnit.UnitID,m_CurrentRotation);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                m_CurrentRotation = new Coordinate(-1, 0);
                p_CreateRotationObjects(selectedUnit.UnitID, m_CurrentRotation);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                m_CurrentRotation = new Coordinate(0,-1);
                p_CreateRotationObjects(selectedUnit.UnitID, m_CurrentRotation);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                m_CurrentRotation = new Coordinate(0, 1);
                p_CreateRotationObjects(selectedUnit.UnitID, m_CurrentRotation);
            }
            else if(Input.GetKeyDown(KeyCode.Return))
            {
                p_CommitRotation();
            }
        }
        
    }
    public override void OnClick(ClickType clickType, Coordinate cord)
    {
        if(abilityActionSelected)
        {
            ClickHandlerAbility.OnClick(clickType, cord);
            return; 
        }
        canvasUIScript = mainUi.canvasUIScript;
        if (moveActionSelected && selectedUnit.PlayerIndex == ruleManager.getPlayerPriority())
        {
            print("kommer den hitr");
            RuleManager.MoveAction moveAction = new RuleManager.MoveAction();
            moveAction.NewPosition = cord;
            moveAction.UnitID = selectedUnit.UnitID;
            moveAction.PlayerIndex = ruleManager.getPlayerPriority();//GlobalNetworkState.LocalPlayerIndex;
            string moveInfo;
            if (ruleManager.ActionIsValid(moveAction, out moveInfo))
            {
                mainUi.EnqueueAction(moveAction);
            }
            else
            {
                canvasUIScript.errorMessage(moveInfo);
            }
        }
        moveActionSelected = false;

        if (AttackActionSelected && selectedUnit.PlayerIndex == ruleManager.getPlayerPriority() && ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID != 0)
        {
            RuleManager.AttackAction attackAction = new RuleManager.AttackAction();
            attackAction.AttackerID = selectedUnit.UnitID;
            attackAction.DefenderID = ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID;
            attackAction.PlayerIndex = ruleManager.getPlayerPriority();
            string attackInfo;

            if (ruleManager.ActionIsValid(attackAction, out attackInfo))
            {
                mainUi.EnqueueAction(attackAction);
            }
            else
            {
                canvasUIScript.errorMessage(attackInfo);
            }
        }

        if (ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID != 0 && !AttackActionSelected)
        {
            DestroyMovementRange();
            selectedUnit = ruleManager.GetUnitInfo(ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID);
            print("Selected ID: "+selectedUnit.UnitID);
            //clicking on unit, play sound
            var UIInfo = mainUi.GetUnitUIInfo(selectedUnit);
            if(UIInfo.UIInfo.SelectSound!= null)
            {
                //GetComponent<AudioSource>().PlayOneShot(UIInfo.UIInfo.SelectSound);
            }
            resetSelection();
            canvasUIScript.createUnitCard(selectedUnit);
            ConstructMovementRange(selectedUnit);
        }
        else
        {
            resetSelection();
            mainUi.DeactivateClickHandler();
        }
    }

    public override void Deactivate()
    {
        canvasUIScript.DisableUnitCard();
        ClickHandlerAbility.Deactivate();
        resetSelection();
        DestroyMovementRange();
        DestroyAttackRange();
    }

    public override bool OnHandleClick(ClickType clickType, Coordinate cord)
    {
        if(ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID != 0)
        {

            return true; 
            
        }
        if(moveActionSelected || AttackActionSelected)
        {
            return true;
        }
        return false; 
    }
    public  void resetSelection()
    {   
        if(abilityActionSelected)
        {
            abilityActionSelected = false;

            DeactivateAbilityClickHandler();
            return;
        }
        if(AttackActionSelected)
        {
            AttackActionSelected = false;
            DestroyAttackRange();

            ConstructMovementRange(selectedUnit);
            return;
        }
        if(moveActionSelected)
        {
            moveActionSelected = false;
            return;
        }

        if(!abilityActionSelected)
        {
            canvasUIScript.DisableUnitCard();
        }
        p_DestroySubElements();
        DestroyMovementRange();
        DestroyAttackRange();

        DeactivateAbilityClickHandler();
    }
    private void DestroyButtons()
    {

    }

    public void DeactivateAbilityClickHandler()
    {
        bool WasActionSelected = abilityActionSelected;
        abilityActionSelected = false;
        if(WasActionSelected)
        {
            ClickHandlerAbility.Deactivate();
        }
    }

    private void ConstructMovementRange(RuleManager.UnitInfo info)
    {
    
        DestroyMovementRange();
        DestroyAttackRange();
        
        TileColoringEffect tileColorEffect = new TileColoringEffect(Color.blue, null, ruleManager.PossibleMoves(info.UnitID));
        createdMovementRangedId = mainUi.AddColoringEffect(tileColorEffect);


    //   int Height = info.Stats.Movement;
    //   print("Movement range for ID: " + info.UnitID);
    //
    //   foreach (RuleManager.Coordinate cord in ruleManager.PossibleMoves(info.UnitID))
    //   {
    //       movementIndicatorObjectDictionary[cord.X][cord.Y].SetActive(true);
    //       if((info.Flags & UnitFlags.HasMoved) != 0)
    //       {
    //           movementIndicatorObjectDictionary[cord.X][cord.Y].GetComponent<SpriteRenderer>().color = new Color(MovementColor.r,MovementColor.g,MovementColor.b,MovementColor.a/2);
    //       }
    //   }


    }
    private void ConstructAttackRange(RuleManager.UnitInfo info)
    {
        DestroyMovementRange();
        DestroyAttackRange();
        
        TileColoringEffect tileColorEffect = new TileColoringEffect(Color.red, null, ruleManager.PossibleAttacks(info.UnitID));
        createdAttackRangedId = mainUi.AddColoringEffect(tileColorEffect);

    //
    //
    //    foreach (RuleManager.Coordinate cord in ruleManager.PossibleAttacks(info.UnitID))
    //    {
    //        attackIndicatorObjectDictionary[cord.X][cord.Y].SetActive(true);
    //        if(ruleManager.GetTileInfo(cord.X,cord.Y).StandingUnitID != 0)
    //        {
    //            AttackAction ActionToTest = new AttackAction(info.UnitID, ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID);
    //            ActionToTest.PlayerIndex = info.PlayerIndex;
    //            string Error;
    //            if (ruleManager.ActionIsValid(ActionToTest, out Error))
    //            {
    //                print("Defender ID: " + ActionToTest.DefenderID);
    //                attackIndicatorObjectDictionary[cord.X][cord.Y].GetComponent<SpriteRenderer>().color = ValidAttackColor;
    //            }
    //        }
    //    }


    }

     public void DestroyMovementRange()
     {

        if(createdMovementRangedId != -1)
        {

            mainUi.RemoveColoringEffect(createdMovementRangedId);
        }
        else
        {
            createdMovementRangedId = -1;
        }

   
      //   foreach (List<GameObject> obj in movementIndicatorObjectDictionary)
      //   {
      //       //obj.SetActive(false);
      //
      //       foreach (GameObject ob in obj)
      //       {
      //           ob.SetActive(false);
      //           ob.GetComponent<SpriteRenderer>().color = MovementColor;
      //       }
      //   }
   
     }
     public void DestroyAttackRange()
     {

        if (createdAttackRangedId != -1)
        {

            mainUi.RemoveColoringEffect(createdAttackRangedId);
        }
        else
        {
            createdAttackRangedId = -1;
        }

        //   foreach (List<GameObject> obj in attackIndicatorObjectDictionary)
        //   {
        //       //obj.SetActive(false);
        //
        //       foreach (GameObject ob in obj)
        //       {
        //           ob.SetActive(false);
        //           ob.GetComponent<SpriteRenderer>().color = AttackColor;
        //       }
        //   }

    }

    public Color RotationColor;
    public Color InvalidRotationColor;
    List<GameObject> m_RotationObjects = new List<GameObject>();
    Coordinate m_CurrentRotation = null;
    private void p_CreateRotationObjects(int UnitID,Coordinate Rotation)
    {
        foreach(GameObject Object in m_RotationObjects)
        {
            Destroy(Object);
        }
        m_RotationObjects.Clear();
        UnitInfo RotatedUnit = mainUi.ruleManager.GetUnitInfo(UnitID);
        m_CurrentRotation = Rotation;
        print("Rotation X: " + m_CurrentRotation.X + " Y: " + m_CurrentRotation.Y);
        foreach (Coordinate Coord in RotatedUnit.GetRotatedOffsets(m_CurrentRotation))
        {
            GameObject newObject = Instantiate(attackRange);
            RuleManager.Coordinate tempCord = Coord+RotatedUnit.TopLeftCorner;
            newObject.transform.eulerAngles = mainUi.gridManager.GetEulerAngle();
            newObject.GetComponent<SpriteRenderer>().color = RotationColor;
            newObject.transform.position = mainUi.gridManager.GetTilePosition(tempCord);
            m_RotationObjects.Add(newObject);
        }
    }
    private void p_DeactivateRotationObjects()
    {
        foreach(GameObject Object in m_RotationObjects)
        {
            Destroy(Object);
        }
        m_RotationObjects.Clear();
    }
    void p_DestroySubElements()
    {
   //     DestroyMovementRange();
        ClickHandlerAbility.DestroyAbilityRangeIndicator();
     //   DestroyAttackRange();
        p_DeactivateRotationObjects();
        moveActionSelected = false;
        abilityActionSelected = false;
        AttackActionSelected = false;
        m_RotateActionSelected = false;
    }
    void p_CommitRotation()
    {
        string Error;
        RotateAction ActionToExecute = new RotateAction();
        ActionToExecute.PlayerIndex = selectedUnit.PlayerIndex;
        ActionToExecute.UnitID = selectedUnit.UnitID;
        ActionToExecute.NewRotation = m_CurrentRotation;
        if(!mainUi.ruleManager.ActionIsValid(ActionToExecute,out Error))
        {
            canvasUIScript.errorMessage(Error);
            resetSelection();
            return;
        }
        mainUi.EnqueueAction(ActionToExecute);
        resetSelection();
    }
    public void ActivateAttackSelection()
    {
        p_DestroySubElements();
        AttackActionSelected = true;
        ConstructAttackRange(selectedUnit);
    }
    public void ActivateMovementSelection()
    {
        p_DestroySubElements();
        moveActionSelected = true;
        ConstructMovementRange(selectedUnit);
    }
    public void ActivateRotateAction()
    {
        p_DestroySubElements();
        m_RotateActionSelected = true;
        p_CreateRotationObjects(selectedUnit.UnitID,selectedUnit.Direction);
    }
    public void ActivateAbilitySelection()
    {
        p_DestroySubElements();
        abilityActionSelected = true;
        ClickHandlerAbility.ShowAbilityRangeIndicators(selectedUnit.UnitID, ClickHandlerAbility.selectedAbilityIndex, new List<RuleManager.Target>());
    }

    public override void OnHover(Coordinate coordinate)
    {
        if(abilityActionSelected)
        {
            ClickHandlerAbility.OnHover(coordinate);
        }
    }

    public override void OnHoverExit(Coordinate coordinate)
    {
        if (abilityActionSelected)
        {
            ClickHandlerAbility.OnHoverExit(coordinate);
        }
    }
}
