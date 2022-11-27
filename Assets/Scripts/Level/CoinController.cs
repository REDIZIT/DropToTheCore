using InGame.Secrets;
using InGame.Tutorial;
using UnityEngine;

namespace InGame.Level
{
    public class CoinController : MonoBehaviour
    {
        public Animator animator;

        public string soundName;

        private bool isGot;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isGot) return;
            if (collision.transform.CompareTag("Player"))
            {
                isGot = true;

                PlayerController.instance.audioSource.PlayOneShot(soundName);

                animator.Play("CoinTake");
                SecretsManager.Secrets.Coins++;
                SecretsManager.Save();

                Destroy(gameObject, 1);

                TutorialPager.Events.OnCoinTaken();
            }
        }
    }
}