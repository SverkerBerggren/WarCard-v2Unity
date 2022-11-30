using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuleManager;
public class GridManager : MonoBehaviour
{

    public GameObject TileObject = null;
    public GameObject GrassObject = null;

    //Temp
      //RuleManager.RuleManager m_RuleManager;
    // Dictionary<int, GameObject> m_VisibleUnits;


    public int Width = 20;
    public int Height = 20;
    public float Angle = 45;
    public float Tilt = 30;
    private ClickReciever m_Reciever = null;

    public float TileWidth = 1;
    public float GrassMultiplier = 4;

    //Vector3 p_GridToWorldPosition(int X, int Y)
    //{
    //    Vector3 ReturnValue= new Vector3(0, 0, 0);
    //    ReturnValue.x = transform.position.x + m_TileWidth * X;
    //    ReturnValue.y = transform.position.y + m_TileWidth * Y;
    //    return (ReturnValue);
    //}
    // Start is called before the first frame update
    void Awake
        ()
    {
        Angle *= -1;
        //m_RuleManager = FindObjectOfType<TheRuleManager>().ruleManager;  //new RuleManager.RuleManager( (uint)Width, (uint)Height);
        Vector3 YDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad* (Angle+90)),Mathf.Sin(Mathf.Deg2Rad*(Angle+90)));
        Vector3 XDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad*Angle),Mathf.Sin(Mathf.Deg2Rad*Angle));
        for (int YIndex = 0; YIndex < Height; YIndex++)
        {
            for(int XIndex = 0; XIndex < Width; XIndex++)
            {
                GameObject NewObject = Instantiate(TileObject);
                NewObject.transform.eulerAngles = GetEulerAngle();
                float ObjectWidth = NewObject.GetComponent<BoxCollider>().size.x;
              
                NewObject.transform.localScale *= TileWidth / ObjectWidth;

                Vector3 NewPosition = new Vector3(transform.position.x, transform.position.y, 0);
                NewPosition += XIndex * TileWidth * XDirection;
                NewPosition += YIndex * TileWidth * YDirection*Mathf.Cos(Mathf.Deg2Rad * Tilt);
                GridClick ClickObject = NewObject.GetComponent<GridClick>();
                ClickObject.X = XIndex;
                ClickObject.Y = YIndex;
                ClickObject.AssociatedGrid = this;
                NewObject.transform.position = NewPosition;
            }
        }
        //float XOffset = TileWidth;
        //float YOffset = TileWidth;
        Vector3 Offset = new Vector3(0,0,0);
        Offset += YDirection * TileWidth;
        Offset += XDirection * TileWidth;
        //float XDiff = -(Width % GrassMultiplier)*TileWidth / 2;
        //float YDiff = -(Height % GrassMultiplier) *TileWidth/2;
        //XOffset += XDiff;
        //YOffset += YDiff;
        for(int YIndex = 0; YIndex < Mathf.Ceil(Height / GrassMultiplier); YIndex++)
        {
            for (int XIndex = 0; XIndex < Mathf.Ceil(Width/GrassMultiplier); XIndex++)
            {
                GameObject NewObject = Instantiate(GrassObject);
                NewObject.transform.eulerAngles = GetEulerAngle();
                float ObjectWidth = NewObject.GetComponent<BoxCollider>().size.x;
                NewObject.transform.localScale *= TileWidth*GrassMultiplier/ObjectWidth;
                //Vector3 NewPosition = new Vector3(XOffset+ transform.position.x + XIndex * TileWidth*GrassMultiplier,-YOffset+ transform.position.y - YIndex * TileWidth*GrassMultiplier, 0);
                Vector3 NewPosition = new Vector3(transform.position.x,transform.position.y, 0);
                NewPosition += XIndex * TileWidth * GrassMultiplier * XDirection;
                NewPosition += YIndex * TileWidth * GrassMultiplier * YDirection * Mathf.Cos(Mathf.Deg2Rad * Tilt);
                NewPosition += Offset;
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
     //   print(Y+" "+X);
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
        Vector3 YDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (Angle + 90)), Mathf.Sin(Mathf.Deg2Rad * (Angle + 90)));
        Vector3 XDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * Angle), Mathf.Sin(Mathf.Deg2Rad * Angle));
        Vector3 ReturnValue = transform.position;
        ReturnValue += cord.X * TileWidth * XDirection;
        ReturnValue += cord.Y * TileWidth * YDirection * Mathf.Cos(Mathf.Deg2Rad* Tilt);
        //return transform.position + new Vector3(TileWidth * cord.X, TileWidth * -cord.Y);
        return (ReturnValue);
    }
    public Vector3 GetEulerAngle()
    {
        return (new Vector3(Tilt,0,Angle));
    }
}
public interface ClickReciever
{
    void OnClick(ClickType clickType, Coordinate cord);
    
}
