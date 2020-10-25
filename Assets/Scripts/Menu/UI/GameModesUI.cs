using InGame.SceneLoading;
using UnityEngine;

namespace InGame.Menu
{
    public class GameModesUI : MonoBehaviour
    {
        public Animator depthsScreen;

        public RectTransform loadingScreenMask;
        public Animator loadingScreenAnimator;

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