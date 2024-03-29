using InGame.Camera;
using InGame.GooglePlay;
using InGame.Level;
using InGame.Level.Generation;
using InGame.SceneLoading;
using InGame.Secrets;
using InGame.Settings;
using InGame.UI;
using InGame.UI.Game;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

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
        [SerializeField] private UnpauseCountdownText countdownText;

        [Header("Ads")]
        public Button watchAdBtn;
        public Button restartBtn, upgradesBtn;

        [Header("Other")]
        public float depth;

        public Text depthText;

        public bool isAlive = true;


        [HideInInspector] public CheckpointController currentCheckpoint;
        

        // One run properties
        private bool isAdWatchedInRun;
        private float startDepth;

        // DI
        private new CameraController camera;


        public const float WORLD_WIDTH = 32;

        [Inject]
        private void Construct(CameraController camera)
        {
            this.camera = camera;
        }
        private void Awake()
        {
            instance = this;
            postProcessing.profile.components.Find(c => c.GetType() == typeof(Bloom)).active = SettingsManager.Settings.IsBloomEnabled;

            HandleLevelGenerators();

            depth = generator.GetPlayerStartDepth(SceneLoader.LoadOnDepth) + 5;
            player.transform.position = new Vector3(0, -depth);

            startDepth = depth;


            Advertisement.Initialize("3872551", false);
        }

        private void Update()
        {
            depth = -player.transform.position.y;
            depthText.text = new KilometersString(depth);

            upgradesBtn.interactable = restartBtn.interactable;
            upgradesBtn.gameObject.SetActive(restartBtn.gameObject.activeSelf);

            if (!isAdWatchedInRun)
            {
                GooglePlayManager.ReportCurrentRunDepth(depth - startDepth);
            }

            if (depth <= -1000)
            {
                GooglePlayManager.GiveHeight1kmAchievement();
            }

            if (SettingsManager.Settings.enableFingerPause && Input.touches.Length >= 2)
            {
                Pause();
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus == false)
            {
                Pause();
            }
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
            player.Revive();

            camera.Reset();


            if (!resumeGame)
            {
                Time.timeScale = 1;
                isAdWatchedInRun = false;
                generator.ClearSpawnedObjects();
                generator.ResetByRevive(currentCheckpoint);

                startDepth = currentCheckpoint == null ? 0 : currentCheckpoint.depth;
            }
            else
            {
                StartCoroutine(IEResumedRevive());
            }
        }
        


        public void Pause()
        {
            Time.timeScale = 0;
            pauseScreen.gameObject.SetActive(true);
            pauseScreen.Play("ShowPauseScreen");
        }
        public void ClickUnpause()
        {
            StartCoroutine(Unpause());
        }
        public void ClickRestart()
        {
            pauseScreen.gameObject.SetActive(false);
            Revive();
        }
        public void GoToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }


        private IEnumerator IEResumedRevive()
        {
            player.shield.Charge();
            player.shield.TryUse();

            yield return StartCoroutine(Unpause());
        }
        private IEnumerator Unpause()
        {
            pauseScreen.gameObject.SetActive(false);

            Time.timeScale = 0;

            countdownText.StartCountdown();
            yield return new WaitUntil(() => countdownText.IsCounted);

            Time.timeScale = 1;
        }



        public void DeadActions(bool canRetry = true)
        {
            isAlive = false;

            deathScreen.SetActive(true);
            deathScreen.GetComponent<Animator>().Play("ShowDeathScreen");

            deathScreenDepthText.text = new KilometersString(Mathf.RoundToInt(depth));
            depthRecordText.text = new KilometersString(SecretsManager.Secrets.Records.GetRecord(SceneLoader.GameType));




            if (SecretsManager.Secrets.Records.IsRecord(SceneLoader.GameType, depth))
            {
                newRecordText.SetActive(true);

                SecretsManager.Secrets.Records.UpdateRecord(SceneLoader.GameType, Mathf.RoundToInt(depth));
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
