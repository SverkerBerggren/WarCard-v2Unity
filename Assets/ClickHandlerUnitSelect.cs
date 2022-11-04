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
    private List<List<GameObject>> movementIndicatorObjectDictionary = new List<List<GameObject>>();//new Dictionary<RuleManager.Coordinate, GameObject>();
    private List<List<GameObject>> attackIndicatorObjectDictionary = new List<List<GameObject>>();//new Dictionary<RuleManager.Coordinate, GameObject>();
    private List<GameObject> buttonDestroyList = new List<GameObject>();
    public UnitInfo selectedUnit;

    private bool abilityActionSelected = false;
    private bool AttackActionSelected = false;
    private bool moveActionSelected = false;

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
        CreateMovementObjects();
        CreateAttackObjects();
        ruleManager = mainUi.ruleManager; 
        GameObject tempObject = Instantiate(ClickHandlerAbilityPrefab, new Vector3(), new Quaternion());
        ClickHandlerAbility = (AbilityClickHandler) tempObject.GetComponent<ClickHandler>();
        ClickHandlerAbility.Setup(ui);
        ClickHandlerAbility.clickHandlerUnitSelect = this;
        ClickHandlerAbility.ruleManager = ui.ruleManager;
        
    }

    void Update()
    {
        print("ar moveActionSelected " + moveActionSelected);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            resetSelection();
        }
    }
    public override void OnClick(ClickType clickType, Coordinate cord)
    {
        if(abilityActionSelected)
        {
            ClickHandlerAbility.OnClick(clickType, cord);
            return; 
        }

        

        //Queue<RuleManager.Action> ExecutedActions = mainUi.ExecutedActions;

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
            //clicking on unit, play sound
            Unit UIInfo = mainUi.GetUnitUIInfo(selectedUnit);
            if(UIInfo.SelectSound != null)
            {
                //UnityEngine.Audio.audios(UIInfo.SelectSound, FindObjectOfType<Camera>().transform.position);
                GetComponent<AudioSource>().PlayOneShot(UIInfo.SelectSound);
            }
       //     selectedUnit = unitInfo;
            
        //    if (selectedUnit != null)
        //    {
        //        if (!abilitySelectionActive && unitInfo.UnitID != selectedUnit.UnitID)
        //        {
        //            foreach (GameObject obj in buttonDestroyList)
        //            {
        //                obj.SetActive(false);
        //            }
        //        }
        //    }

            resetSelection();

            

            canvasUIScript.createUnitCard(selectedUnit, mainUi.m_OpaqueToUIInfo);

            ConstructMovementRange(selectedUnit);
        }
        else
        {
        //    selectedUnit = null;
        //    unitCard.SetActive(false);
        //    unitActions.SetActive(false);
        //    canvasUIScript.DestroyButtons();
        //

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
        DestroyMovementRange();
        DestroyAttackRange();

        DeactivateAbilityClickHandler();
 
        print("deaktiveras den nagon going");
        moveActionSelected = false;
        AttackActionSelected = false;
        abilityActionSelected = false;



    }
    private void DestroyButtons()
    {

    }

    public void DeactivateAbilityClickHandler()
    {
        abilityActionSelected = false;
        if(ClickHandlerAbility.active)
        {
            ClickHandlerAbility.Deactivate();
        }
    }

    private void ConstructMovementRange(RuleManager.UnitInfo info)
    {


        int Height = info.Stats.Movement;


        foreach (RuleManager.Coordinate cord in ruleManager.PossibleMoves(info.UnitID))
        {
            movementIndicatorObjectDictionary[cord.X][cord.Y].SetActive(true);
            if((info.Flags & UnitFlags.HasMoved) != 0)
            {
                movementIndicatorObjectDictionary[cord.X][cord.Y].GetComponent<SpriteRenderer>().color = new Color(MovementColor.r,MovementColor.g,MovementColor.b,MovementColor.a/2);
            }
        }


    }
    private void ConstructAttackRange(RuleManager.UnitInfo info)
    {


        int Height = info.Stats.Range;


        foreach (RuleManager.Coordinate cord in ruleManager.PossibleAttacks(info.UnitID))
        {
            attackIndicatorObjectDictionary[cord.X][cord.Y].SetActive(true);
            if(ruleManager.GetTileInfo(cord.X,cord.Y).StandingUnitID != 0)
            {
                AttackAction ActionToTest = new AttackAction(info.UnitID, ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID);
                ActionToTest.PlayerIndex = info.PlayerIndex;
                string Error;
                if (ruleManager.ActionIsValid(ActionToTest, out Error))
                {
                    print("Defender ID: " + ActionToTest.DefenderID);
                    attackIndicatorObjectDictionary[cord.X][cord.Y].GetComponent<SpriteRenderer>().color = ValidAttackColor;
                }
            }
        }


    }

    public void DestroyMovementRange()
    {



        foreach (List<GameObject> obj in movementIndicatorObjectDictionary)
        {
            //obj.SetActive(false);

            foreach (GameObject ob in obj)
            {
                ob.SetActive(false);
                ob.GetComponent<SpriteRenderer>().color = MovementColor;
            }
        }

    }
    public void DestroyAttackRange()
    {



        foreach (List<GameObject> obj in attackIndicatorObjectDictionary)
        {
            //obj.SetActive(false);

            foreach (GameObject ob in obj)
            {
                ob.SetActive(false);
                ob.GetComponent<SpriteRenderer>().color = AttackColor;
            }
        }

    }
    private void CreateMovementObjects()
    {
        for (int i = 0; i < mainUi.gridManager.Width; i++)
        {
            movementIndicatorObjectDictionary.Add(new List<GameObject>());

            for (int z = 0; z < mainUi.gridManager.Height; z++)
            {
                movementIndicatorObjectDictionary[i].Add(null);
            }
        }


        for (int i = 0; i < mainUi.gridManager.Width; i++)
        {
            for (int z = 0; z < mainUi.gridManager.Height; z++)
            {
                GameObject newObject = Instantiate(MovementRange);
                RuleManager.Coordinate tempCord = new RuleManager.Coordinate(i, z);
                MovementColor = newObject.GetComponent<SpriteRenderer>().color;
                //    print(tempCord.X + " " + tempCord.Y);
                newObject.transform.position = mainUi.gridManager.GetTilePosition(tempCord);
                movementIndicatorObjectDictionary[i][z] = newObject;
                newObject.SetActive(false);
            }
        }
    }
    private void CreateAttackObjects()
    {
        for (int i = 0; i < mainUi.gridManager.Width; i++)
        {
            attackIndicatorObjectDictionary.Add(new List<GameObject>());

            for (int z = 0; z < mainUi.gridManager.Height; z++)
            {
                attackIndicatorObjectDictionary[i].Add(null);
            }
        }


        for (int i = 0; i < mainUi.gridManager.Width; i++)
        {
            for (int z = 0; z < mainUi.gridManager.Height; z++)
            {
                GameObject newObject = Instantiate(attackRange);
                RuleManager.Coordinate tempCord = new RuleManager.Coordinate(i, z);
                newObject.GetComponent<SpriteRenderer>().color = AttackColor;
                //    print(tempCord.X + " " + tempCord.Y);
                newObject.transform.position = mainUi.gridManager.GetTilePosition(tempCord);
                attackIndicatorObjectDictionary[i][z] = newObject;
                newObject.SetActive(false);
            }
        }
    }

    public void ActivateAttackSelection()
    {
        moveActionSelected = false;
        abilityActionSelected = false;
        AttackActionSelected = true;
        DestroyMovementRange();
        ClickHandlerAbility.DestroyAbilityRangeIndicator();
        ConstructAttackRange(selectedUnit);
    }
    public void ActivateMovementSelection()
    {
        moveActionSelected = true;
        abilityActionSelected = false;
        AttackActionSelected = false;
        DestroyAttackRange();
        ClickHandlerAbility.DestroyAbilityRangeIndicator();
        ConstructMovementRange(selectedUnit);
    }
    public void ActivateAbilitySelection()
    {
        moveActionSelected = false;
        abilityActionSelected = true;
        AttackActionSelected = false;
        DestroyAttackRange();
        DestroyMovementRange();
        ClickHandlerAbility.ShowAbilityRangeIndicators(selectedUnit.UnitID, ClickHandlerAbility.selectedAbilityIndex, new List<RuleManager.Target>());
    }
}
