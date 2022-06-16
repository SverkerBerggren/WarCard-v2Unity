using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public GameObject TileObject;

    public int Width = 20;
    public int Height = 20;

    // Start is called before the first frame update
    void Start()
    {
        for(int YIndex = 0; YIndex < Height;YIndex++)
        {
            for(int XIndex = 0; XIndex < Width; XIndex++)
            {
                GameObject NewObject = Instantiate(TileObject);
                //Assumes that tiles are quadratic
                float TileWidth = NewObject.GetComponent<SpriteRenderer>().size.x;
                Vector3 NewPosition = new Vector3(transform.position.x + XIndex * TileWidth, transform.position.y + YIndex * TileWidth, 0);
                GridClick ClickObject = NewObject.GetComponent<GridClick>();
                ClickObject.X = XIndex;
                ClickObject.Y = YIndex;
                ClickObject.AssociatedGrid = this;
                NewObject.transform.position = NewPosition;
            }
        }
    }
    public void OnClick(int X, int Y)
    {
        print(X+" "+Y.ToString());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
