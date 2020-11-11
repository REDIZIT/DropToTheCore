using GooglePlayGames;
using GooglePlayGames.BasicApi;
using InGame.Secrets;
using InGame.Settings;
using System.Collections.Generic;
using Stopwatch = System.Diagnostics.Stopwatch;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;

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
        public static PlayGamesPlatform platform;


        private static IEnumerable<PlayGamesAchievement> achievements;
        private static bool isFirstEntry = true;

        private static GameAchievement oneRun1km, oneRun2km, oneRun5km, height1km;


        private static DateTime lastReportCurrentRunDepthTime;


        private const string TUTORIAL_ID = "CgkIo5iH1dYXEAIQAA";
        private const string COMING_SOON_ID = "CgkIo5iH1dYXEAIQAg";
        private const string ONE_RUN_1KM = "CgkIo5iH1dYXEAIQBQ";
        private const string ONE_RUN_2KM = "CgkIo5iH1dYXEAIQBg";
        private const string ONE_RUN_5KM = "CgkIo5iH1dYXEAIQBw";
        private const string HEIGHT_1KM = "CgkIo5iH1dYXEAIQCA";



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

            platform = PlayGamesPlatform.Instance;


            Social.localUser.Authenticate((success) => 
            {
                if (!success)
                {
                    Debug.Log("Google Play not authed");
                }
                else
                {

                    Social.LoadAchievements(OnAchievementsGot);

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
        public static void GiveHeight1kmAchievement()
        {
            platform.UnlockAchievement(HEIGHT_1KM);
            height1km.isUnlocked = true;
        }


        public static void ReportCurrentRunDepth(float depth)
        {
            if (!IsAuthenticated) return;
            if (depth <= 0) return;


            if ((DateTime.Now - lastReportCurrentRunDepthTime).TotalSeconds > 2)
            {
                lastReportCurrentRunDepthTime = DateTime.Now;

                SetStepsForAchievement(oneRun1km, depth);
                SetStepsForAchievement(oneRun2km, depth);
                SetStepsForAchievement(oneRun5km, depth);
            }

            
        }







        private static void SetStepsForAchievement(GameAchievement achievement, float stepsToSet)
        {
            if (achievement.refer.completed) return;

            int difference = Mathf.FloorToInt(stepsToSet - achievement.actualSteps);

            if (difference <= 0) return;


            //Stopwatch w = Stopwatch.StartNew();

            platform.IncrementAchievement(achievement.refer.id, difference, ReportProgressCallback);
            achievement.actualSteps += difference;

            //w.Stop();

            //Debug.Log($"<color=red><b>+{difference}m</b></color> steps: {achievement.actualSteps}/{achievement.refer.totalSteps}");
        }




        private static void ReportProgressCallback(bool success)
        {
            if (!success)
            {
                Debug.LogError("Failed to report progress to Google Play");
            }
        }



        private static void OnAchievementsGot(IAchievement[] callback)
        {
            achievements = callback.Cast<PlayGamesAchievement>();
            //Debug.Log($"Successfully casted to {achievements.Count()} achiements!");

            CheckTutorialMessUp(callback);

            oneRun1km = new GameAchievement(achievements.FirstOrDefault(a => a.id == ONE_RUN_1KM));
            oneRun2km = new GameAchievement(achievements.FirstOrDefault(a => a.id == ONE_RUN_2KM));
            oneRun5km = new GameAchievement(achievements.FirstOrDefault(a => a.id == ONE_RUN_5KM));
            height1km = new GameAchievement(achievements.FirstOrDefault(a => a.id == HEIGHT_1KM));
        }
        private static void CheckTutorialMessUp(IAchievement[] callback)
        {
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






    internal class GameAchievement
    {
        public float actualSteps;
        public bool isUnlocked;

        public PlayGamesAchievement refer;

        public GameAchievement(PlayGamesAchievement achievement)
        {
            this.refer = achievement;
            isUnlocked = achievement.completed;
            actualSteps = achievement.currentSteps;
        }
    }
}