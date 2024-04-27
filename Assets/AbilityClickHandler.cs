using RuleManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityClickHandler : ClickHandler
{   

    private List<GameObject> buttonDestroyList = new List<GameObject>();
    private RuleManager.UnitInfo selectedUnit = new RuleManager.UnitInfo();
    private List<RuleManager.TargetCondition> requiredAbilityTargets = new List<RuleManager.TargetCondition>();
    public int selectedAbilityIndex = 0;


    private int currentTargetToSelect = 0;
    private List<RuleManager.Target> selectedTargetsForAbilityExecution = new List<RuleManager.Target>();

    public  ClickHandlerUnitSelect clickHandlerUnitSelect;

    private Color CorrectTargetColor = new Color(0,0.5f,0.5f);
    private Color TargetRangeColor = new Color(0,0.5f,0.5f);
    //private List<List<GameObject>> abilityRangeIndicators = new List<List<GameObject>>();
    // public GameObject abilityRangeIndicator;
    private int createdAbilityRangeId = -1; 
    private int createdValidTargetsRangeId = -1;
    private int abilityHoverId = -1;

    private CanvasUiScript canvasUIScript;
    // Start is called before the first frame update

    public override void OnClick(ClickType clickType, RuleManager.Coordinate cord)
    {

        selectedUnit = clickHandlerUnitSelect.selectedUnit;


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
        if(currentTargetToSelect == requiredAbilityTargets.Count)
        {
            Deactivate();
            return;
        }
         if (ruleManager.p_VerifyTarget(requiredAbilityTargets[currentTargetToSelect], EffectSource, EmptyTargets, targetTile, out ErrorString,true))
        {
            currentTargetToSelect += 1;
            targetWasCorrect = true;
            selectedTargetsForAbilityExecution.Add(targetTile);
        }
        if (currentTargetToSelect < requiredAbilityTargets.Count)
        {
            if (ruleManager.p_VerifyTarget(requiredAbilityTargets[currentTargetToSelect], EffectSource, EmptyTargets, targetUnit, out ErrorString,true) && !targetWasCorrect)
            {
                currentTargetToSelect += 1;
                targetWasCorrect = true;
                selectedTargetsForAbilityExecution.Add(targetUnit);
            }

        }
        if (currentTargetToSelect < requiredAbilityTargets.Count)
            ShowAbilityRangeIndicators(selectedUnit.UnitID, selectedAbilityIndex, selectedTargetsForAbilityExecution);


        print("Targets count: " + selectedTargetsForAbilityExecution.Count);


        if (requiredAbilityTargets.Count == currentTargetToSelect)
        {

            //   print("Executas ability");

            RuleManager.EffectAction abilityToExecute = new RuleManager.EffectAction();

            abilityToExecute.EffectIndex = selectedAbilityIndex;
            abilityToExecute.PlayerIndex = ruleManager.getPlayerPriority(); ;

            List<RuleManager.Target> argumentList = new List<RuleManager.Target>(selectedTargetsForAbilityExecution);

            abilityToExecute.Targets = argumentList;

            abilityToExecute.UnitID = selectedUnit.UnitID;

            if(ruleManager.GetUnitInfo(selectedUnit.UnitID).PlayerIndex == abilityToExecute.PlayerIndex)
            {
                if(GlobalNetworkState.IsLocal ||(selectedUnit.PlayerIndex == GlobalNetworkState.LocalPlayerIndex))
                {
                    mainUi.EnqueueAction(abilityToExecute);
                    selectedUnit.AbilityActivationCount[selectedAbilityIndex] = 1;
                    canvasUIScript.createUnitCard(selectedUnit);
                }
            }

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
        selectedUnit = new RuleManager.UnitInfo();
         
        requiredAbilityTargets = new List<RuleManager.TargetCondition>();
        selectedAbilityIndex = 0;
        selectedUnit = new RuleManager.UnitInfo();

        currentTargetToSelect = 0;
        selectedTargetsForAbilityExecution = new List<RuleManager.Target>();
        clickHandlerUnitSelect.DeactivateAbilityClickHandler();
        RemoveAbilityHover();

        DestroyAbilityRangeIndicator();

    }

    public void resetSelection()
    {
        Deactivate();
    }

   
    public override void Setup(MainUI ui)
    {
        mainUi = ui;
        canvasUIScript = ui.canvasUIScript;
       // CreateAbilityRangeIndicators();


    }
    

    public void ShowAbilityRangeIndicators(int UnitID, int effectIndex, List<Target> currentTargets)
    {
        DestroyAbilityRangeIndicator();
        createdAbilityRangeId =  mainUi.AddColoringEffect(new TileColoringEffect(TargetRangeColor,null,ruleManager.GetAbilityRange(UnitID, effectIndex, currentTargets)));
 
    } 
    public void DestroyAbilityRangeIndicator()
    {
        if(createdAbilityRangeId != -1)
        {
            mainUi.RemoveColoringEffect(createdAbilityRangeId);
            mainUi.RemoveColoringEffect(createdValidTargetsRangeId);
        }

    }

    public override void OnHover(Coordinate coordinate)
    {
        List<Target> currentTargetsWithCoordinate = new List<Target>(selectedTargetsForAbilityExecution);
        currentTargetsWithCoordinate.Add(new Target_Tile( coordinate));
        if(ruleManager.GetAbilityHover(clickHandlerUnitSelect.selectedUnit.UnitID, selectedAbilityIndex, currentTargetsWithCoordinate).Count == 0)
        {
            RemoveAbilityHover();
            return;
        }
        print("Kommer den hit till ability clickhandlern ");
        abilityHoverId =  mainUi.AddColoringEffect( new TileColoringEffect( Color.green, null,ruleManager.GetAbilityHover(clickHandlerUnitSelect.selectedUnit.UnitID, selectedAbilityIndex, currentTargetsWithCoordinate))); 
    }

    public override void OnHoverExit(Coordinate coordinate)
    {
        RemoveAbilityHover();
    }

    private void RemoveAbilityHover()
    {
        if (abilityHoverId != -1)
        {
            mainUi.RemoveColoringEffect(abilityHoverId);
            abilityHoverId = -1;
        }
    }

    public void Setup(int selectedAbilityIndex, List<RuleManager.TargetCondition> requiredAbilityTargets)
    {
        this.selectedAbilityIndex = selectedAbilityIndex;
        this.requiredAbilityTargets = requiredAbilityTargets;
    }
}
