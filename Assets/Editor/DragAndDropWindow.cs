using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
public class DragAndDropWindow : EditorWindow
{
    static int Width = 43;
    static int Height = 31;

    ResourceManager.ResourceManager s_Resources = null;
    
    [MenuItem("Tools/DragAndDrop")]
    public static void ShowExample()
    {
        DragAndDropWindow wnd = GetWindow<DragAndDropWindow>();
        wnd.titleContent = new GUIContent("Drag And Drop");
    }
    //class TableClickHandler : Clickable
    //{
    //    TableClickHandler()
    //    {
    //        this.cli
    //    }
    //}

    public class UserTerrainInfo
    {
        public string Name = "";
        public Sprite Image = null;
    };

    void whenClicked(string ArmyType,int Row,int col)
    {
        Debug.Log("current list index: " + m_SelectedIndex);
        Debug.Log("indices : "+ Row + " "+col);
        if(ArmyType != "Terrain")
        {
            var unit = m_ArmyUnits[ArmyType][m_SelectedIndex];
            m_CurrentSelection[Row][col].UnitName = unit.Name;
            m_CurrentSelection[Row][col].PlayerIndex = m_PlayerIndex;
            var image = m_TableContents[Row][col].Children().First() as Image;
            image.sprite = GetSprite(unit);
        }
        else
        {
            var terrain = m_TerrainInfo[m_SelectedIndex];
            m_CurrentSelection[Row][col].UnitName = terrain.Name;
            m_CurrentSelection[Row][col].PlayerIndex = m_PlayerIndex;
            var image = m_TableContents[Row][col].Children().First() as Image;
            image.sprite = terrain.Image;
        }
    }

    public List<List<VisualElement>> m_TableContents = new List<List<VisualElement>>();
    public List<List<UserTileInfo>> m_CurrentSelection = new List<List<UserTileInfo>>();

    System.Action getLambda(int row, int col)
    {
        return () => { whenClicked(m_SelectedArmy, row, col); };
    }
    VisualElement CreateRow(int rowIndex , int ColCount)
    {
        VisualElement ReturnValue = new VisualElement();
        ReturnValue.AddToClassList("slot_row");
        List<VisualElement> TableRow = new();
        List<UserTileInfo> SelectionRow = new();
        for(int i = 0; i < ColCount;i++)
        {
            Button Cell = new Button();
            Cell.clicked += getLambda(rowIndex, i);
            Cell.AddToClassList("slot");
            ReturnValue.Add(Cell);
            Cell.Add(new Image());
            SelectionRow.Add(new UserTileInfo());
            TableRow.Add(Cell);
        }
        m_CurrentSelection.Add(SelectionRow);
        m_TableContents.Add(TableRow);
        return ReturnValue;
    }


    Dictionary<string, List<ResourceManager.UnitResource>> m_ArmyUnits = new Dictionary<string, List<ResourceManager.UnitResource>>();
    List<UserTerrainInfo> m_TerrainInfo = new();
    Dictionary<string,Sprite> m_TerrainNameToSprite = new();
    string m_SelectedArmy = "";
    int m_SelectedIndex = 0;

    UnityEngine.Sprite GetSprite(ResourceManager.UnitResource Resource)
    {
        UnityEngine.Sprite ReturnValue = null;
        var DownInfo = Resource.UIInfo.DownAnimation;
        if(DownInfo.VisualInfo is ResourceManager.Visual_Image)
        {
            ReturnValue = (DownInfo.VisualInfo as ResourceManager.Visual_Image).Sprite;
        }
        else if(DownInfo.VisualInfo is ResourceManager.Visual_Animation)
        {
            ReturnValue = (DownInfo.VisualInfo as ResourceManager.Visual_Animation).AnimationContent[0];
        }
        return ReturnValue;
    }


    void SelectionChangeTerrain(IEnumerable<int> selectedItems)
    {

        m_SelectedArmy = "Terrain";
        m_SelectedIndex = selectedItems.First();
    }

