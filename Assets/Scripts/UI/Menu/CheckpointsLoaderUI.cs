using Assets.SimpleLocalization;
using InGame.Level.Generation;
using InGame.SceneLoading;
using InGame.Secrets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Menu
{
    public class CheckpointsLoaderUI : MonoBehaviour
    {
        public CheckpointLoaderItem topItem;

        public GameObject regularCheckpointItemPrefab;
        public Transform content;

        public Text recordText;

        public RectTransform loadingScreenMask;
        public Animator loadingScreenAnimator;

        private void Awake()
        {
            GlobalEvents.onSaveDataLoaded += Start;
        }
        private void Start()
        {
            float distance = CheckpointLevelGenerator.CHECKPOINTS_DISTANCE;

            int recordDepth = SecretsManager.Secrets.Records.GetRecord(SceneLoader.LoadGameType.Checkpoints);

            topItem.Refresh(Mathf.FloorToInt(recordDepth / distance) * distance, OnDepthItemClicked);
            recordText.text = LocalizationManager.Localize("YourRecordIs") + " " + recordDepth + "m";


            int checkpoints = Mathf.FloorToInt(recordDepth / distance);
            if (checkpoints >= 1)
            {
                List<float> depthes = new List<float>();
                for (int i = 0; i < checkpoints; i++)
                {
                    depthes.Add(i * distance);
                }

                depthes.Reverse();
                foreach (float depth in depthes)
                {
                    CreateCheckpointItem(depth);
                }
            }
        }

        private void CreateCheckpointItem(float depth)
        {
            GameObject obj = Instantiate(regularCheckpointItemPrefab, content);
            obj.GetComponent<CheckpointLoaderItem>().Refresh(depth, OnDepthItemClicked);
        }

        private void OnDepthItemClicked(float depth)
        {
            loadingScreenMask.anchoredPosition = Input.mousePosition;
            loadingScreenAnimator.Play("ShowScreen");

            StartCoroutine(SceneLoader.LoadCheckpointsGame(depth));
        }
    }
}