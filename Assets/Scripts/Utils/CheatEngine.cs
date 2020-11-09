using InGame.SceneLoading;
using InGame.Secrets;
using IngameDebugConsole;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace InGame.Utils
{
//#if UNITY_EDITOR
//    [InitializeOnLoad]
//#endif
    public static class CheatEngine
    {
        public static void Initialize()
        {
            if (!File.Exists(Application.persistentDataPath + "/Data/.dev")) return;


            DebugLogConsole.AddCommand<int>("givecoins", "give coins [count of coins]", Commands.GiveCoins);
            DebugLogConsole.AddCommand("clearTutorials", "reset all passed tutorials", Commands.ClearTutorial);
            DebugLogConsole.AddCommand<int, int>("setRecord", "gamemodeId => 0 - checkpoints, 1 - infinity, 2 - hard infinity", Commands.SetRecord);
        }



        private static class Commands
        {
            public static void GiveCoins(int count)
            {
                SecretsManager.Secrets.Coins += count;
                SecretsManager.Save();
            }

            public static void ClearTutorial()
            {
                SecretsManager.Secrets.PassedTutorials = Tutorial.TutorialPager.Page.None;
                SecretsManager.Save();
            }

            public static void SetRecord(int gamemodeId, int depth)
            {
                SceneLoader.LoadGameType gamemode = (SceneLoader.LoadGameType)gamemodeId;

                if (gamemode == SceneLoader.LoadGameType.Checkpoints) SecretsManager.Secrets.DepthRecord = depth;
                else if (gamemode == SceneLoader.LoadGameType.Infinity) SecretsManager.Secrets.InfinityDepthRecord = depth;
                else if (gamemode == SceneLoader.LoadGameType.HardInfinity) SecretsManager.Secrets.HardInfinityDepthRecord = depth;
            }
        }
    }
}