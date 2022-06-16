using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera m_CameraComponent;
    void Start()
    {
        m_CameraComponent = GetComponent<Camera>();
    }
    public float ScrollSpeed = 0.1f;
    public float MouseSpeed = 10;
    public float ZoomSpeed = 1;
    // Update is called once per frame
    void Update()
    {
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
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 MouseDelta;
            MouseDelta.x = Input.GetAxis("Mouse X");
            MouseDelta.y = Input.GetAxis("Mouse Y");
            MouseDelta *= -Time.deltaTime*MouseSpeed*(m_CameraComponent.orthographicSize/5);
            transform.position += new Vector3(MouseDelta.x,MouseDelta.y,0);
        }
        Vector2 Scroll = Input.mouseScrollDelta;
        m_CameraComponent.orthographicSize -= Scroll.y * ZoomSpeed;
        if(m_CameraComponent.orthographicSize < 1)
        {
            m_CameraComponent.orthographicSize = 1;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit HitInfo;
            bool Result = Physics.Raycast(m_CameraComponent.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, 0, 1000),out HitInfo,10000,(1<<LayerMask.NameToLayer("Clickable")));
            if(HitInfo.collider != null)
            {
                Clickable Clicker = HitInfo.collider.gameObject.GetComponent<Clickable>();
                if(Clicker != null)
                {
                    Clicker.OnClick();
                }
            }
        }
        //m_CameraComponent.setwi += Scroll.y * ZoomSpeed;
        //transform.position += new Vector3(0, 0, Scroll.y) * ZoomSpeed;
    }
}
