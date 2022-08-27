using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // Start is called before the first frame update
    System.Action<bool> m_ReadyCallback = null;
    System.Action<int> m_FactionIndexCallback = null;

    //Ugly af
    public GameObject FactionIconObject = null;
    public GameObject CheckmarkObject = null;
    public GameObject NameObject = null;
    public List<Sprite> FactionIcons = new List<Sprite>();

    bool m_IsReady = false;
    int m_FactionIndex = 0;
    int m_FactionMax = 2;


    bool m_IsInteractive = true;
    void Start()
    {
        
    }

    public void ReadyClick()
    {
        if (!m_IsInteractive)
        {
            return;
        }
        m_IsReady = !m_IsReady;
        if(m_ReadyCallback != null)
        {
            m_ReadyCallback(m_IsReady);
        }
        CheckmarkObject.SetActive(m_IsReady);
    }
    public void FactionClick()
    {
        if (!m_IsInteractive)
        {
            return;
        }
        m_FactionIndex += 1;
        m_FactionIndex %= m_FactionMax;
        if(m_FactionIndexCallback != null)
        {
            m_FactionIndexCallback(m_FactionIndex);
        }
        FactionIconObject.GetComponent<UnityEngine.UI.Image>().sprite = FactionIcons[m_FactionIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetInteractive(bool IsInteractive)
    {
        m_IsInteractive = IsInteractive;
    }
    public void SetName(string NewName)
    {
        NameObject.GetComponent<TMPro.TextMeshProUGUI>().text = NewName;
    }
    public void SetFactionIndex(int NewIndex)
    {
        m_FactionIndex = NewIndex;
        FactionIconObject.GetComponent<UnityEngine.UI.Image>().sprite = FactionIcons[m_FactionIndex];
    }
    public void SetReady(bool IsReady)
    {
        m_IsReady = IsReady;
        CheckmarkObject.SetActive(m_IsReady);
    }
    public void SetFactionIndexCallback(System.Action<int> Callback)
    {
        m_FactionIndexCallback = Callback;
    }

    public void SetReadyCallback(System.Action<bool> Callback)
    {
        m_ReadyCallback = Callback;
    }
}
