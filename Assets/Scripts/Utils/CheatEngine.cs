using InGame.Secrets;
using IngameDebugConsole;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace InGame.Utils
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class CheatEngine
    {
        static CheatEngine()
        {
            if (!File.Exists(Application.persistentDataPath + "/Data/.dev")) return;


            DebugLogConsole.AddCommand<int>("givecoins", "give coins [count of coins]", GiveCoins);
            DebugLogConsole.AddCommand("clearTutorials", "reset all passed tutorials", ClearTutorial);
        }

        private static void GiveCoins(int count)
        {
            SecretsManager.Secrets.Coins += count;
            SecretsManager.Save();
        }

        private static void ClearTutorial()
        {
            SecretsManager.Secrets.PassedTutorials = Tutorial.TutorialPager.Page.None;
            SecretsManager.Save();
        }
    }
}