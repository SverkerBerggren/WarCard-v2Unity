using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColoringEffect
{
    public Color color;
    public RuleManager.EffectSource effectSource;
    public List<RuleManager.Coordinate> coordinateList;
    public TileColoringEffect()
    {

    }
    public TileColoringEffect(Color color, RuleManager.EffectSource effectSource, List<RuleManager.Coordinate> coordinates )
    {
        this.color = color;
        this.effectSource = effectSource;
        coordinateList = coordinates;
    }
}