    VisualElement createArmyTabs()
    {
        string ArmyType = "all";
        List<ResourceManager.UnitResource> Units = new List<ResourceManager.UnitResource>();
        Debug.Log("loaded units: " + s_Resources.m_LoadedUnitInfos.Count);
        foreach(var Unit in s_Resources.GetUnits())
        {
            Units.Add(Unit);
        }
        m_ArmyUnits[ArmyType] = Units;
        ListView VisualList = new ListView();
        VisualList.itemsSource = Units;
        VisualList.makeItem = () => { var result = new Image(); result.style.height = new StyleLength(new Length(120, LengthUnit.Percent)); return result; };
        VisualList.bindItem = (item, index) => { (item as Image).sprite = GetSprite(Units[index]); };
        VisualList.itemsSource = Units;
        VisualList.onSelectedIndicesChange += OnSelectionChangeBruh;


        ListView TerrainList = new ListView();
        var ImpassableSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Prefabs/MovementRange.png");
        var ObjectiveSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/PH_Objective_4.png");

        var ImpassableTerrain = new UserTerrainInfo();
        var ObjectiveTerrain = new UserTerrainInfo();
        ImpassableTerrain.Name = "Impassable";
        ImpassableTerrain.Image = ImpassableSprite;
        ObjectiveTerrain.Name = "Objective";
        ObjectiveTerrain.Image = ObjectiveSprite;
        m_TerrainNameToSprite["Objective"] = ObjectiveSprite;
        m_TerrainNameToSprite["Impassable"] = ImpassableSprite;
        m_TerrainInfo = new List<UserTerrainInfo> { ImpassableTerrain, ObjectiveTerrain };
        TerrainList.itemsSource = m_TerrainInfo;
        TerrainList.makeItem = () => { var result = new Image(); result.style.height = new StyleLength(new Length(120, LengthUnit.Percent)); return result; };
        TerrainList.bindItem = (item, index) => { (item as Image).sprite = m_TerrainInfo[index].Image; };
        TerrainList.onSelectedIndicesChange += SelectionChangeTerrain;

        //terrain tab


        return TabbedMenu.CreateTabbedMenu(new List<string> { "all" ,"terrain"}, new List<VisualElement> { VisualList , TerrainList});
    }
    private void OnSelectionChangeBruh(IEnumerable<int> selectedItems)
    {
        // Clear all previous content from the pane
        m_SelectedArmy = "all";
        m_SelectedIndex = selectedItems.First();
    }
    int m_PlayerIndex = 0;

    class JsonContainer
    {
        public List<List<UserTileInfo>> content;
    }
    void saveFile(string Filepath)
    {
        FileStream outFile = new FileStream(Filepath,FileMode.Create);

        var data = System.Text.Encoding.UTF8.GetBytes(MBJson.JSONObject.SerializeObject(m_CurrentSelection).ToString());
        Debug.Log("current width and height: " + m_TableContents.Count + " " + m_TableContents[0].Count);
        outFile.Write(data);
        outFile.Flush();
        outFile.Close();
    }
    void loadFile(string Filepath)
    {
        FileStream inFile = new FileStream(Filepath, FileMode.OpenOrCreate);
        var Content = System.Text.UTF8Encoding.UTF8.GetBytes(new StreamReader(inFile).ReadToEnd());
        var JSONContent = MBJson.JSONObject.ParseJSONObject(Content);
        var newData = MBJson.JSONObject.DeserializeObject<List<List<UserTileInfo>>>(JSONContent);
        Debug.Log(JSONContent.ToString());
        Debug.Log("width and height: " + newData.Count + " " + newData[0].Count);
        Debug.Log("current width and height: " + m_TableContents.Count + " " + m_TableContents[0].Count);
        if( newData.Count == Width && newData[0].Count == Height)
        {
            m_CurrentSelection = newData;
            for(int rowIndex = 0; rowIndex < newData.Count;rowIndex++)
            {
                for(int colIndex = 0; colIndex < newData[0].Count; colIndex++)
                {
                    string Unitname = m_CurrentSelection[rowIndex][colIndex].UnitName;
                    if (Unitname != "")
                    {
                        if(m_TerrainNameToSprite.ContainsKey(Unitname))
                        {
                            (m_TableContents[rowIndex][colIndex].Children().First() as Image).sprite = m_TerrainNameToSprite[Unitname];
                        }
                        else
                        {
                            (m_TableContents[rowIndex][colIndex].Children().First() as Image).sprite = GetSprite(s_Resources.GetUnitResource(Unitname));
                        }
                    }
                    else
                    {
                        (m_TableContents[rowIndex][colIndex].Children().First() as Image).sprite = null;
                    }
                }
            }
        }
        else
        {
            Debug.Log("Didnt load file, society");
        }
        inFile.Close();
    }

