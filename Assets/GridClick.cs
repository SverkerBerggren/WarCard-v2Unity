using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridClick : Clickable, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public GridManager AssociatedGrid;
    public int X = 0;
    public int Y = 0;
    override public void OnClick(ClickType Type) 
    {
        AssociatedGrid.OnClick(Type,X, Y);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AssociatedGrid.OnHover(X, Y);
       print("enter " + X + " " + Y); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AssociatedGrid.OnHoverExit(X, Y);
       print("exit " + X + " " + Y);

    }

    private void OnMouseEnter()
    {
      //  print("enter " + X + " " + Y);

    }

    private void OnMouseExit()
    {
      //  print("exit " + X + " " + Y);
    }
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
