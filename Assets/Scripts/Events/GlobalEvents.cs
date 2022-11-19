using System;

public static class GlobalEvents
{
    /// <summary>Invokes when game reading secrets file from Google Cloud and importing it to game. Use this event to refresh UI if you don't want use Update instead of Start</summary>
    public static Action onSaveDataLoaded;
}
