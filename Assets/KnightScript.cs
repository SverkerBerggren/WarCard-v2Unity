using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class KnightScript : Unit
{
    public VideoClip AttackAnimation;
    public Sprite forwardSprite;
    public Sprite backWardSprite;
    // Start is called before the first frame update
    void Start()
    {

    }


    public override ResourceManager.UnitResource CreateUnitInfo()
    {
        ResourceManager.UnitResource ReturnValue = new ResourceManager.UnitResource();
        ReturnValue.Name = "Knight";
        ReturnValue.GameInfo = Templars.GetHorseKnight();
        ReturnValue.UIInfo = new ResourceManager.UnitUIInfo();

        ReturnValue.UIInfo.UpAnimation = new ResourceManager.Animation();
        ReturnValue.UIInfo.UpAnimation.VisualInfo = new ResourceManager.Visual_Image();
        ((ResourceManager.Visual_Image)ReturnValue.UIInfo.UpAnimation.VisualInfo).Sprite = backWardSprite;

        ReturnValue.UIInfo.DownAnimation = new ResourceManager.Animation();
        ReturnValue.UIInfo.DownAnimation.VisualInfo = new ResourceManager.Visual_Image();
        ((ResourceManager.Visual_Image)ReturnValue.UIInfo.DownAnimation.VisualInfo).Sprite = forwardSprite;

        ReturnValue.UIInfo.AttackAnimation = new ResourceManager.Animation();
        ReturnValue.UIInfo.AttackAnimation.VisualInfo = MainUI.g_ResourceManager.GetUnitResource("Knight").UIInfo.AttackAnimation.VisualInfo;

        ReturnValue.UIInfo.UpAnimation.VisualInfo.Width = 20;
        ReturnValue.UIInfo.DownAnimation.VisualInfo.Width = 20;
        ReturnValue.UIInfo.UpAnimation.VisualInfo.YCenter = 2100 / 7195f;
        ReturnValue.UIInfo.UpAnimation.VisualInfo.XCenter = 0.45f;
        ReturnValue.UIInfo.DownAnimation.VisualInfo.YCenter = 2100 / 7195f;
        ReturnValue.UIInfo.DownAnimation.VisualInfo.XCenter = 0.45f;
        //ReturnValue.UIInfo.AttackAnimation.VisualInfo = new ResourceManager.Visual_Video();
        //((ResourceManager.Visual_Video)ReturnValue.UIInfo.AttackAnimation.VisualInfo).Clip = AttackAnimation;

        return (ReturnValue);
    }

    // Update is called once per frame
    void Update()
    {

    }   
}
