using InGame.GooglePlay;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
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
                string key = "нюхай бебру";
                string altKey = new DirectoryInfo(Application.persistentDataPath).CreationTime.Ticks.ToString();


                try
                {
                    string decrypted = "";
                    try
                    {
                        decrypted = StringCipher.Decrypt(encrypted, key);
                        _secrets = JsonConvert.DeserializeObject<SecretsModel>(decrypted);
                        Debug.Log("Decrypted as new");
                    }
                    catch
                    {
                        decrypted = StringCipher.Decrypt(encrypted, altKey);
                        Debug.Log("Decrypted as alt");

                    }
                    finally
                    {
                        _secrets = JsonConvert.DeserializeObject<SecretsModel>(decrypted);
                    }
                }
                catch
                {
                    File.Delete(filepath);
                    _secrets = new SecretsModel();
                }


            }
            else
            {
                _secrets = new SecretsModel();
                
            }
        }
        //private static void Load()
        //{
        //    if (File.Exists(filepath))
        //    {
        //        string encrypted = File.ReadAllText(filepath);
        //        string key = new DirectoryInfo(Application.persistentDataPath).CreationTime.Ticks.ToString();

        //        try
        //        {
        //            string decrypted = StringCipher.Decrypt(encrypted, key);
        //            _secrets = JsonConvert.DeserializeObject<SecretsModel>(decrypted);
        //        }
        //        catch
        //        {
        //            File.Delete(filepath);
        //            _secrets = new SecretsModel();
        //        }

                
        //    }
        //    else
        //    {
        //        _secrets = new SecretsModel();
        //        GoogleCloud.ReadData(GoogleCloud.DEFAULT_SAVE_NAME, (status, data) =>
        //        {
        //            if (status == GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.Success)
        //            {
        //                string encrypted = Encoding.UTF8.GetString(data);
        //                string key = new DirectoryInfo(Application.persistentDataPath).CreationTime.Ticks.ToString();

        //                string decrypted = StringCipher.Decrypt(encrypted, key);
        //                _secrets = JsonConvert.DeserializeObject<SecretsModel>(decrypted);
        //            }
        //        });
        //    }
        //}
        

        public static void Save()
        {
            string parentFolder = new FileInfo(filepath).DirectoryName;
            Directory.CreateDirectory(parentFolder);

            string content = JsonConvert.SerializeObject(_secrets, Formatting.Indented);
            string key = "нюхай бебру";


            string encrypted = StringCipher.Encrypt(content, key);
            File.WriteAllText(filepath, encrypted);

            UploadCloudSave(Encoding.UTF8.GetBytes(encrypted));
        }
        //public static void Save()
        //{
        //    string parentFolder = new FileInfo(filepath).DirectoryName;
        //    Directory.CreateDirectory(parentFolder);

        //    string content = JsonConvert.SerializeObject(_secrets, Formatting.Indented);
        //    string key = new DirectoryInfo(Application.persistentDataPath).CreationTime.Ticks.ToString();


        //    string encrypted = StringCipher.Encrypt(content, key);
        //    File.WriteAllText(filepath, encrypted);

        //    UploadCloudSave(Encoding.UTF8.GetBytes(encrypted));
        //}




        public static void ImportCloudSave(byte[] data)
        {
            Debug.Log("Importing cloud save");
            File.WriteAllText(filepath, Encoding.UTF8.GetString(data));
        }
        public static void PullCloudSave()
        {
            if (File.Exists(filepath)) return;

            Debug.Log("Pulling from Google Cloud default save");
            GoogleCloud.ReadData(GoogleCloud.DEFAULT_SAVE_NAME, (status, data) =>
            {
                Debug.Log("Pull result is " + status);
                if (status == GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.Success)
                {
                    File.WriteAllBytes(filepath, data);
                    Load();

                    GlobalEvents.onGoogleCloudImportSettings?.Invoke();
                }
            });
        }
        public static void UploadCloudSave(byte[] data)
        {
            Debug.Log("Uploading cloud save");
            GoogleCloud.WriteData(data);
        }
    }
}