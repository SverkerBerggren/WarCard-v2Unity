using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryScript : Unit
{
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override Sprite GetUnitSprite()
    {
        return sprite;
    }
    public override RuleManager.UnitInfo CreateUnitInfo()
    {
        return Militarium.GetArtillery();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
