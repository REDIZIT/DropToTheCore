using InGame.SceneLoading;
using InGame.Settings;
using UnityEngine;

namespace InGame.Menu
{
    public class PlayButtonUI : MonoBehaviour
    {
        public Animator animator;
        
        public void OnClick(string animationName)
        {
            if (!SettingsManager.Settings.IsTutorialPassed)
            {
                SceneLoader.LoadTutorial();
            }
            else
            {
                animator.Play(animationName);
            }
        }
    }
}