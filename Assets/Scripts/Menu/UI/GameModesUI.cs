using InGame.SceneLoading;
using InGame.Secrets;
using InGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Menu
{
    public class GameModesUI : MonoBehaviour
    {
        public Animator depthsScreen;

        public RectTransform loadingScreenMask;
        public Animator loadingScreenAnimator;

        public Text checkpointsRecordText, infiniteRecordText, hardInfinityRecordText;

        private void Start()
        {
            checkpointsRecordText.text = new KilometersString(SecretsManager.Secrets.DepthRecord);
            infiniteRecordText.text = new KilometersString(SecretsManager.Secrets.InfinityDepthRecord);
            hardInfinityRecordText.text = new KilometersString(SecretsManager.Secrets.HardInfinityDepthRecord);
        }
        public void OnCheckpointButtonClick()
        {
            depthsScreen.Play("ShowScreen");
        }
        public void OnInfiniteButtonClick()
        {
            StartLoadAnimation();
            StartCoroutine(SceneLoader.LoadInfinityGame());
        }
        public void OnHardInfinityButtonClick()
        {
            StartLoadAnimation();
            StartCoroutine(SceneLoader.LoadHardInfinityGame());
        }

        private void StartLoadAnimation()
        {
            loadingScreenMask.anchoredPosition = Input.mousePosition;
            loadingScreenAnimator.Play("ShowScreen");
        }
    }
}