using InGame.SceneLoading;
using InGame.Secrets;
using UnityEngine;

namespace InGame.Menu
{
    public class PlayButtonUI : MonoBehaviour
    {
        public Animator animator;
        
        public void OnClick(string animationName)
        {
            if (!SecretsManager.Secrets.IsTutorialPassed)
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