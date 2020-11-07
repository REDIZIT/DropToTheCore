using Assets.SimpleLocalization;
using InGame.Level;
using InGame.Level.Generation;
using InGame.SceneLoading;
using InGame.Secrets;
using InGame.Settings;
using InGame.UI;
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
        public Volume postProcessing;

        [Header("Level generators")]
        public CheckpointLevelGenerator checkpointGenerator;
        public InfiniteLevelGenerator infiniteGenerator;
        public HardInfiniteLevelGenerator hardInfiniteGenerator;
        [HideInInspector] public BasicLevelGenerator generator;

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
        private string meterSymbol;

        public bool isAlive = true;


        [HideInInspector] public CheckpointController currentCheckpoint;

        private bool isAdWatchedInRun;


        public const float WORLD_WIDTH = 32;


        private void Awake()
        {
            instance = this;
            postProcessing.profile.components.Find(c => c.GetType() == typeof(Bloom)).active = SettingsManager.Settings.IsBloomEnabled;

            HandleLevelGenerators();

            depth = generator.GetPlayerStartDepth(SceneLoader.LoadOnDepth) + 5;
            player.transform.position = new Vector3(0, -depth);
            meterSymbol = LocalizationManager.Localize("MeterSymbol");


            Advertisement.Initialize("3872551", false);
        }

        private void Update()
        {
            depth = -player.transform.position.y;
            depthText.text = Mathf.RoundToInt(depth) + meterSymbol;
        }

        private void HandleLevelGenerators()
        {
            checkpointGenerator.enabled = SceneLoader.GameType == SceneLoader.LoadGameType.Checkpoints;
            infiniteGenerator.enabled = SceneLoader.GameType == SceneLoader.LoadGameType.Infinity;
            hardInfiniteGenerator.enabled = SceneLoader.GameType == SceneLoader.LoadGameType.HardInfinity;


            switch (SceneLoader.GameType)
            {
                case SceneLoader.LoadGameType.Checkpoints: generator = checkpointGenerator; break;
                case SceneLoader.LoadGameType.Infinity: generator = infiniteGenerator; break;
                case SceneLoader.LoadGameType.HardInfinity: generator = hardInfiniteGenerator; break;
            }
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
                generator.ClearSpawnedObjects();
                generator.ResetByRevive(currentCheckpoint);
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

            if (deathScreen == null)
            {
                Debug.LogError("Death screen is null");
            }
            else
            {
                Debug.Log("Death screen is");
                Debug.Log(deathScreen.name);
            }
            deathScreen.SetActive(true);
            deathScreen.GetComponent<Animator>().Play("ShowDeathScreen");

            deathScreenDepthText.text = Mathf.RoundToInt(depth) + "m";
            depthRecordText.text = new KilometersString(SecretsManager.Secrets.DepthRecord);

            if (Mathf.RoundToInt(depth) > SecretsManager.Secrets.DepthRecord)
            {
                newRecordText.SetActive(true);

                int record = Mathf.RoundToInt(depth);
                if (SceneLoader.GameType == SceneLoader.LoadGameType.Checkpoints) SecretsManager.Secrets.DepthRecord = record;
                else if (SceneLoader.GameType == SceneLoader.LoadGameType.Infinity) SecretsManager.Secrets.InfinityDepthRecord = record;
                else if(SceneLoader.GameType == SceneLoader.LoadGameType.HardInfinity) SecretsManager.Secrets.HardInfinityDepthRecord = record;
                SecretsManager.Save();
            }
            else
            {
                newRecordText.SetActive(false);
            }

            
            if (!isAdWatchedInRun && Advertisement.IsReady() && canRetry)
            {
                StartCoroutine(IEAdButtonPresent());
            }
            else
            {
                restartBtn.interactable = true;
                watchAdBtn.gameObject.SetActive(false);
            }


            restartBtn.gameObject.SetActive(canRetry);
        }

        public void WatchAd()
        {
            if (Advertisement.IsReady())
            {
                ShowOptions op = new ShowOptions()
                {
                    resultCallback = (r) =>
                    {
                        Debug.Log("Ad result is " + r);
                        isAdWatchedInRun = true;
                        Revive(true);
                    }
                };
                Advertisement.Show("rewardedVideo", op);
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
            if (showResult == ShowResult.Finished)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    isAdWatchedInRun = true;
                    Revive(true);
                });
            }
        }
    }
}
