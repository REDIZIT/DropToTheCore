using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

namespace InGame.Analytics
{
    public class GameAnalytics
    {
        public void SendPlayerDeath(int depth)
        {
            // Send custom event
            Dictionary<string, object> parameters = new Dictionary<string, object>()
{
    { "Depth", depth },
};
            // The ‘myEvent’ event will get queued up and sent every minute
            AnalyticsService.Instance.SetAnalyticsEnabled(true);
            Debug.Log(AnalyticsService.Instance.GetAnalyticsUserID());
            AnalyticsService.Instance.CustomData("Player death", parameters);
            //Debug.Log(UnityEngine.Analytics.Analytics.SendEvent("Test", parameters));

            // Optional - You can call Events.Flush() to send the event immediately
            AnalyticsService.Instance.Flush();

            Debug.Log("Sent");
        }
    }
}