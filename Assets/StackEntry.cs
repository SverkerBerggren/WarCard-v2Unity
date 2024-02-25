using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class StackEntry : MonoBehaviour,IPointerClickHandler //, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update

    public GameObject TargetIndicator;

    List<GameObject> m_ActiveTargetIndicators = new();
    int m_Index = -1;
    RuleManager.StackEntity m_StackEntity;
    System.Action<int> m_StackOnClick = null;
    MainUI m_UI = null;
    TextMeshProUGUI m_TextComponent = null;
    void Start()
    {
    }
    public void ResetStackEntry()
    {
        m_TextComponent.enabled = false;
        foreach(var Object in m_ActiveTargetIndicators)
        {
            Destroy(Object);
        }
        m_ActiveTargetIndicators.Clear();
    }
    public void Initialize(System.Action<int> Handler, int Index, RuleManager.StackEntity AssociatedEntity)
    {
        m_StackOnClick = Handler;
        m_Index = Index;
        m_UI = MainUI.GetStaticInstance();
        m_StackEntity = AssociatedEntity;
        m_TextComponent = GetComponentInChildren<TextMeshProUGUI>();
        m_TextComponent.text = AssociatedEntity.EffectToResolve.GetText();
        if(AssociatedEntity.Source is RuleManager.EffectSource_Unit)
        {
            var Source = AssociatedEntity.Source as RuleManager.EffectSource_Unit;
            var UnitInfo = m_UI.UnitIDToResource(Source.UnitID);
            if(UnitInfo != null)
            {
                if(UnitInfo.UIInfo.AbilityIcons.ContainsKey(Source.EffectIndex))
                {
                    var Visual = UnitInfo.UIInfo.AbilityIcons[Source.EffectIndex].VisualInfo;
                    if(Visual is ResourceManager.Visual_Image)
                    {
                        GetComponentInChildren<Image>().sprite = (Visual as ResourceManager.Visual_Image).Sprite;
                    }
                }
            }
        }
    }

    float m_DoubleClickDelay = 0.2f;
    float m_ElapsedClickTime = -1;


    void SingleClick()
    {
        m_StackOnClick(m_Index);
        m_TextComponent.enabled = !m_TextComponent.enabled;
    }
    void DoubleClick()
    {
        m_StackOnClick(m_Index);
        //display target indicators
        foreach(var Target in m_StackEntity.Targets)
        {
            RuleManager.Coordinate TargetCoordinate = null;
            if(Target is RuleManager.Target_Tile)
            {
                TargetCoordinate = (Target as RuleManager.Target_Tile).TargetCoordinate;
            }
            else if(Target is RuleManager.Target_Unit)
            {
                TargetCoordinate = m_UI.GetUnitInfo((Target as RuleManager.Target_Unit).UnitID).TopLeftCorner;
            }
            var NewObject = Instantiate(TargetIndicator);
            NewObject.transform.localScale = new Vector3(5, 5, 5);
            NewObject.GetComponent<SpriteRenderer>().sortingOrder = 1000;
            NewObject.transform.position = m_UI.TileToWorldSpace(TargetCoordinate) + new Vector3(0,14,0);
            m_ActiveTargetIndicators.Add(NewObject);
        }
        if(m_StackEntity.Source is RuleManager.EffectSource_Unit)
        {
            var Source = m_StackEntity.Source as RuleManager.EffectSource_Unit;
            var NewObject = Instantiate(TargetIndicator);
            var UnitPosition = m_UI.GetUnitInfo(Source.UnitID).TopLeftCorner;
            NewObject.transform.localScale = new Vector3(5, 5, 5);
            NewObject.GetComponent<SpriteRenderer>().sortingOrder = 1000;
            NewObject.GetComponent<SpriteRenderer>().color = new Color(0,1,1);
            NewObject.transform.position = m_UI.TileToWorldSpace(UnitPosition) + new Vector3(0, 14, 0); ;
            m_ActiveTargetIndicators.Add(NewObject);
            m_UI.MoveCamera(m_UI.TileToWorldSpace(UnitPosition));
        }
    }
    public void OnPointerClick(PointerEventData Event)
    {
        if(m_ElapsedClickTime >= 0)
        {
            DoubleClick();
            m_ElapsedClickTime = -1;
        }
        else
        {
            m_ElapsedClickTime = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_ElapsedClickTime >= 0)
        {
            m_ElapsedClickTime += Time.deltaTime;
        }
        if(m_ElapsedClickTime >= m_DoubleClickDelay)
        {
            SingleClick();
            m_ElapsedClickTime = -1;
        }
    }
}
