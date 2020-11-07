using Assets.SimpleLocalization;
using InGame.Audio;
using InGame.GooglePlay;
using InGame.Settings;
using InGame.UI.Custom;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace InGame.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public Volume postProcessingVolume;

        [Header("Settings")]
        public Slider musicVolumeSlider;
        public CustomToggle bloomToggle;
        public CustomToggle highScaleToggle;

        private void Awake()
        {
            LoadSettings();
            GooglePlayManager.Initialize();
        }


        public void OnMusicVolumeDrag()
        {
            AudioManager.asource.volume = musicVolumeSlider.value;
            SettingsManager.Settings.MusicVolume = musicVolumeSlider.value;
            SettingsManager.Save();
        }
        public void ShowAchievements()
        {
            GooglePlayManager.ShowAchievements();
        }
        public void OnLanguageBtnClick(string language)
        {
            SettingsManager.Settings.Language = language;
            SettingsManager.Save();
            LocalizationManager.Language = language;
        }


        private void LoadSettings()
        {
            postProcessingVolume.profile.components.First(c => c.GetType() == typeof(Bloom)).active = SettingsManager.Settings.IsBloomEnabled;
            (GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset).renderScale = SettingsManager.Settings.UseHighRenderScale ? 1 : 0.75f;




            musicVolumeSlider.value = SettingsManager.Settings.MusicVolume;
            AudioManager.asource.volume = SettingsManager.Settings.MusicVolume;



            bloomToggle.SetIsOnWithoutAnimation(SettingsManager.Settings.IsBloomEnabled);
            bloomToggle.OnStateChanged += (isOn) =>
            {
                SettingsManager.Settings.IsBloomEnabled = isOn;
                SettingsManager.Save();

                postProcessingVolume.profile.components.First(c => c.GetType() == typeof(Bloom)).active = isOn;
            };



            highScaleToggle.SetIsOnWithoutAnimation(SettingsManager.Settings.UseHighRenderScale);
            highScaleToggle.OnStateChanged += (isOn) =>
            {
                SettingsManager.Settings.UseHighRenderScale = isOn;
                SettingsManager.Save();

                (GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset).renderScale = isOn ? 1 : 0.75f;
            };




            if(SettingsManager.Settings.Language == "Not set")
            {
                SettingsManager.Settings.Language = GetSupportedSystemLanguage();
            }
            LocalizationManager.Language = SettingsManager.Settings.Language;
        }

        private string GetSupportedSystemLanguage()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian:
                case SystemLanguage.Ukrainian:
                    return "Russian";

                case SystemLanguage.French:
                    return "French";

                case SystemLanguage.Spanish: return "Spanish";

                case SystemLanguage.Italian: return "Italian";

                default: return "English";
            }
        }
    }
}