using System.IO;
using System.Threading;
using UnityEngine;

namespace InGame.Audio
{
    [RequireComponent(typeof(AudioYB))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public static AudioYB asource;

        [SerializeField] private string[] audioNames;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                asource = GetComponent<AudioYB>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            asource.Play(audioNames.Random());
        }
    }
}