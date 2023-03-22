using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestScript : Unit
{
    public Sprite sidewaySprite;
    public Sprite forwardSprite;
    public Sprite backWardSprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override ResourceManager.UnitResource CreateUnitInfo()
    {
        ResourceManager.UnitResource ReturnValue = new ResourceManager.UnitResource();
        ReturnValue.Name = "Priest";
        ReturnValue.GameInfo = Templars.GetPriest();
        ReturnValue.UIInfo = new ResourceManager.UnitUIInfo();

        ReturnValue.UIInfo.UpAnimation = new ResourceManager.Animation();
        ReturnValue.UIInfo.UpAnimation.VisualInfo = new ResourceManager.Visual_Image();
        ((ResourceManager.Visual_Image)ReturnValue.UIInfo.UpAnimation.VisualInfo).Sprite = backWardSprite;

        ReturnValue.UIInfo.DownAnimation = new ResourceManager.Animation();
        ReturnValue.UIInfo.DownAnimation.VisualInfo = new ResourceManager.Visual_Image();
        ((ResourceManager.Visual_Image)ReturnValue.UIInfo.DownAnimation.VisualInfo).Sprite = forwardSprite;

        return (ReturnValue);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
