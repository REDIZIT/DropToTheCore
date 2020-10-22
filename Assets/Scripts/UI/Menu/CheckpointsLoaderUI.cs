using Assets.SimpleLocalization;
using InGame.Level;
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


        private void Start()
        {
            float distance = LevelGenerator.CHECKPOINTS_DISTANCE;

            topItem.Refresh(Mathf.FloorToInt(SecretsManager.Secrets.DepthRecord / distance) * distance, OnDepthItemClicked);
            recordText.text = LocalizationManager.Localize("YourRecordIs") + " " + SecretsManager.Secrets.DepthRecord + "m";


            int checkpoints = Mathf.FloorToInt(SecretsManager.Secrets.DepthRecord / distance);
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

            StartCoroutine(SceneLoader.LoadGame(depth));
        }
    }
}