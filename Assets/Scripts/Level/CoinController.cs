using InGame.Secrets;
using InGame.Tutorial;
using UnityEngine;

namespace InGame.Level
{
    public class CoinController : MonoBehaviour
    {
        public Animator animator;

        public AudioClip sound;

        private bool isGot;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isGot) return;
            if (collision.transform.CompareTag("Player"))
            {
                isGot = true;

                PlayerController.instance.audioSource.PlayOneShot(sound);

                animator.Play("CoinTake");
                SecretsManager.Secrets.Coins++;
                SecretsManager.Save();

                Destroy(gameObject, 1);

                TutorialPager.Events.OnCoinTaken();
            }
        }
    }
}