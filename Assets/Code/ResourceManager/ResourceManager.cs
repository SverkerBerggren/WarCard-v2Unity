using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;




namespace ResourceManager
{
    public class Visual
    {

    }
    public class Visual_Image : Visual
    {
        public UnityEngine.Texture2D Sprite;
    }
    public class Visual_Video : Visual
    {
        public string VideoURL;
    }
    public class Animation
    {
        public Visual VisualInfo = null;
        public float SecLengh = 0;
    }
    public class UnitUIInfo
    {
        public Animation UpAnimation;
        public Animation DownAnimation;
        public Animation AttackAnimation;
    }

    public class UnitResource
    {
        public string Name = "";
        public RuleManager.UnitInfo GameInfo;
        public UnitUIInfo UIInfo;
    }

    class UnitDirectoryIterator
    {
        public static IEnumerable<string> GetDirectoryFiles_Recursive(string DirectoryPath)
        {
            Stack<string> DirsToIterate = new Stack<string>();
            DirsToIterate.Push(DirectoryPath);
            while(DirsToIterate.Count > 0)
            {
                string CurrentDir = DirsToIterate.Pop();
                foreach(string FilePath in Directory.EnumerateFiles(CurrentDir,"*.unit"))
                {
                    yield return FilePath;
                }
                foreach(string Directory in Directory.EnumerateDirectories(CurrentDir))
                {
                    DirsToIterate.Push(Directory);
                }
            }
            yield break;
        }
    }
    
    public class ResourceManager
    {
        Dictionary<string, UnitResource> m_LoadedUnitInfos = new Dictionary<string, UnitResource>();
        Dictionary<string, Visual> m_LoadedVisuals = new Dictionary<string, Visual>();


        UnitResource GetUnitResource(string NameOfUnit)
        {
            if(m_LoadedUnitInfos.ContainsKey(NameOfUnit))
            {
                return (m_LoadedUnitInfos[NameOfUnit]);
            }
            throw new System.Exception("No unit with name " + NameOfUnit + " is loaded");
        }

        static string p_GetRelativePath(string FilePath, string RelativePath)
        {
            string ReturnValue = "";
            ReturnValue = Path.GetDirectoryName(FilePath) + RelativePath;
            return (ReturnValue);
        }
        //used only in the case of the JSON format
        Visual p_ParseVisual(MBJson.JSONObject UIInfo,string UnitFilePath)
        {
            Visual ReturnValue = null;
            if(UIInfo["VisualType"].GetStringData() == "Image")
            {
                Visual_Image NewImage = new Visual_Image();
                string SpritePath = p_GetRelativePath(UnitFilePath, UIInfo["File"].GetStringData());
                byte[] ImageData = File.ReadAllBytes(SpritePath);
                NewImage.Sprite = new UnityEngine.Texture2D(2,2);
                NewImage.Sprite.LoadImage(ImageData);
                ReturnValue = NewImage;
            }
            else if (UIInfo["VisualType"].GetStringData() == "Video")
            {
                Visual_Video NewVideo = new Visual_Video();
                string SpritePath = p_GetRelativePath(UnitFilePath, UIInfo["File"].GetStringData());
                NewVideo.VideoURL = SpritePath;
                ReturnValue = NewVideo;
            }
            else
            {
                throw new System.Exception("Invalid visual type");
            }
            return (ReturnValue);
        }
        Animation p_ParseAnimation(MBJson.JSONObject UIInfo, string UnitFilePath)
        {
            Animation ReturnValue = new Animation();
            ReturnValue.VisualInfo = p_ParseVisual(UIInfo, UnitFilePath);
            return (ReturnValue);
        }
        private void p_ParseUnit(MBJson.JSONObject UnitInfo,string UnitFilePath)
        {
            UnitResource ResourceToAdd = new UnitResource();
            //stats
            MBJson.JSONObject Stats = UnitInfo["Stats"];
            ResourceToAdd.GameInfo.Stats.HP = Stats["HP"].GetIntegerData();
            ResourceToAdd.GameInfo.Stats.Damage = Stats["Damage"].GetIntegerData();
            ResourceToAdd.GameInfo.Stats.Range = Stats["Range"].GetIntegerData();
            ResourceToAdd.GameInfo.Stats.Movement = Stats["Range"].GetIntegerData();
            ResourceToAdd.GameInfo.Stats.ObjectiveControll = Stats["ObjectiveControll"].GetIntegerData();
            //visual
            MBJson.JSONObject Visuals = UnitInfo["Visuals"];
            ResourceToAdd.UIInfo.AttackAnimation = p_ParseAnimation(Visuals["AttackVisual"],UnitFilePath);
            ResourceToAdd.UIInfo.UpAnimation = p_ParseAnimation(Visuals["UpVisual"], UnitFilePath);
            ResourceToAdd.UIInfo.DownAnimation = p_ParseAnimation(Visuals["DownVisual"], UnitFilePath);
        }
        //used whether or not the json/*parser* format is used
        void LoadUnitFile(string PathToLoad)
        {
            byte[] FileData = File.ReadAllBytes(PathToLoad);
            int Out;
            MBJson.JSONObject UnitInfo = MBJson.JSONObject.ParseJSONObject(FileData, 0, out Out);
            p_ParseUnit(UnitInfo,PathToLoad);
        }
        void LoadResourceFolder(string PathToLoad)
        {
            foreach(string UnitPath in UnitDirectoryIterator.GetDirectoryFiles_Recursive(PathToLoad))
            {

            }
        }
    }

}