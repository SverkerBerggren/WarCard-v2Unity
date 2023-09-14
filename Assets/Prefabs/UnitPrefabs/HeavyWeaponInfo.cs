using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeaponInfo : Unit
{
    public Sprite backwardSprite = null;
    public Sprite forwardSprite = null;
    public Sprite sideSprite = null;

    public override ResourceManager.UnitResource CreateUnitInfo()
    {
        return MainUI.g_ResourceManager.GetUnitResource("HeavyWeapon");
        ResourceManager.UnitResource ReturnValue = new ResourceManager.UnitResource();
        ReturnValue.GameInfo = Militarium.GetHeavyWeapons();
        ReturnValue.UIInfo = new ResourceManager.UnitUIInfo();

        ReturnValue.UIInfo.UpAnimation = new ResourceManager.Animation();
        ReturnValue.UIInfo.UpAnimation.VisualInfo = new ResourceManager.Visual_Image();
        ((ResourceManager.Visual_Image)ReturnValue.UIInfo.UpAnimation.VisualInfo).Sprite = backwardSprite;

        ReturnValue.UIInfo.DownAnimation = new ResourceManager.Animation();
        ReturnValue.UIInfo.DownAnimation.VisualInfo = new ResourceManager.Visual_Image();
        ((ResourceManager.Visual_Image)ReturnValue.UIInfo.DownAnimation.VisualInfo).Sprite = forwardSprite;

        return (ReturnValue);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
