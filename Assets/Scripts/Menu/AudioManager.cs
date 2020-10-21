using System.IO;
using System.Threading;
using UnityEngine;

namespace InGame.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public static AudioSource asource;

        private string OVERRIDE_MUSIC_FOLDER;

        [System.Obsolete]
        private void Awake()
        {
            OVERRIDE_MUSIC_FOLDER = Application.persistentDataPath + "/Data/Music";

            if (instance == null)
            {
                instance = this;
                asource = GetComponent<AudioSource>();
                DontDestroyOnLoad(gameObject);

                if (Directory.Exists(OVERRIDE_MUSIC_FOLDER))
                {
                    string[] files = Directory.GetFiles(OVERRIDE_MUSIC_FOLDER);
                    if (files.Length > 0)
                    {
                        asource.clip = LoadAudio(files[0]);
                        asource.Play();
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [System.Obsolete]
        private AudioClip LoadAudio(string path)
        {
            Debug.Log($"LoadAudio({path})");
            // Must work in sync mode!
            using (WWW www = new WWW("file:///" + System.Net.WebUtility.UrlEncode(path)))
            {
                while (!www.isDone) { Thread.Sleep(16); }
                return www.GetAudioClip(false, false);
            }
        }
    }
}