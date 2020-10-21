using InGame.Level;
using InGame.SceneLoading;
using InGame.Secrets;
using InGame.Settings;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InGame.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public SODB sodb;
        public PlayerController player;
        public LevelGenerator generator;
        public Volume postProcessing;

        [Header("Death screen")]
        public GameObject deathScreen;
        public Text deathScreenDepthText, depthRecordText;
        public GameObject newRecordText;

        [Header("Pause screen")]
        public Animator pauseScreen;

        public float depth;

        public Text depthText;

        public bool isAlive = true;


        [HideInInspector] public CheckpointController currentCheckpoint;


        public const float WORLD_WIDTH = 32;


        private void Awake()
        {
            instance = this;
            postProcessing.profile.components.Find(c => c.GetType() == typeof(Bloom)).active = SettingsManager.Settings.IsBloomEnabled;

            depth = generator.GetCurrentCheckpointDepth(SceneLoader.LoadOnDepth) + 5;
            player.transform.position = new Vector3(0, -depth);

            generator.GenerationLoop(true);
        }
        private void Update()
        {
            depth = -player.transform.position.y;
            depthText.text = Mathf.RoundToInt(depth) + "m";
        }

        public void Revive()
        {
            if (currentCheckpoint == null)
            {
                player.transform.position = Vector3.zero;
            }
            else
            {
                player.transform.position = currentCheckpoint.transform.position - new Vector3(0, 20, 0);
            }

            deathScreen.SetActive(false);
            Time.timeScale = 1;
            isAlive = true;
            player.Relive();
            generator.ClearSpawnedAreas(currentCheckpoint);
        }
        public void Pause()
        {
            Time.timeScale = 0;
            pauseScreen.gameObject.SetActive(true);
            pauseScreen.Play("ShowPauseScreen");
        }
        public void Unpause()
        {
            Time.timeScale = 1;
            pauseScreen.gameObject.SetActive(false);
        }
        public void GoToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }




        public void DeadActions()
        {
            isAlive = false;

            deathScreen.SetActive(true);
            deathScreen.GetComponent<Animator>().Play("ShowDeathScreen");

            deathScreenDepthText.text = Mathf.RoundToInt(depth) + "m";
            depthRecordText.text = SecretsManager.Secrets.DepthRecord + "m";

            if (Mathf.RoundToInt(depth) > SecretsManager.Secrets.DepthRecord)
            {
                newRecordText.SetActive(true);

                SecretsManager.Secrets.DepthRecord = Mathf.RoundToInt(depth);
                SecretsManager.Save();
            }
            else
            {
                newRecordText.SetActive(false);
            }
        }




        public void SetCheckpoint(CheckpointController checkpoint)
        {
            currentCheckpoint = checkpoint;
        }
    }
}
