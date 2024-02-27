using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;




namespace ResourceManager
{
    public class Visual
    {
        public float XCenter = 0.5f;
        public float YCenter = 0.5f;
        public float Width = 10;
    }
    public class Visual_Image : Visual
    {
        public UnityEngine.Sprite Sprite;
    }
    public class Visual_Animation : Visual
    {
        public int FPS = 60;

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
        public Animation()
        {
               
        }
        public Animation(Visual VisualInfo)
        {
            this.VisualInfo = VisualInfo;
        }
    }
    public class UnitUIInfo
    {
        public Animation UpAnimation = new Animation();
        public Animation DownAnimation = new Animation();
        public Animation AttackAnimation = null;

        public AudioClip SelectSound = null;

        public Dictionary<int,Animation> AbilityIcons = new Dictionary<int, Animation>();
    }

    public class UnitResource
    {
        public string Name = "";
        public RuleManager.UnitInfo GameInfo = new RuleManager.UnitInfo();
        public UnitUIInfo UIInfo = new UnitUIInfo();
        //needed to support serialisation
        public Dictionary<int, RuleManager.Effect> TotalEffects = new();
        public Dictionary<int, RuleManager.TargetCondition> TotalTargetConditions = new();
        public int CurrentEffectID = 0;
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
        public Dictionary<string, UnitResource> m_LoadedUnitInfos = new Dictionary<string, UnitResource>();
        Dictionary<string, Visual> m_LoadedVisuals = new Dictionary<string, Visual>();
        UnitScript.UnitConverter m_ScriptHandler = new UnitScript.UnitConverter();
        Parser.Parser m_Parser = new Parser.Parser();
        MBCC.Tokenizer m_Tokenizer;
        bool m_VisualsLoaded = false;
        string m_ResourceFolder = "";

        public ResourceManager(string ResourceFolder)
        {
            m_ResourceFolder = ResourceFolder;
            m_Tokenizer = m_Parser.GetTokenizer();
            m_ScriptHandler.AddBuiltins(GetUnitScriptFuncs());
        }

