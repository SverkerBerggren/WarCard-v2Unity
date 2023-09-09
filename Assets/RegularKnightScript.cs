using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class RegularKnightScript : Unit
{
    public Sprite forwardSprite;
    public Sprite backWardSprite;


    public override ResourceManager.UnitResource CreateUnitInfo()
    {
        ResourceManager.UnitResource ReturnValue = MainUI.g_ResourceManager.GetUnitResource("Knight");

        //ReturnValue.UIInfo.UpAnimation = new ResourceManager.Animation();
        //ReturnValue.UIInfo.UpAnimation.VisualInfo = new ResourceManager.Visual_Image();
        //((ResourceManager.Visual_Image)ReturnValue.UIInfo.UpAnimation.VisualInfo).Sprite = backWardSprite;
        //
        //ReturnValue.UIInfo.DownAnimation = new ResourceManager.Animation();
        //ReturnValue.UIInfo.DownAnimation.VisualInfo = new ResourceManager.Visual_Image();
        //((ResourceManager.Visual_Image)ReturnValue.UIInfo.DownAnimation.VisualInfo).Sprite = forwardSprite;



        //ReturnValue.UIInfo.AttackAnimation.VisualInfo = new ResourceManager.Visual_Video();
        //((ResourceManager.Visual_Video)ReturnValue.UIInfo.AttackAnimation.VisualInfo).Clip = AttackAnimation;

        return (ReturnValue);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
