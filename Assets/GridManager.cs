using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuleManager;
public class GridManager : MonoBehaviour,RuleManager.RuleEventHandler
{

    public GameObject TileObject;

    //Temp
      RuleManager.RuleManager m_RuleManager;
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
        UnitStats Stats = new UnitStats();
        Stats.ActivationCost = 0;
        Stats.Damage = 0;
        Stats.HP = 5;
        Stats.Damage = 3;
        Stats.Movement = 3;

        for(int i = 0; i < 3; i++)
        {
            UnitInfo NewUnit = new UnitInfo();
            NewUnit.Stats = Stats;
            NewUnit.Position = new Coordinate(0, i);
            //m_RuleManager.RegisterUnit(NewUnit, 0);
            NewUnit.Position = new Coordinate(5, i);
            //m_RuleManager.RegisterUnit(NewUnit, 1);
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
        
    }

    public void SetInputReciever(ClickReciever clicker)
    {

    }

    
}
public interface ClickReciever
{
    void OnClick(ClickType clickType, Coordinate cord);
    
}