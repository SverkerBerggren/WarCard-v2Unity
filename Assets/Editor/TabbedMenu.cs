// This script attaches the tabbed menu logic to the game.
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
//Inherits from class `MonoBehaviour`. This makes it attachable to a game object as a component.
public class TabbedMenu
{
    private TabbedMenuController controller;

    static VisualElement GetChildWithName(VisualElement NodeToSearch,string Name)
    {
        VisualElement ReturnValue = null;
        foreach(var Child in NodeToSearch.Children())
        {
            if(Child.name == Name)
            {
                return Child;
            }
        }
        return ReturnValue;
    }
    public static VisualElement CreateTabbedMenu(List<string> Names,List<VisualElement> Contents)
    {
        //return new Label("slugma");
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/TabbedMenu.uxml");
        VisualElement baseTree = visualTree.Instantiate();
        VisualElement contentRoot = GetChildWithName(baseTree, "mainContainer");
        VisualElement tabContainer = null;
        VisualElement tabContentContainer = null;
        foreach(var Child in contentRoot.Children())
        {
            if(Child.name == "tabs")
            {
                tabContainer = Child;
            }
            else if(Child.name == "tabContent")
            {
                tabContentContainer = Child;
            }
        }
        tabContainer.Clear();
        tabContentContainer.Clear();
        for(int i = 0; i < Names.Count;i++)
        {
            Label NewTab = new Label();
            NewTab.AddToClassList("tab");
            NewTab.text = Names[i];
            tabContainer.Add(NewTab);

            VisualElement NewContent = Contents[i];
            NewTab.name = Names[i] + "Tab";
            NewContent.name = Names[i] + "Content";
            tabContentContainer.Add(NewContent);
            if(i == 0)
            {
                NewTab.AddToClassList("currentlySelectedTab");
            }
            else
            {
                NewContent.AddToClassList("unselectedContent");
            }
        }
        TabbedMenuController newController = new TabbedMenuController(baseTree);
        newController.RegisterTabCallbacks();
        return baseTree;
    }
    private void OnEnable()
    {
        //UIDocument menu = GetComponent<UIDocument>();
        //VisualElement root = menu.rootVisualElement;
        //
        //controller = new(root);
        //
        //controller.RegisterTabCallbacks();
    }
}