using UnityEngine;
using GameAnalyticsSDK;

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
            Debug.Log("Send player death event");
            GameAnalytics.NewDesignEvent("Player death", depth);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "test");
        }
    }
}