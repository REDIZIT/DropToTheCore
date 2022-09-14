using UnityEngine;
using GameAnalyticsSDK;
using InGame.SceneLoading;
using InGame.Level.Generation;

namespace InGame.Analytics
{
    public class CoreAnalytics
    {

        public CoreAnalytics()
        {
            Debug.Log("Start GameAnalyics init");
            GameAnalytics.Initialize();
        }
        public void SendPlayerDeath(int depth)
        {
            int distanceBetweenCheckpoints = CheckpointLevelGenerator.CHECKPOINTS_DISTANCE;
            int roundedDepth = Mathf.FloorToInt(depth / distanceBetweenCheckpoints) * distanceBetweenCheckpoints;

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, SceneLoader.GameType.ToString(), roundedDepth + "m");
        }
    }
}