using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMouse : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 Hotspot = Vector2.zero;
    public Texture2D MouseTexture = null;
    void Start()
    {
        Cursor.SetCursor(MouseTexture, Hotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
