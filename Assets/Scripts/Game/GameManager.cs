using InGame.Level;
using InGame.SceneLoading;
using InGame.Secrets;
using InGame.Settings;
using InGame.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InGame.Game
{
    public class GameManager : MonoBehaviour, IUnityAdsListener
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

        [Header("Ads")]
        public Button watchAdBtn;
        public Button restartBtn;

        [Header("Other")]
        public float depth;

        public Text depthText;

        public bool isAlive = true;


        [HideInInspector] public CheckpointController currentCheckpoint;

        private bool isAdWatchedInRun;


        public const float WORLD_WIDTH = 32;


        private void Awake()
        {
            instance = this;
            postProcessing.profile.components.Find(c => c.GetType() == typeof(Bloom)).active = SettingsManager.Settings.IsBloomEnabled;

            depth = generator.GetCurrentCheckpointDepth(SceneLoader.LoadOnDepth) + 5;
            player.transform.position = new Vector3(0, -depth);

            generator.GenerationLoop(true);

            Advertisement.AddListener(this);
            Advertisement.Initialize("3872551", false);
        }

        private void Update()
        {
            depth = -player.transform.position.y;
            depthText.text = Mathf.RoundToInt(depth) + "m";
        }

        public void Revive(bool resumeGame = false)
        {
            isAlive = true;
            deathScreen.SetActive(false);

            // New player position based on resumeGame and checkpoint
            Vector3 newPlayerPosition = resumeGame ?
                    player.transform.position :
                    currentCheckpoint == null ?
                        Vector3.zero :
                        currentCheckpoint.transform.position - new Vector3(0, 20, 0);

            player.transform.position = newPlayerPosition;
            player.Relive();



            if (!resumeGame)
            {
                Time.timeScale = 1;
                isAdWatchedInRun = false;
                generator.ClearSpawnedAreas(currentCheckpoint);
            }
            else
            {
                StartCoroutine(IEResumedRevive());
            }
        }
        private IEnumerator IEResumedRevive()
        {
            player.shield.hasShield = true;
            player.shield.Use();

            Time.timeScale = 0.5f;

            while (Time.timeScale < 1)
            {
                yield return new WaitForEndOfFrame();
                Time.timeScale += (0.5f - (Time.timeScale - 0.5f)) * 0.01f;
            }
            Time.timeScale = 1;
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




        public void DeadActions(bool canRetry = true)
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

            
            if (!isAdWatchedInRun && Advertisement.IsReady())
            {
                StartCoroutine(IEAdButtonPresent());
            }
            else
            {
                restartBtn.interactable = true;
                watchAdBtn.gameObject.SetActive(false);
            }


            restartBtn.gameObject.SetActive(canRetry);
            watchAdBtn.gameObject.SetActive(canRetry);
        }

        public void WatchAd()
        {
            if (Advertisement.IsReady())
            {
                Advertisement.Show("rewardedVideo");
            }
        }

        private IEnumerator IEAdButtonPresent()
        {
            restartBtn.interactable = false;
            watchAdBtn.gameObject.SetActive(true);

            yield return new WaitForSeconds(3);

            restartBtn.interactable = true;
        }




        public void OnUnityAdsReady(string placementId) { }
        public void OnUnityAdsDidError(string message) { Debug.LogError("Unity ad error: " + message); }

        public void OnUnityAdsDidStart(string placementId) { }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (showResult != ShowResult.Failed)
            {
                isAdWatchedInRun = true;
                Revive(true);
            }
        }
    }
}
