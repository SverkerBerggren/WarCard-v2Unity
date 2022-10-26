using RuleManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickHandlerUnitSelect : ClickHandler
{
    private List<List<GameObject>> movementIndicatorObjectDictionary = new List<List<GameObject>>();//new Dictionary<RuleManager.Coordinate, GameObject>();
    private List<GameObject> buttonDestroyList = new List<GameObject>();
    private UnitInfo selectedUnit;

    public bool abilitySelectionActive = false;
    public bool AttackActionSelected = false;
    public bool moveActionSelected = false;

    public GameObject MovementRange;
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
        ruleManager = mainUi.ruleManager; 
        GameObject tempObject = Instantiate(ClickHandlerAbilityPrefab, new Vector3(), new Quaternion());
        ClickHandlerAbility = tempObject.GetComponent<ClickHandler>();
        ClickHandlerAbility.Setup(ui);
    }

    void Update()
    {
        print("ar moveActionSelected " + moveActionSelected);
    }
    public override void OnClick(ClickType clickType, Coordinate cord)
    {
        if(abilitySelectionActive)
        {
            ClickHandlerAbility.OnClick(clickType, cord);
            return; 
        }

        

        Queue<RuleManager.Action> ExecutedActions = mainUi.ExecutedActions;

        canvasUIScript = mainUi.canvasUIScript;


        if (moveActionSelected && selectedUnit.PlayerIndex == ruleManager.getPlayerPriority())
        {
            print("kommer den hitr");
            RuleManager.MoveAction moveAction = new RuleManager.MoveAction();
            moveAction.NewPosition = cord;
            moveAction.UnitID = selectedUnit.UnitID;
            moveAction.PlayerIndex = GlobalNetworkState.LocalPlayerIndex;
            string moveInfo;
            if (ruleManager.ActionIsValid(moveAction, out moveInfo))
            {
                ExecutedActions.Enqueue(moveAction);
            }
            else
            {
                canvasUIScript.errorMessage(moveInfo);
            }
        }
        moveActionSelected = false;


        if (ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID != 0)
        {
            DestroyMovementRange();
            RuleManager.UnitInfo unitInfo = ruleManager.GetUnitInfo(ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID);
            selectedUnit = unitInfo; 
            if (selectedUnit != null)
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
                            mainUi.canvasUIScript.errorMessage(errorMessageText);


                        }
                        else
                        {

                            ExecutedActions.Enqueue(attackAction);
                            
                        }

                        resetSelection();
                    }

                    print(actionInfo);
                }
            }
            AttackActionSelected = false;
            if (selectedUnit != null)
            {
                if (!abilitySelectionActive && unitInfo.UnitID != selectedUnit.UnitID)
                {
                    foreach (GameObject obj in buttonDestroyList)
                    {
                        obj.SetActive(false);
                    }
                }
            }

            resetSelection();

            selectedUnit = unitInfo;

            canvasUIScript.createUnitCard(unitInfo, mainUi.m_OpaqueToUIInfo);

            ConstructMovementRange(unitInfo);
        }
        else
        {
        //    selectedUnit = null;
        //    unitCard.SetActive(false);
        //    unitActions.SetActive(false);
        //    canvasUIScript.DestroyButtons();
        //
            DestroyMovementRange();
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
        if(moveActionSelected)
        {
            return true;
        }
        return false; 
    }
    public  void resetSelection()
    {
        if(!abilitySelectionActive)
        {
            canvasUIScript.DisableUnitCard();
        }

        if(abilitySelectionActive)
        {
            ClickHandlerAbility.Deactivate(); 
        }
        print("deaktiveras den nagon going");
        moveActionSelected = false;
        AttackActionSelected = false;
    }
    private void DestroyButtons()
    {

    }

    public void DeactivateAbilityClickHandler()
    {
        abilitySelectionActive = false;
        if(ClickHandlerAbility.active)
        {
            ClickHandlerAbility.Deactivate();
        }
    }

    private void ConstructMovementRange(RuleManager.UnitInfo info)
    {


        int Height = info.Stats.Movement;

        //    float xPosition = gridManager.GetTilePosition(info.Position).x;
        //    float yPosition = gridManager.GetTilePosition(info.Position).y;


        foreach (RuleManager.Coordinate cord in ruleManager.PossibleMoves(info.UnitID))
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

    public void DestroyMovementRange()
    {



        foreach (List<GameObject> obj in movementIndicatorObjectDictionary)
        {
            //obj.SetActive(false);

            foreach (GameObject ob in obj)
            {
                ob.SetActive(false);
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

                //    print(tempCord.X + " " + tempCord.Y);
                newObject.transform.position = mainUi.gridManager.GetTilePosition(tempCord);
                movementIndicatorObjectDictionary[i][z] = newObject;
                newObject.SetActive(false);
            }
        }
    }
}
