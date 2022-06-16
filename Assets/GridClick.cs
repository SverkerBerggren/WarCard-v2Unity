using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridClick : Clickable
{
    // Start is called before the first frame update
    public GridManager AssociatedGrid;
    public int X = 0;
    public int Y = 0;
    override public void OnClick() 
    {
        AssociatedGrid.OnClick(X, Y);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
