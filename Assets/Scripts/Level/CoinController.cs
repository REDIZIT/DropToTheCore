using InGame.Audio;
using InGame.Game;
using InGame.Secrets;
using UnityEngine;

namespace InGame.Level
{
    public class CoinController : MonoBehaviour
    {
        public Animator animator;

        public AudioClip sound;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                PlayerController.instance.audioSource.PlayOneShot(sound);

                animator.Play("CoinTake");
                SecretsManager.Secrets.Coins++;
                SecretsManager.Save();
            }
        }
    }
}