using RuleManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityClickHandler : ClickHandler
{   

    public List<GameObject> buttonDestroyList = new List<GameObject>();
    public RuleManager.UnitInfo selectedUnit = new RuleManager.UnitInfo();
    bool abilitySelectionStarted;
    public List<RuleManager.TargetCondition> requiredAbilityTargets = new List<RuleManager.TargetCondition>();
    public int selectedAbilityIndex = 0;
    
    
    int currentTargetToSelect = 0;
    List<RuleManager.Target> selectedTargetsForAbilityExecution = new List<RuleManager.Target>();

    public ClickHandlerUnitSelect clickHandlerUnitSelect;

    private List<List<GameObject>> abilityRangeIndicators = new List<List<GameObject>>();
    public GameObject abilityRangeIndicator;

    public bool active = false;

    CanvasUiScript canvasUIScript;
    // Start is called before the first frame update

    public override void OnClick(ClickType clickType, RuleManager.Coordinate cord)
    {

        selectedUnit = clickHandlerUnitSelect.selectedUnit;

        ShowAbilityRangeIndicators(ruleManager.GetAbilityRange(selectedUnit.UnitID, selectedAbilityIndex, selectedTargetsForAbilityExecution));

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

        List<RuleManager.Target> EmptyTargets = new List<RuleManager.Target>(selectedTargetsForAbilityExecution);
        string ErrorString = "";
        RuleManager.EffectSource_Unit EffectSource = new RuleManager.EffectSource_Unit(ruleManager.GetUnitInfo(selectedUnit.UnitID).PlayerIndex, selectedUnit.UnitID, selectedAbilityIndex);
        if (ruleManager.p_VerifyTarget(requiredAbilityTargets[currentTargetToSelect], EffectSource, EmptyTargets, targetTile, out ErrorString))
        {
            currentTargetToSelect += 1;
            targetWasCorrect = true;
            selectedTargetsForAbilityExecution.Add(targetTile);
        }
        if (currentTargetToSelect < requiredAbilityTargets.Count)
        {
            if (ruleManager.p_VerifyTarget(requiredAbilityTargets[currentTargetToSelect], EffectSource, EmptyTargets, targetUnit, out ErrorString) && !targetWasCorrect)
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


            mainUi.EnqueueAction(abilityToExecute);


            Deactivate();

            print("den gor abiliten");
            currentTargetToSelect = 0;
            //
            return;
            // abilityToExecute.

        }

        if (targetWasCorrect)
        {
            return;
        }

        if (!targetWasCorrect)
        {
            canvasUIScript.errorMessage(ErrorString);
            Deactivate();
            return;
        }


            // return; 
            //List )
        
        
    }
    public override bool OnHandleClick(ClickType clickType, Coordinate cord)
    {
        return true; 
    }
    public override void Deactivate()
    {


        buttonDestroyList = new List<GameObject>();
        selectedUnit = new RuleManager.UnitInfo();
         
        requiredAbilityTargets = new List<RuleManager.TargetCondition>();
        selectedAbilityIndex = 0;
        selectedUnit = new RuleManager.UnitInfo();

        currentTargetToSelect = 0;
        selectedTargetsForAbilityExecution = new List<RuleManager.Target>();
        active = false;
        clickHandlerUnitSelect.DeactivateAbilityClickHandler();

    }

    public void resetSelection()
    {
        buttonDestroyList = new List<GameObject>();
        selectedUnit = new RuleManager.UnitInfo();

        requiredAbilityTargets = new List<RuleManager.TargetCondition>();
        selectedAbilityIndex = 0;

        
        currentTargetToSelect = 0;
        selectedTargetsForAbilityExecution = new List<RuleManager.Target>();

        Deactivate();
    }

   
    public override void Setup(MainUI ui)
    {
        mainUi = ui;
        canvasUIScript = ui.canvasUIScript;
        CreateAbilityRangeIndicators();


    }
    

    public void ShowAbilityRangeIndicators(List<RuleManager.Coordinate> listOfCords)
    {
        foreach (RuleManager.Coordinate cord in listOfCords)
        {
            abilityRangeIndicators[cord.X][cord.Y].SetActive(true);

        }
    } 
    public void CreateAbilityRangeIndicators()
    {
        for (int i = 0; i < mainUi.gridManager.Width; i++)
        {
            abilityRangeIndicators.Add(new List<GameObject>());

            for (int z = 0; z < mainUi.gridManager.Height; z++)
            {
                abilityRangeIndicators[i].Add(null);
            }
        }


        for (int i = 0; i < mainUi.gridManager.Width; i++)
        {
            for (int z = 0; z < mainUi.gridManager.Height; z++)
            {
                GameObject newObject = Instantiate(abilityRangeIndicator);
                RuleManager.Coordinate tempCord = new RuleManager.Coordinate(i, z);

                //    print(tempCord.X + " " + tempCord.Y);
                newObject.transform.position = mainUi.gridManager.GetTilePosition(tempCord);
                abilityRangeIndicators[i][z] = newObject;
                newObject.SetActive(false);
            }
        }
    }    

    public void CreateValidTargetIndicators()
    {

    }

}