        IEnumerable<UnitResource> p_UnitIterator()
        {
            foreach(var pair in m_LoadedUnitInfos)
            {
                yield return pair.Value;
            }
            yield break;
        }
        public IEnumerable<UnitResource> GetUnits()
        {
            return p_UnitIterator();
        }
        public UnitScript.UnitConverter GetScriptHandler()
        {
            return m_ScriptHandler;   
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
        //Visual_Animation p_ParseVisualAnimation(string UnitFilePath,MBJson.JSONObject VisualAnimation)
        //{
        //    Visual_Animation ReturnValue = new Visual_Animation();
        //    ReturnValue.FPS = VisualAnimation["FPS"].GetIntegerData();
        //
        //    string AnimationDir = p_GetRelativePath(UnitFilePath, VisualAnimation["Directory"].GetStringData());
        //    List<string> DirectionContent = new List<string>(UnitDirectoryIterator.GetDirectoryFiles_Recursive(AnimationDir));
        //    DirectionContent.Sort();
        //    foreach(string AnimationFile in DirectionContent)
        //    {
        //        if(AnimationFile.EndsWith(".meta"))
        //        {
        //            continue;
        //        }
        //        byte[] ImageData = File.ReadAllBytes(AnimationFile);
        //        UnityEngine.Texture2D NewTexture = new UnityEngine.Texture2D(2, 2);
        //        NewTexture.LoadImage(ImageData);
        //        UnityEngine.Sprite NewSprite = Sprite.Create(NewTexture,
        //            new Rect(0, 0, NewTexture.width, NewTexture.height), new Vector2(ReturnValue.XCenter, ReturnValue.YCenter), 100, 0, SpriteMeshType.FullRect);
        //        
        //        ReturnValue.AnimationContent.Add(NewSprite);
        //    }
        //    return (ReturnValue);
        //}

        static public Sprite SpriteFromTexture(Texture2D TextureToConvert)
        {
            return (Sprite.Create(TextureToConvert,
                    new Rect(0, 0, TextureToConvert.width, TextureToConvert.height), new Vector2(0.5f, 0), 100, 0, SpriteMeshType.FullRect));
        }
        //used only in the case of the JSON format
        //Visual p_ParseVisual(MBJson.JSONObject UIInfo,string UnitFilePath)
        //{
        //    Visual ReturnValue = null;
        //    if(UIInfo["VisualType"].GetStringData() == "Image")
        //    {
        //        Visual_Image NewImage = new Visual_Image();
        //        string SpritePath = p_GetRelativePath(UnitFilePath, UIInfo["File"].GetStringData());
        //        byte[] ImageData = File.ReadAllBytes(SpritePath);
        //        Texture2D Texture = new UnityEngine.Texture2D(2, 2);
        //        Texture.LoadImage(ImageData);
        //        NewImage.Sprite = Sprite.Create(Texture,
        //            new Rect(0, 0, Texture.width, Texture.height), new Vector2(0.5f, 0), 100, 0, SpriteMeshType.FullRect);
        //        ReturnValue = NewImage;
        //    }
        //    else if (UIInfo["VisualType"].GetStringData() == "Video")
        //    {
        //        Visual_Video NewVideo = new Visual_Video();
        //        string SpritePath = p_GetRelativePath(UnitFilePath, UIInfo["File"].GetStringData());
        //        NewVideo.VideoURL = SpritePath;
        //        ReturnValue = NewVideo;
        //    }
        //    else if(UIInfo["VisualType"].GetStringData() == "Animation")
        //    {
        //        ReturnValue = p_ParseVisualAnimation(UnitFilePath,UIInfo);
        //    }
        //    else
        //    {
        //        throw new System.Exception("Invalid visual type");
        //    }
        //    if (UIInfo.HasAttribute("XCenter"))
        //    {
        //        ReturnValue.XCenter = UIInfo["XCenter"].GetIntegerData()/ (float)100;
        //    }
        //    if (UIInfo.HasAttribute("YCenter"))
        //    {
        //        ReturnValue.YCenter = UIInfo["YCenter"].GetIntegerData()/ (float)100;
        //    }
        //    if(UIInfo.HasAttribute("Width"))
        //    {
        //        ReturnValue.Width = UIInfo["Width"].GetIntegerData();
        //    }
        //    return (ReturnValue);
        //}
        //Animation p_ParseAnimation(MBJson.JSONObject UIInfo, string UnitFilePath)
        //{
        //    Animation ReturnValue = new Animation();
        //    ReturnValue.VisualInfo = p_ParseVisual(UIInfo, UnitFilePath);
        //    return (ReturnValue);
        //}
        //private UnitResource p_ParseUnit(MBJson.JSONObject UnitInfo,string UnitFilePath)
        //{
        //    UnitResource ResourceToAdd = new UnitResource();
        //    //stats
        //    MBJson.JSONObject Stats = UnitInfo["Stats"];
        //    ResourceToAdd.Name = UnitInfo["Name"].GetStringData();
        //    ResourceToAdd.GameInfo.Stats.HP = Stats["HP"].GetIntegerData();
        //    ResourceToAdd.GameInfo.Stats.Damage = Stats["Damage"].GetIntegerData();
        //    ResourceToAdd.GameInfo.Stats.Range = Stats["Range"].GetIntegerData();
        //    ResourceToAdd.GameInfo.Stats.Movement = Stats["Movement"].GetIntegerData();
        //    ResourceToAdd.GameInfo.Stats.ObjectiveControll = Stats["ObjectiveControll"].GetIntegerData();
        //    //visual
        //    MBJson.JSONObject Visuals = UnitInfo["Visuals"];
        //    if(Visuals.HasAttribute("AttackVisual"))
        //    {
        //        ResourceToAdd.UIInfo.AttackAnimation = p_ParseAnimation(Visuals["AttackVisual"],UnitFilePath);
        //    }
        //    else if(Visuals.HasAttribute("UpVisual"))
        //    {
        //        ResourceToAdd.UIInfo.UpAnimation = p_ParseAnimation(Visuals["UpVisual"], UnitFilePath);
        //    }
        //    else if(Visuals.HasAttribute("DownVisual"))
        //    {
        //        ResourceToAdd.UIInfo.DownAnimation = p_ParseAnimation(Visuals["DownVisual"], UnitFilePath);
        //    }
        //
        //    foreach(var Key in Visuals.GetAggregateData())
        //    {
        //        //ResourceToAdd.UIInfo.OtherAnimations[Key.Key] = p_ParseAnimation(Key.Value,UnitFilePath);
        //    }
        //    return (ResourceToAdd);
        //}
        //used whether or not the json/*parser* format is used
        void LoadUnitFile(string PathToLoad)
        {
            //byte[] FileData = File.ReadAllBytes(PathToLoad);
            //int Out;
            //MBJson.JSONObject UnitInfo = MBJson.JSONObject.ParseJSONObject(FileData, 0, out Out);
            //UnitResource NewResource = p_ParseUnit(UnitInfo,PathToLoad);
            //m_LoadedUnitInfos[NewResource.Name] = NewResource;
            m_ScriptHandler.SetCurrentPath(PathToLoad);
            string Text = File.ReadAllText(PathToLoad);
            m_Tokenizer.SetText(Text);
            List<UnitScript.Diagnostic> Errors = new List<UnitScript.Diagnostic>();
            try
            {
                Parser.Unit ParsedUnit = m_Parser.ParseUnit(m_Tokenizer);
                UnitResource NewResource = m_ScriptHandler.ConvertUnit(Errors,ParsedUnit);
                m_LoadedUnitInfos[NewResource.Name] = NewResource;
            }
            catch(Exception e)
            {
                throw new Exception("Error parsing unit file: "+e.Message);
            }
            if(Errors.Count > 0)
            {
                throw new Exception("Parsed unit files had diagnostics errors");
            }
        }
        public void LoadResourceFolder(string PathToLoad)
        {
            if(m_VisualsLoaded)
            {
                return;   
            }
            foreach(string UnitPath in UnitDirectoryIterator.GetDirectoryFiles_Recursive(PathToLoad))
            {
                if(UnitPath.EndsWith(".unit"))
                {
                    LoadUnitFile(UnitPath);
                }
            }
            m_VisualsLoaded = true;
        }

        void p_UpdateVisualKeyParameter(Visual VisualToModify,UnitScript.BuiltinFuncArgs Args)
        {
            if (Args.KeyArguments.ContainsKey("XCenter"))
            {
                VisualToModify.XCenter = ((int)Args.KeyArguments["XCenter"]) / (float)100;
            }
            if (Args.KeyArguments.ContainsKey("YCenter"))
            {
                VisualToModify.YCenter = ((int)Args.KeyArguments["YCenter"])/ (float)100;
            }
            if(Args.KeyArguments.ContainsKey("Width"))
            {
                VisualToModify.Width = ((int)Args.KeyArguments["Width"]);
            }
        }
        
        object p_Image(UnitScript.BuiltinFuncArgs Args)
        {
            Visual ReturnValue = null;
            string ImageToLoad = (string)Args.Arguments[0];
            Visual_Image NewImage = new Visual_Image();
            string SpritePath = p_GetRelativePath(Args.Handler.GetCurrentPath(), ImageToLoad);
            byte[] ImageData = File.ReadAllBytes(SpritePath);
            Texture2D Texture = new UnityEngine.Texture2D(2, 2);
            Texture.LoadImage(ImageData);
            Texture.filterMode = FilterMode.Point;
            NewImage.Sprite = Sprite.Create(Texture,
                new Rect(0, 0, Texture.width, Texture.height), new Vector2(0.5f, 0), 100, 0, SpriteMeshType.FullRect);
            ReturnValue = NewImage;
            p_UpdateVisualKeyParameter(ReturnValue,Args);
            return new Animation(ReturnValue);
        }
        object p_Video(UnitScript.BuiltinFuncArgs Args)
        {
            Visual_Video NewVideo = new Visual_Video();
            string VideoToLoad = (string)Args.Arguments[0];
            string SpritePath = p_GetRelativePath(Args.Handler.GetCurrentPath(), VideoToLoad);
            NewVideo.VideoURL = SpritePath;
            p_UpdateVisualKeyParameter(NewVideo,Args);
            return new Animation(NewVideo);
        }
        object p_Animation(UnitScript.BuiltinFuncArgs Args)
        {
            Visual_Animation ReturnValue = new Visual_Animation();
            if(Args.KeyArguments.ContainsKey("FPS"))
            {
                ReturnValue.FPS = (int)Args.KeyArguments["FPS"];
            }
            string DirPath = (string)Args.Arguments[0];
            string AnimationDir = p_GetRelativePath(Args.Handler.GetCurrentPath(), DirPath);
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
            p_UpdateVisualKeyParameter(ReturnValue,Args);
            return new Animation(ReturnValue);
        }
        Dictionary<string,UnitScript.Builtin_FuncInfo> GetUnitScriptFuncs()
        {
            Dictionary<string,UnitScript.Builtin_FuncInfo> ReturnValue = new Dictionary<string, UnitScript.Builtin_FuncInfo>();
            Dictionary<string,Type> CommonKeyArgTypes = new Dictionary<string,Type>{ 
                {"XCenter",typeof(int)},
                {"YCenter",typeof(int)},
                {"Width",typeof(int)},
                {"FPS",typeof(int)},
            };
            
            UnitScript.Builtin_FuncInfo Image = new UnitScript.Builtin_FuncInfo();
            Image.ArgTypes = new List<HashSet<Type>>{new HashSet<Type>{typeof(string)}};
            Image.ResultType = typeof(Animation);
            Image.ValidContexts = UnitScript.EvalContext.Compile;
            Image.Callable = p_Image;
            Image.KeyArgTypes = CommonKeyArgTypes;
            ReturnValue["Image"] = Image;

            UnitScript.Builtin_FuncInfo Video = new UnitScript.Builtin_FuncInfo();
            Video.ArgTypes = new List<HashSet<Type>>{new HashSet<Type>{typeof(string)}};
            Video.ResultType = typeof(Animation);
            Video.ValidContexts = UnitScript.EvalContext.Compile;
            Video.Callable = p_Video;
            Video.KeyArgTypes = CommonKeyArgTypes;
            ReturnValue["Video"] = Video;

            UnitScript.Builtin_FuncInfo Animation = new UnitScript.Builtin_FuncInfo();
            Animation.ArgTypes = new List<HashSet<Type>>{new HashSet<Type>{typeof(string)}};
            Animation.ResultType = typeof(Animation);
            Animation.ValidContexts = UnitScript.EvalContext.Compile;
            Animation.Callable = p_Animation;
            Animation.KeyArgTypes = CommonKeyArgTypes;
            ReturnValue["Animation"] = Animation;

            return ReturnValue;
        }
    }

}
