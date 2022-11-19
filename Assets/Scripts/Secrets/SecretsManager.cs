using UnityEngine;
using YG;

namespace InGame.Secrets
{
    public static class SecretsManager
    {
        public static SecretsModel Secrets => YandexGame.savesData.secrets;

        public static void Save()
        {
            Debug.Log("Save progress");
            YandexGame.SaveProgress();
        }
    }
}