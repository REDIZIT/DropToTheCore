namespace InGame.Settings
{
    [System.Serializable]
    public class SettingsModel
    {
        public float MusicVolume = 0.8f;
        public bool IsBloomEnabled = true;
        public bool UseHighRenderScale  = true;
        public bool enableFingerPause = true;

        public void SetLowestPreset()
        {
            IsBloomEnabled = false;
            UseHighRenderScale = false;
        }
        public void SetMediumPreset()
        {
            IsBloomEnabled = true;
            UseHighRenderScale = false;
        }
        public void SetHighestPreset()
        {
            IsBloomEnabled = true;
            UseHighRenderScale = true;
        }
    }
}