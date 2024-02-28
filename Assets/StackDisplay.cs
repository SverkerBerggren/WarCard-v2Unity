using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDisplay : MonoBehaviour, RuleManager.StackEventHandler,RestorableUIElement
{
    // Start is called before the first frame update

    public GameObject StackEntryPrefab;

    MainUI m_UI = null;
    bool m_Visible = false;

    int m_StackSize = 0;

    List<StackEntry> m_CurrentChildren = new();
    void p_ToggleVisability(bool IsVisible)
    {
        gameObject.SetActive(IsVisible);
        m_Visible = IsVisible;
    }
    void Awake()
    {
        m_UI = MainUI.GetStaticInstance();
        m_UI.RegisterStackEventHandler(this);
        m_UI.RegisterGameRestorableUIElement(this);
        p_ToggleVisability(m_Visible);
    }

    public void RestoreFromGamestate( RuleManager.RuleManager CurrentGamestate)
    {
        foreach(var Entity in CurrentGamestate.GetStack())
        {
            OnStackPush(Entity);
        }
    }

    void ResetOtherElements(int ActiveIndex)
    {
        for(int i = 0; i < m_CurrentChildren.Count;i++)
        {
            if(i != ActiveIndex)
            {
                m_CurrentChildren[i].ResetStackEntry();
            }
        }
    }

    public void OnStackPush(RuleManager.StackEntity NewElement)
    {
        GameObject NewStackObject = Instantiate(StackEntryPrefab);
        NewStackObject.transform.parent = transform;
        NewStackObject.transform.position = new Vector3(0, 0, 0);
        NewStackObject.transform.localPosition = new Vector3(0, 220, 0);
        NewStackObject.transform.localPosition += new Vector3(0, -(m_StackSize * 50));
        StackEntry NewEntry = NewStackObject.GetComponent<StackEntry>();
        NewEntry.Initialize(ResetOtherElements, m_StackSize, NewElement);
        m_CurrentChildren.Add(NewEntry);
        p_ToggleVisability(true);
        m_StackSize += 1;
    }
    public void OnStackPop(RuleManager.StackEntity NewElement)
    {
        m_StackSize -= 1;
        m_CurrentChildren[m_CurrentChildren.Count - 1].ResetStackEntry();
        Destroy(m_CurrentChildren[m_CurrentChildren.Count - 1].gameObject);
        m_CurrentChildren.RemoveAt(m_CurrentChildren.Count - 1);
        if(m_StackSize == 0)
        {
            p_ToggleVisability(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
