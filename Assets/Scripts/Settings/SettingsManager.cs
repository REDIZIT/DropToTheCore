using YG;

namespace InGame.Settings
{
    public static class SettingsManager
    {
        public static SettingsModel Settings => YandexGame.savesData.settings;

        public static void Save()
        {
            YandexGame.SaveProgress();
        }
    }
}