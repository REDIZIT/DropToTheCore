using GooglePlayGames;
using UnityEngine;

namespace InGame.GooglePlay
{
    public static class GooglePlayManager
    {
        private const string TUTORIAL_ID = "CgkIo5iH1dYXEAIQAA";

        public static void Initialize()
        {
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((success) => { if (!success) { Debug.Log("Google Play Social auth result is " + success); } });
        }

        public static void ShowAchievements()
        {
            Social.ShowAchievementsUI();
        }
        public static void GetTutorialAchievement()
        {
            Social.ReportProgress(TUTORIAL_ID, 100, ReportProgressCallback);
        }

        private static void ReportProgressCallback(bool success)
        {
            if (!success)
            {
                Debug.LogError("Failed to report progress to Google Play");
            }
        }
    }
}