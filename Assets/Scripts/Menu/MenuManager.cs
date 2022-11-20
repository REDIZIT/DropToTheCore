using Assets.SimpleLocalization;
using InGame.Audio;
using InGame.Secrets;
using InGame.Settings;
using InGame.UI.Custom;
using InGame.Utils;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using YG;

namespace InGame.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Volume postProcessingVolume;

        [Header("Settings")]
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private CustomToggle bloomToggle;
        [SerializeField] private CustomToggle highScaleToggle;
        [SerializeField] private CustomToggle fingerPauseToggle;

        private void Awake()
        {
            Application.targetFrameRate = 120;

            GlobalEvents.onSaveDataLoaded += LoadSettings;
            LoadSettings();
            CheatEngine.Initialize();

            YandexGame.SwitchLangEvent += LocalizationManager.SwitchLanguage;
        }
        private void OnDestroy()
        {
            YandexGame.SwitchLangEvent -= LocalizationManager.SwitchLanguage;
        }

        public void OnMusicVolumeDrag()
        {
            AudioManager.asource.volume = musicVolumeSlider.value / 10f;
            SettingsManager.Settings.MusicVolume = musicVolumeSlider.value;
            SettingsManager.Save();
        }
        public void ShowAchievements()
        {
            Debug.Log("Ignored. ShowAchievements");
            //GooglePlayManager.ShowAchievements();
        }
        public void OnLanguageBtnClick(string language)
        {
            YandexGame.SwitchLanguage(language);
        }


        private void LoadSettings()
        {
            postProcessingVolume.profile.components.First(c => c.GetType() == typeof(Bloom)).active = SettingsManager.Settings.IsBloomEnabled;
            (GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset).renderScale = SettingsManager.Settings.UseHighRenderScale ? 1 : 0.75f;




            musicVolumeSlider.value = SettingsManager.Settings.MusicVolume;
            AudioManager.asource.volume = SettingsManager.Settings.MusicVolume / 10f;



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


            fingerPauseToggle.SetIsOnWithoutAnimation(SettingsManager.Settings.enableFingerPause);
            fingerPauseToggle.OnStateChanged += (isOn) =>
            {
                SettingsManager.Settings.enableFingerPause = isOn;
                SettingsManager.Save();
            };

            //if(SettingsManager.Settings.Language == "Not set")
            //{
            //    SettingsManager.Settings.Language = GetSupportedSystemLanguage();
            //}
            //LocalizationManager.Language = SettingsManager.Settings.Language;
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