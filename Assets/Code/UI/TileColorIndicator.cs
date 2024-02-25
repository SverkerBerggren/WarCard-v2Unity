using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.TerrainTools;
using UnityEngine;

public class TileColorIndicator : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    public void PaintTile(TileColoringCondition tileColoringConditon)
    {
        spriteRenderer.color = tileColoringConditon.color;
    }

}
