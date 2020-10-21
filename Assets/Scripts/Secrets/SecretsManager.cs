using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace InGame.Secrets
{
    public static class SecretsManager
    {
        public static SecretsModel Secrets
        {
            get
            {
                if (_secrets == null)
                {
                    Load();
                }
                return _secrets;
            }
            set
            {
                _secrets = value;
            }
        }


        private static SecretsModel _secrets;
        private static readonly string filepath = Application.persistentDataPath + "/Data/.secrets";


        private static void Load()
        {
            if (File.Exists(filepath))
            {
                string encrypted = File.ReadAllText(filepath);
                string key = new DirectoryInfo(Application.persistentDataPath).CreationTime.Ticks.ToString();

                try
                {
                    string decrypted = StringCipher.Decrypt(encrypted, key);
                    _secrets = JsonConvert.DeserializeObject<SecretsModel>(decrypted);
                }
                catch
                {
                    //Debug.LogError("А кто тут такой мамкин хацкер? :D");
                    File.Delete(filepath);
                    _secrets = new SecretsModel();
                }

                
            }
            else
            {
                _secrets = new SecretsModel();
            }
        }
        public static void Save()
        {
            string parentFolder = new FileInfo(filepath).DirectoryName;
            Directory.CreateDirectory(parentFolder);

            string content = JsonConvert.SerializeObject(_secrets, Formatting.Indented);
            string key = new DirectoryInfo(Application.persistentDataPath).CreationTime.Ticks.ToString();


            string encrypted = StringCipher.Encrypt(content, key);
            File.WriteAllText(filepath, encrypted);
        }
    }
}