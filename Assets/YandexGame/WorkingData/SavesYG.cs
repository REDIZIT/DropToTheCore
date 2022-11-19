using InGame.Secrets;
using InGame.Settings;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        public bool isFirstSession = true;
        public string language = "ru";
        public bool feedbackDone;
        public bool promptDone;

        // Ваши сохранения
        public SettingsModel settings = new SettingsModel();
        public SecretsModel secrets = new SecretsModel();
    }
}
