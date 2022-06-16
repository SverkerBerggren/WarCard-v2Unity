using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuleManager;
public class GridManager : MonoBehaviour,RuleManager.RuleEventHandler
{

    public GameObject TileObject;

    //Temp
    RuleManager.RuleManager m_RuleManager;
    Dictionary<int, GameObject> m_VisibleUnits;


    public int Width = 20;
    public int Height = 20;


    void p_RegisterUnit(RuleManager.UnitInfo NewUnit, int PlayerIndex)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        m_RuleManager = new RuleManager.RuleManager( (uint)Width, (uint)Height);
        for(int YIndex = 0; YIndex < Height; YIndex++)
        {
            for(int XIndex = 0; XIndex < Width; XIndex++)
            {
                GameObject NewObject = Instantiate(TileObject);
                //Assumes that tiles are quadratic
                float TileWidth = NewObject.GetComponent<SpriteRenderer>().size.x;
                Vector3 NewPosition = new Vector3(transform.position.x + XIndex * TileWidth, transform.position.y - YIndex * TileWidth, 0);
                GridClick ClickObject = NewObject.GetComponent<GridClick>();
                ClickObject.X = XIndex;
                ClickObject.Y = YIndex;
                ClickObject.AssociatedGrid = this;
                NewObject.transform.position = NewPosition;
            }
        }
    }

    public void OnUnitMove(int UnitID, Coordinate PreviousPosition, Coordinate NewPosition)
    {

    }
    public void OnUnitAttack(int AttackerID, int DefenderID)
    {

    }
    public void OnUnitDestroyed(int UnitID)
    {

    }
    public void OnTurnChange(int CurrentPlayerTurnIndex, int CurrentTurnCount)
    {

    }

    public void OnClick(int X, int Y)
    {
        print(Y+" "+X);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
