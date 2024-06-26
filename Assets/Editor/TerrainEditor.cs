using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements;

public class TerrainEditor : EditorWindow
{
    [SerializeField] private int m_SelectedIndex = -1;
    private VisualElement m_RightPane;

    [MenuItem("Tools/My Custom Editor")]
    public static void ShowMyEditor()
    {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<TerrainEditor>();
        wnd.titleContent = new GUIContent("My Custom Editor");

        // Limit size of the window
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }
    List<Sprite> allObjects = new List<Sprite>();
    public void CreateGUI()
    {
        // Get a list of all sprites in the project
        var allObjectGuids = AssetDatabase.FindAssets("t:Sprite");
        foreach (var guid in allObjectGuids)
        {
          allObjects.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        // Create a two-pane view with the left pane being fixed with
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        // Add the panel to the visual tree by adding it as a child to the root element
        rootVisualElement.Add(splitView);

        // A TwoPaneSplitView always needs exactly two child elements
        var leftPane = new ListView();
        splitView.Add(leftPane);
        //m_RightPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
        m_RightPane = new VisualElement();

        //test content
        var image = new Label();
        image.text = "slugma";
        var FirstChild = new Box();
        FirstChild.Add(image);
        FirstChild.Add(image);
        FirstChild.Add(new Slider());
        FirstChild.Add(new Label("bruh"));
        var secondChild = new Box();
        secondChild.Add(image);
        secondChild.Add(image);
        secondChild.Add(new Label("bruh bruh bruh"));
        secondChild.Add(image);
        m_RightPane.Add(FirstChild);
        m_RightPane.Add(secondChild);
        

        splitView.Add(m_RightPane);

        // Initialize the list view with all sprites' names
        leftPane.makeItem = () => new Label();
        leftPane.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        leftPane.itemsSource = allObjects;

        // React to the user's selection
        leftPane.onSelectionChange += OnSpriteSelectionChange;

        // Restore the selection index from before the hot reload
        leftPane.selectedIndex = m_SelectedIndex;

        // Store the selection index when the selection changes
        leftPane.onSelectionChange += (items) => { m_SelectedIndex = leftPane.selectedIndex; };
    }

    private void OnSpriteSelectionChange(IEnumerable<object> selectedItems)
    {
        return;
        // Clear all previous content from the pane
        m_RightPane.Clear();

        // Get the selected sprite
        var selectedSprite = selectedItems.First() as Sprite;
        if (selectedSprite == null)
          return;

        // Add a new Image control and display the sprite
        var spriteImage = new Image();
        spriteImage.scaleMode = ScaleMode.ScaleToFit;
        spriteImage.sprite = selectedSprite;

        // Add the Image control to the right-hand pane
        m_RightPane.Add(spriteImage);
    }
}