    void HandleKeyDown(KeyDownEvent currentEvent)
    {
        Debug.Log("asdasdasdasdsd");
        if(currentEvent.keyCode == KeyCode.O)
        {
            string path = EditorUtility.OpenFilePanel("file to open", Application.streamingAssetsPath, "json");
            loadFile(path);
        }
        else if(currentEvent.keyCode == KeyCode.S && (currentEvent.modifiers & EventModifiers.Control) != 0)
        {
            string path = EditorUtility.SaveFilePanel("file to save", Application.streamingAssetsPath,"", "json");
            saveFile(path);
        }
    }
    public void CreateGUI()
    {


        s_Resources = new ResourceManager.ResourceManager(Application.streamingAssetsPath);
        var temp = new RuleManager.RuleManager( (uint)Width, (uint)Height);
        s_Resources.GetScriptHandler().AddBuiltins(temp.GetUnitScriptFuncs());
        s_Resources.LoadResourceFolder(Application.streamingAssetsPath);
        Debug.Log(Application.streamingAssetsPath);
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        //root.AddManipulator(KeyboardManipulator);
        root.RegisterCallback<KeyDownEvent>(HandleKeyDown);
        var toggle = new Button();
        toggle.text = "Player 1";
        toggle.clicked += () => { m_PlayerIndex = (m_PlayerIndex + 1) % 2; toggle.text = "Player " + m_PlayerIndex; };
        root.Add(toggle);

        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        var leftPane = createArmyTabs();
        //var leftPane = new Box();
        splitView.Add(leftPane);
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/DragAndDropWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        //root.Add(labelFromUXML);
        splitView.Add(leftPane);
        var Scroll = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
        Scroll.Add(labelFromUXML);
        splitView.Add(Scroll);
        root.Add(splitView);

        //add rows
        VisualElement RowContainer = null;
        foreach(var Child in labelFromUXML.Children())
        {
            if(Child.name == "slots")
            {
                RowContainer = Child;
            }
            break;
        }
        m_CurrentSelection = new List<List<UserTileInfo>>();
        if (RowContainer != null)
        {
            RowContainer.Clear();
            int CurrentRow = 0;
            for (int i = 0; i < Width; i++)
            {
                RowContainer.Add(CreateRow(CurrentRow, Height));
                CurrentRow++;
            }
        }

        //test test
        //System.Collections.IEnumerable data =(System.Collections.IEnumerable) ((object) new List<List<int>> { new List<int> { 1, 2, 3 }, new List<int> { 4, 5, 6 } });
        System.Collections.IEnumerable data = (System.Collections.IEnumerable)m_CurrentSelection;
        foreach (var element in data)
        {
            Debug.Log(element);
        }
        Debug.Log("bruh " + m_CurrentSelection.Count + " " + m_CurrentSelection[0].Count);
        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DragAndDropWindow.uss");
    }
}
