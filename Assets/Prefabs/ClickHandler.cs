using RuleManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClickHandler : MonoBehaviour
{
    public RuleManager.RuleManager  ruleManager;
    public MainUI mainUi;

    // Start is called before the first frame update

    public abstract void OnHover(Coordinate coordinate);
    public abstract void OnHoverExit(Coordinate coordinate);
    public abstract void OnClick(ClickType clickType, RuleManager.Coordinate cord);

    public abstract void Deactivate();
    public abstract bool OnHandleClick(ClickType clickType, Coordinate cord);
    public abstract void Setup(MainUI mainUI);
}
