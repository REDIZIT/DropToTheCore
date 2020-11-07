using UnityEngine;

namespace InGame.Game.Bonuses
{
    public class BonusController : MonoBehaviour
    {
        public Bonus model;
        public Animator animator;

        private bool isUsed;


        private void Update()
        {
            float noiseX = Mathf.PerlinNoise(transform.position.y / 10f, Time.time / 10f) - 0.5f;
            float noiseY = Mathf.PerlinNoise(transform.position.x / 10f, Time.time / 10f) - 0.5f;


            transform.position += new Vector3(noiseX / 15f, noiseY / 15f + (5f * Time.deltaTime));
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -14.5f, 14.5f), transform.position.y);

            if (transform.position.y >= PlayerController.instance.transform.position.y + 100)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isUsed) return;

            if (collision.transform.CompareTag("Player"))
            {
                if (GameManager.instance.isAlive)
                {
                    if (model.TryApply(PlayerController.instance))
                    {
                        isUsed = true;
                        animator.Play("BonusUse");

                        Destroy(gameObject, 1);
                    }
                }
            }
        }
    }

    public enum BonusType
    {
        Shield
    }
}