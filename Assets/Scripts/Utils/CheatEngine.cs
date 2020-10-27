using InGame.Secrets;
using IngameDebugConsole;
using UnityEditor;

namespace InGame.Utils
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class CheatEngine
    {
        static CheatEngine()
        {
            DebugLogConsole.AddCommand<int>("givecoins", "give coins [count of coins]", GiveCoins);
        }

        private static void GiveCoins(int count)
        {
            SecretsManager.Secrets.Coins += count;
            SecretsManager.Save();
        }
    }
}