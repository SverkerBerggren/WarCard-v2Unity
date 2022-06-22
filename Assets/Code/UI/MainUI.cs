using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour, RuleManager.RuleEventHandler , ClickReciever
{

    RuleManager.RuleManager ruleManager;

    public GridManager gridManager; 

    public GameObject prefabToInstaniate; 

    public Sprite[] theSprites;

    private Dictionary<int, GameObject> listOfImages = new Dictionary<int, GameObject>();

    private int unitEtt;

    private int unitTva; 

  //  public GameObject test; 
    // Start is called before the first frame update
    void Start()
    {
        ruleManager = FindObjectOfType<TheRuleManager>().ruleManager;
        //    gridManager = FindObjectOfType<GridManager>();


        gridManager.SetInputReciever(this);

        RuleManager.UnitInfo forstaUnit = new RuleManager.UnitInfo();

        forstaUnit.Stats.HP = 10;

        forstaUnit.Position = new RuleManager.Coordinate(0, 0);

        UIInfo UIForsta = new UIInfo();

        UIForsta.WhichImage = theSprites[0];

        forstaUnit.OpaqueInteger = UIForsta;

        unitEtt = ruleManager.RegisterUnit(forstaUnit, 0);


        GameObject forstaUnitPaKartan =  Instantiate(prefabToInstaniate, gridManager.GetTilePosition(forstaUnit.Position), new Quaternion());
        listOfImages.Add(unitEtt, forstaUnitPaKartan);
        forstaUnitPaKartan.GetComponent<SpriteRenderer>().sprite = theSprites[0];

//        print("unit id forsta  " + forstaUnit.UnitID);

        //    test.transform.position = gridManager.GetTilePosition(forstaUnit.Position);


        RuleManager.UnitInfo andraUnit = new RuleManager.UnitInfo();

        andraUnit.Stats.HP = 5;

        andraUnit.Position = new RuleManager.Coordinate(2, 0);

      //  print("unit id andsra  " + andraUnit.UnitID);
        unitTva = ruleManager.RegisterUnit(andraUnit, 0);

        GameObject andraUnitPaKartan = Instantiate(prefabToInstaniate, gridManager.GetTilePosition(andraUnit.Position), new Quaternion());
        listOfImages.Add(unitTva, andraUnitPaKartan);
        andraUnitPaKartan.GetComponent<SpriteRenderer>().sprite = theSprites[1];

        
    


    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUnitMove(int UnitID, RuleManager.Coordinate PreviousPosition, RuleManager.Coordinate NewPosition)
    {
       // ruleManager.GetUnitInfo(UnitID).
        
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

    public void OnUnitCreate(RuleManager.UnitInfo NewUnit)
    {

    }

    public void OnClick(ClickType clickType, RuleManager.Coordinate cord)
    {   
        int unitId = ruleManager.GetTileInfo(cord.X, cord.Y).StandingUnitID;
        print(" vad blir ubnit id et " + unitId);
        if(unitId != 0)
        {

        }

        RuleManager.MoveAction hej = new RuleManager.MoveAction();

        hej.UnitID = unitId;
        RuleManager.Coordinate temp = ruleManager.GetUnitInfo(unitId).Position;
        temp.X += 1; 
        hej.NewPosition = temp;
        hej.PlayerIndex = 0; 
        

        ruleManager.ExecuteAction(hej);

        listOfImages[unitId].transform.position = gridManager.GetTilePosition(temp);


    }
}


public class UIInfo
{
    public Sprite WhichImage; 
}

