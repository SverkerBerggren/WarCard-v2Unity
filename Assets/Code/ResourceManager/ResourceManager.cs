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
    public class Visual_Animation : Visual
    {
        public int FPS = 0;
        public float XCenter = 0.5f;
        public float YCenter = 0.5f;
        public List<UnityEngine.Sprite> AnimationContent = new List<Sprite>();
    }
    public class Visual_Video : Visual
    {
        public string VideoURL = null;
        public UnityEngine.Video.VideoClip Clip = null;
    }
    public class Animation
    {
        public Visual VisualInfo = null;
        public float SecLengh = 0;
    }
    public class UnitUIInfo
    {
        public Animation UpAnimation = new Animation();
        public Animation DownAnimation = new Animation();
        public Animation AttackAnimation = new Animation();
    }

    public class UnitResource
    {
        public string Name = "";
        public RuleManager.UnitInfo GameInfo = new RuleManager.UnitInfo();
        public UnitUIInfo UIInfo = new UnitUIInfo();
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
                foreach(string FilePath in Directory.EnumerateFiles(CurrentDir))
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
        bool m_VisualsLoaded = false;
        string m_ResourceFolder = "";

        public ResourceManager(string ResourceFolder)
        {
            m_ResourceFolder = ResourceFolder;
        }
        public UnitResource GetUnitResource(string NameOfUnit)
        {
            LoadResourceFolder(m_ResourceFolder);
            if(m_LoadedUnitInfos.ContainsKey(NameOfUnit))
            {
                return (m_LoadedUnitInfos[NameOfUnit]);
            }
            throw new System.Exception("No unit with name " + NameOfUnit + " is loaded");
        }
        static string p_GetRelativePath(string FilePath, string RelativePath)
        {
            string ReturnValue = "";
            ReturnValue = Path.GetDirectoryName(FilePath) +"/"+ RelativePath;
            return (ReturnValue);
        }
        Visual_Animation p_ParseVisualAnimation(string UnitFilePath,MBJson.JSONObject VisualAnimation)
        {
            Visual_Animation ReturnValue = new Visual_Animation();
            ReturnValue.FPS = VisualAnimation["FPS"].GetIntegerData();
            ReturnValue.XCenter = VisualAnimation["XCenter"].GetIntegerData()/100f;
            ReturnValue.YCenter = VisualAnimation["YCenter"].GetIntegerData()/100f;
            string AnimationDir = p_GetRelativePath(UnitFilePath, VisualAnimation["Directory"].GetStringData());
            List<string> DirectionContent = new List<string>(UnitDirectoryIterator.GetDirectoryFiles_Recursive(AnimationDir));
            DirectionContent.Sort();
            foreach(string AnimationFile in DirectionContent)
            {
                if(AnimationFile.EndsWith(".meta"))
                {
                    continue;
                }
                byte[] ImageData = File.ReadAllBytes(AnimationFile);
                UnityEngine.Texture2D NewTexture = new UnityEngine.Texture2D(2, 2);
                NewTexture.LoadImage(ImageData);
                UnityEngine.Sprite NewSprite = Sprite.Create(NewTexture,
                    new Rect(0, 0, NewTexture.width, NewTexture.height), new Vector2(ReturnValue.XCenter, ReturnValue.YCenter), 100, 0, SpriteMeshType.FullRect);
                ReturnValue.AnimationContent.Add(NewSprite);
            }
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
            else if(UIInfo["VisualType"].GetStringData() == "Animation")
            {
                ReturnValue = p_ParseVisualAnimation(UnitFilePath,UIInfo);
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
        private UnitResource p_ParseUnit(MBJson.JSONObject UnitInfo,string UnitFilePath)
        {
            UnitResource ResourceToAdd = new UnitResource();
            //stats
            MBJson.JSONObject Stats = UnitInfo["Stats"];
            ResourceToAdd.Name = UnitInfo["Name"].GetStringData();
            ResourceToAdd.GameInfo.Stats.HP = Stats["HP"].GetIntegerData();
            ResourceToAdd.GameInfo.Stats.Damage = Stats["Damage"].GetIntegerData();
            ResourceToAdd.GameInfo.Stats.Range = Stats["Range"].GetIntegerData();
            ResourceToAdd.GameInfo.Stats.Movement = Stats["Movement"].GetIntegerData();
            ResourceToAdd.GameInfo.Stats.ObjectiveControll = Stats["ObjectiveControll"].GetIntegerData();
            //visual
            MBJson.JSONObject Visuals = UnitInfo["Visuals"];
            if(Visuals.HasAttribute("AttackVisual"))
            {
                ResourceToAdd.UIInfo.AttackAnimation = p_ParseAnimation(Visuals["AttackVisual"],UnitFilePath);
            }
            else if(Visuals.HasAttribute("UpVisual"))
            {
                ResourceToAdd.UIInfo.UpAnimation = p_ParseAnimation(Visuals["UpVisual"], UnitFilePath);
            }
            else if(Visuals.HasAttribute("DownVisual"))
            {
                ResourceToAdd.UIInfo.DownAnimation = p_ParseAnimation(Visuals["DownVisual"], UnitFilePath);
            }
            return (ResourceToAdd);
        }
        //used whether or not the json/*parser* format is used
        void LoadUnitFile(string PathToLoad)
        {
            byte[] FileData = File.ReadAllBytes(PathToLoad);
            int Out;
            MBJson.JSONObject UnitInfo = MBJson.JSONObject.ParseJSONObject(FileData, 0, out Out);
            UnitResource NewResource = p_ParseUnit(UnitInfo,PathToLoad);
            m_LoadedUnitInfos[NewResource.Name] = NewResource;
        }
        void LoadResourceFolder(string PathToLoad)
        {
            m_VisualsLoaded = true;
            foreach(string UnitPath in UnitDirectoryIterator.GetDirectoryFiles_Recursive(PathToLoad))
            {
                if(UnitPath.EndsWith(".json"))
                {
                    LoadUnitFile(UnitPath);
                }
            }
        }
    }

}