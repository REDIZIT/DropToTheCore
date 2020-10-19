using UnityEngine;

namespace InGame.Level
{
    public class DisappearPlatform : MonoBehaviour
    {
        public Animator animator;
        public new BoxCollider2D collider;
        public const float DISAPPEAR_TIME = 0.25f;

        private bool isHit;
        private bool isFallen;
        private float currentTime;

        private void Update()
        {
            if (!isHit || isFallen) return;

            
            if(currentTime <= 0)
            {
                isFallen = true;
                animator.Play("DisappearPlatformFall");
                collider.enabled = false;

                Destroy(gameObject, 1);
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isHit) return;

            isHit = true;
            currentTime = DISAPPEAR_TIME;
        }
    }
}