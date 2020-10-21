namespace InGame.Settings
{
    public class SettingsModel
    {
        public float MusicVolume { get; set; } = 0.8f;
        public bool IsBloomEnabled { get; set; } = true;
        public bool UseHighRenderScale { get; set; } = true;
        public bool IsTutorialPassed { get; set; } = false;




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