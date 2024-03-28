using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;


namespace ResourceManager
{
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

    public class ResourceManagerImpl
    {
        static void p_UpdateVisualKeyParameter(Visual VisualToModify, UnitScript.BuiltinFuncArgs Args)
        {
            if (Args.KeyArguments.ContainsKey("XCenter"))
            {
                VisualToModify.XCenter = ((int)Args.KeyArguments["XCenter"]) / (float)100;
            }
            if (Args.KeyArguments.ContainsKey("YCenter"))
            {
                VisualToModify.YCenter = ((int)Args.KeyArguments["YCenter"]) / (float)100;
            }
            if (Args.KeyArguments.ContainsKey("Width"))
            {
                VisualToModify.Width = ((int)Args.KeyArguments["Width"]);
            }
        }
        static Sprite SpriteFromTexture(Texture2D TextureToConvert)
        {
            return (Sprite.Create(TextureToConvert,
                    new Rect(0, 0, TextureToConvert.width, TextureToConvert.height), new Vector2(0.5f, 0), 100, 0, SpriteMeshType.FullRect));
        }
        static string p_GetRelativePath(string FilePath, string RelativePath)
        {
            string ReturnValue = "";
            ReturnValue = Path.GetDirectoryName(FilePath) + "/" + RelativePath;
            return (ReturnValue);
        }
        static object p_Image(UnitScript.BuiltinFuncArgs Args)
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
            p_UpdateVisualKeyParameter(ReturnValue, Args);
            return new Animation(ReturnValue);
        }
        static object p_Video(UnitScript.BuiltinFuncArgs Args)
        {
            Visual_Video NewVideo = new Visual_Video();
            string VideoToLoad = (string)Args.Arguments[0];
            string SpritePath = p_GetRelativePath(Args.Handler.GetCurrentPath(), VideoToLoad);
            NewVideo.VideoURL = SpritePath;
            p_UpdateVisualKeyParameter(NewVideo, Args);
            return new Animation(NewVideo);
        }
        static object p_Animation(UnitScript.BuiltinFuncArgs Args)
        {
            Visual_Animation ReturnValue = new Visual_Animation();
            if (Args.KeyArguments.ContainsKey("FPS"))
            {
                ReturnValue.FPS = (int)Args.KeyArguments["FPS"];
            }
            string DirPath = (string)Args.Arguments[0];
            string AnimationDir = p_GetRelativePath(Args.Handler.GetCurrentPath(), DirPath);
            List<string> DirectionContent = new List<string>(UnitDirectoryIterator.GetDirectoryFiles_Recursive(AnimationDir));
            DirectionContent.Sort();
            foreach (string AnimationFile in DirectionContent)
            {
                if (AnimationFile.EndsWith(".meta"))
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
            p_UpdateVisualKeyParameter(ReturnValue, Args);
            return new Animation(ReturnValue);
        }
        public static Dictionary<string, UnitScript.Builtin_FuncInfo> GetUnitScriptFuncs()
        {
            Dictionary<string, UnitScript.Builtin_FuncInfo> ReturnValue = new Dictionary<string, UnitScript.Builtin_FuncInfo>();
            Dictionary<string, Type> CommonKeyArgTypes = new Dictionary<string, Type>{
                {"XCenter",typeof(int)},
                {"YCenter",typeof(int)},
                {"Width",typeof(int)},
                {"FPS",typeof(int)},
            };

            UnitScript.Builtin_FuncInfo Image = new UnitScript.Builtin_FuncInfo();
            Image.ArgTypes = new List<HashSet<Type>> { new HashSet<Type> { typeof(string) } };
            Image.ResultType = typeof(Animation);
            Image.ValidContexts = UnitScript.EvalContext.Compile;
            Image.Callable = p_Image;
            Image.KeyArgTypes = CommonKeyArgTypes;
            ReturnValue["Image"] = Image;

            UnitScript.Builtin_FuncInfo Video = new UnitScript.Builtin_FuncInfo();
            Video.ArgTypes = new List<HashSet<Type>> { new HashSet<Type> { typeof(string) } };
            Video.ResultType = typeof(Animation);
            Video.ValidContexts = UnitScript.EvalContext.Compile;
            Video.Callable = p_Video;
            Video.KeyArgTypes = CommonKeyArgTypes;
            ReturnValue["Video"] = Video;

            UnitScript.Builtin_FuncInfo Animation = new UnitScript.Builtin_FuncInfo();
            Animation.ArgTypes = new List<HashSet<Type>> { new HashSet<Type> { typeof(string) } };
            Animation.ResultType = typeof(Animation);
            Animation.ValidContexts = UnitScript.EvalContext.Compile;
            Animation.Callable = p_Animation;
            Animation.KeyArgTypes = CommonKeyArgTypes;
            ReturnValue["Animation"] = Animation;

            return ReturnValue;
        }
    }
}