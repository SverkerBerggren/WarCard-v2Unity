﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera m_CameraComponent;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    void Start()
    {
        m_CameraComponent = GetComponent<Camera>();

        //m_Raycaster = GetComponent<GraphicRaycaster>();
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        print(m_Raycaster);
        m_EventSystem = GetComponent<EventSystem>();
    }

    public void SetActive(bool IsActive)
    {
        m_Active = IsActive;
    }

    private bool m_Active = true;

    public float ScrollSpeed = 0.1f;
    public float MouseSpeed = 10;
    public float ZoomSpeed = 1;
    public float MaxZoom = 10;

    Vector3 m_DragOriginWorld = new Vector3();
    // Update is called once per frame
    void Update()
    {
        if(!m_Active)
        {
            return;
        }
        float CurrentScrollSpeed = ScrollSpeed * Time.deltaTime * (m_CameraComponent.orthographicSize / 5);
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, CurrentScrollSpeed, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -CurrentScrollSpeed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-CurrentScrollSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(CurrentScrollSpeed, 0, 0);
        }
        Vector2 Scroll = Input.mouseScrollDelta;
        m_CameraComponent.orthographicSize -= Scroll.y * ZoomSpeed;
        if(m_CameraComponent.orthographicSize < 1)
        {
            m_CameraComponent.orthographicSize = 1;
        }
        if(m_CameraComponent.orthographicSize > MaxZoom)
        {
            m_CameraComponent.orthographicSize = MaxZoom;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse2))
        {
            RaycastHit HitInfo;
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
           // List<RaycastResult> results = new List<RaycastResult>();
            List<RaycastResult> results = new List<RaycastResult>();
            //Raycast using the Graphics Raycaster and mouse click position
       //    print(results);
       //    print(m_PointerEventData);
       //    print(m_Raycaster);
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            if(results.Count != 0)
            {
           //    foreach (RaycastResult result in results)
           //    {
           //       // Debug.Log("Hit " + result.gameObject.name);
           //    }
            }


            bool Result = Physics.Raycast(m_CameraComponent.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, 0, 1000),out HitInfo,10000,(1<<LayerMask.NameToLayer("Clickable")));
         //   print(HitInfo.collider.gameObject.tag);
            if (HitInfo.collider != null && results.Count == 0)
            {
                Clickable Clicker = HitInfo.collider.gameObject.GetComponent<Clickable>();
                if(Clicker != null)
                {
                    ClickType Type = ClickType.rightClick;
                    if(Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        Type = ClickType.leftClick;
                    }
                    else if(Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        Type = ClickType.rightClick;
                    }
                    else if (Input.GetKeyDown(KeyCode.Mouse2))
                    {
                        Type = ClickType.middleClick;
                    }
                    Clicker.OnClick(Type);
                }
            }
            m_DragOriginWorld = m_CameraComponent.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2))
        {
            Vector3 DeltaScreen = (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2));
            Vector3 DeltaWorld = new Vector3((DeltaScreen.x/Screen.width)*m_CameraComponent.orthographicSize*m_CameraComponent.aspect,
                (DeltaScreen.y/Screen.height)*m_CameraComponent.orthographicSize)*2;
            m_CameraComponent.transform.position = m_DragOriginWorld - DeltaWorld;
        }
        
        //m_CameraComponent.setwi += Scroll.y * ZoomSpeed;
        //transform.position += new Vector3(0, 0, Scroll.y) * ZoomSpeed;
    }
}
