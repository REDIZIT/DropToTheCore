using GooglePlayGames;
using GooglePlayGames.BasicApi;
using InGame.Secrets;
using InGame.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace InGame.GooglePlay
{
    public static class GooglePlayManager
    {
        public static bool IsAuthenticated
        {
            get
            {
                if (PlayGamesPlatform.Instance != null) return PlayGamesPlatform.Instance.IsAuthenticated();
                return false;
            }
        }


        private static IEnumerable<IAchievement> achievements;
        private static bool isFirstEntry = true;


        private const string TUTORIAL_ID = "CgkIo5iH1dYXEAIQAA";
        private const string COMING_SOON_ID = "CgkIo5iH1dYXEAIQAg";


        public static void Initialize(bool debug = false)
        {
            if (!isFirstEntry) return;
            isFirstEntry = false;


            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                .EnableSavedGames()
                .Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = debug;
            PlayGamesPlatform.Activate();


            Social.localUser.Authenticate((success) => 
            {
                if (!success)
                {
                    Debug.Log("Google Play not authed");
                }
                else
                {

                    Social.LoadAchievements(CheckTutorialMessUp);

                    GoogleCloud.Initialize();
                    SecretsManager.PullCloudSave();
                }
            });
        }

        public static void ShowAchievements()
        {
            Social.ShowAchievementsUI();
        }
        public static void GiveTutorialAchievement()
        {
            Social.ReportProgress(TUTORIAL_ID, 100, ReportProgressCallback);
        }
        public static void GiveComingSoonAchievement()
        {
            Social.ReportProgress(COMING_SOON_ID, 100, ReportProgressCallback);
        }

        private static void ReportProgressCallback(bool success)
        {
            if (!success)
            {
                Debug.LogError("Failed to report progress to Google Play");
            }
        }



        private static void CheckTutorialMessUp(IAchievement[] callback)
        {
            achievements = callback;

            IAchievement tutorialAchievement = achievements.FirstOrDefault(c => c.id == TUTORIAL_ID);
            if (tutorialAchievement != null)
            {
                if (!tutorialAchievement.completed && SettingsManager.Settings.IsTutorialPassed)
                {
                    Social.ReportProgress(TUTORIAL_ID, 100, ReportProgressCallback);
                }
            }
        }
    }
}