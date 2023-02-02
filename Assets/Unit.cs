using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    // Start is called before the first frame update
    public Sprite unitCardSprite;
    public List<Sprite> AbilityIcons = new List<Sprite>();
    public AudioClip SelectSound = null;
    void Start()
    {

    }

    public virtual ResourceManager.UnitResource CreateUnitInfo()
    {
        return null;
    }
     

    // Update is called once per frame
    void Update()
    {
        
    }
}
