using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuleManager;
public class GridManager : MonoBehaviour
{

    public GameObject TileObject;

    //Temp
      //RuleManager.RuleManager m_RuleManager;
    // Dictionary<int, GameObject> m_VisibleUnits;


    public int Width = 20;
    public int Height = 20;
    private ClickReciever m_Reciever = null;

    private float m_TileWidth = 0;

    Vector3 p_GridToWorldPosition(int X, int Y)
    {
        Vector3 ReturnValue= new Vector3(0, 0, 0);
        ReturnValue.x = transform.position.x + m_TileWidth * X;
        ReturnValue.y = transform.position.y + m_TileWidth * Y;
        return (ReturnValue);
    }
    // Start is called before the first frame update
    void Start()
    {
        //m_RuleManager = FindObjectOfType<TheRuleManager>().ruleManager;  //new RuleManager.RuleManager( (uint)Width, (uint)Height);
        for(int YIndex = 0; YIndex < Height; YIndex++)
        {
            for(int XIndex = 0; XIndex < Width; XIndex++)
            {
                GameObject NewObject = Instantiate(TileObject);
                //Assumes that tiles are quadratic
                float TileWidth = NewObject.GetComponent<SpriteRenderer>().size.x;
                m_TileWidth = TileWidth;
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
   //     Destroy(m_VisibleUnits[UnitID]);
   //     m_VisibleUnits.Remove(UnitID);
    }
    public void OnTurnChange(int CurrentPlayerTurnIndex, int CurrentTurnCount)
    {

    }
    public void OnUnitCreate(RuleManager.UnitInfo NewUnit)
    {
        
    }

    public void OnClick(ClickType Type, int X, int Y)
    {
        print(Y+" "+X);
        if(m_Reciever != null)
        {
            m_Reciever.OnClick(Type, new Coordinate(X, Y));
        }
    }
    // Update is called once per frame
    void Update()
    {
      //  print("tilewidth update " + m_TileWidth);
    }

    public void SetInputReciever(ClickReciever clicker)
    {
        m_Reciever = clicker; 
    }

    public Vector3 GetTilePosition(Coordinate cord)
    {
       // print("tildedithe" + m_TileWidth);

      //  print(transform.position + new Vector3(16 * cord.X, 16 * -cord.Y));

        return transform.position + new Vector3(16 * cord.X, 16 * -cord.Y);
    }
}
public interface ClickReciever
{
    void OnClick(ClickType clickType, Coordinate cord);
    
}
