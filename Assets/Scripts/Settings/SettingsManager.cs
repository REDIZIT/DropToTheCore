using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace InGame.Settings
{
    public static class SettingsManager
    {
        public static SettingsModel Settings
        {
            get
            {
                if (_settings == null)
                {
                    Load();
                }
                return _settings;
            }
            set
            {
                _settings = value;
                Save();
            }
        }


        private static SettingsModel _settings;
        private static string absoluteFilePath = Application.persistentDataPath + "/Data/settings.json";






        public static void Load()
        {
            CreateDirectoryIfNeeded(absoluteFilePath);


            if (File.Exists(absoluteFilePath))
            {
                _settings = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(absoluteFilePath));
            }
            else
            {
                _settings = new SettingsModel();
            }
        }
        public static void Save()
        {
            CreateDirectoryIfNeeded(absoluteFilePath);

            File.WriteAllText(absoluteFilePath, JsonConvert.SerializeObject(_settings, Formatting.Indented));
        }


        private static void CreateDirectoryIfNeeded(string filepath)
        {
            string parentFolder = new FileInfo(filepath).DirectoryName;
            Directory.CreateDirectory(parentFolder);
        }
    }
}