using System.Collections;
using System.Collections.Generic;
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
    public class Audio
    {

    }
    public class UnitUIInfo
    {
        public Animation UpAnimation = new Animation();
        public Animation DownAnimation = new Animation();
        public Animation AttackAnimation = null;

        public Audio SelectSound = null;

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
        public int ResourceID = 0;

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
        public Dictionary<int, UnitResource> m_IDToUnitResource = new();
        Dictionary<string, Visual> m_LoadedVisuals = new Dictionary<string, Visual>();
        UnitScript.UnitConverter m_ScriptHandler = new UnitScript.UnitConverter();
        Parser.Parser m_Parser = new Parser.Parser();
        MBCC.Tokenizer m_Tokenizer;
        bool m_VisualsLoaded = false;
        string m_ResourceFolder = "";
        int m_CurrentResourceID = 0;

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
        public UnitResource GetUnitResource(int ResourceID)
        {
            LoadResourceFolder(m_ResourceFolder);
            if (m_IDToUnitResource.ContainsKey(ResourceID))
            {
                return (m_IDToUnitResource[ResourceID]);
            }
            throw new System.Exception("No unit with id " + ResourceID + " is loaded");
        }
        public bool HasResourceWithID(int ResourceID)
        {
            LoadResourceFolder(m_ResourceFolder);
            return m_IDToUnitResource.ContainsKey(ResourceID);
        }


        void LoadUnitFile(string PathToLoad)
        {
            m_ScriptHandler.SetCurrentPath(PathToLoad);
            string Text = File.ReadAllText(PathToLoad);
            m_Tokenizer.SetText(Text);
            List<UnitScript.Diagnostic> Errors = new List<UnitScript.Diagnostic>();
            try
            {
                Parser.Unit ParsedUnit = m_Parser.ParseUnit(m_Tokenizer);
                UnitResource NewResource = m_ScriptHandler.ConvertUnit(Errors,ParsedUnit,m_CurrentResourceID);
                m_IDToUnitResource[NewResource.ResourceID] = NewResource;
                m_CurrentResourceID++;
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
       
        public Dictionary<string,UnitScript.Builtin_FuncInfo> GetUnitScriptFuncs()
        {
            return ResourceManagerImpl.GetUnitScriptFuncs();
        }
    }

}
