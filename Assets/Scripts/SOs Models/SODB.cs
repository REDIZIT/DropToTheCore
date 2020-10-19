using InGame.Level;
using System.Collections.Generic;
using Stopwatch = System.Diagnostics.Stopwatch;
using System.IO;
using UnityEditor;
using UnityEngine;
using InGame.Game.Bonuses;

namespace InGame
{
    [CreateAssetMenu(menuName = "InGame/SODB")]
    public class SODB : ScriptableObject
    {
        public static SODB Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null)
                {
                    Init();
                    Index();
                }
#endif
                return _instance;
            }
        }
        private static SODB _instance;

        public List<AreaSO> patterns;
        public List<Bonus> bonuses;


        public SODB()
        {
            _instance = this;
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (_instance == null)
            {
                Init();
            }
            Index();
        }




        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            _instance = AssetDatabase.LoadAssetAtPath<SODB>("Assets/GameContent/SODB.asset");
        }

        private static void Index()
        {
            Stopwatch w = Stopwatch.StartNew();
            

            Instance.patterns = new List<AreaSO>();

            string[] aMaterialFiles = Directory.GetFiles(Application.dataPath + "/GameContent/Patterns/SOs", "*.asset", SearchOption.AllDirectories);
            foreach (string matFile in aMaterialFiles)
            {
                string assetPath = "Assets" + matFile.Replace(Application.dataPath, "").Replace('\\', '/');
                Instance.patterns.Add((AreaSO)AssetDatabase.LoadAssetAtPath(assetPath, typeof(AreaSO)));
            }

            if (w.ElapsedMilliseconds > 100)
            {
                Debug.LogError($"SODB indexed patterns files in {w.ElapsedMilliseconds}ms! That's too long!");
            }
        }
#endif
    }
}