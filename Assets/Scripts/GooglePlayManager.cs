//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using UnityEngine;

namespace InGame.GooglePlay
{
    public static class GooglePlayManager
    {
        private const string TUTORIAL_ID = "CgkIo5iH1dYXEAIQAA";
        private const string COMING_SOON_ID = "CgkIo5iH1dYXEAIQAg";

        public static void Initialize()
        {

        }

        public static void ShowAchievements()
        {
            //Social.ShowAchievementsUI();
        }
        public static void GiveTutorialAchievement()
        {
            //Social.ReportProgress(TUTORIAL_ID, 100, ReportProgressCallback);
        }
        public static void GiveComingSoonAchievement()
        {
            //Social.ReportProgress(COMING_SOON_ID, 100, ReportProgressCallback);
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