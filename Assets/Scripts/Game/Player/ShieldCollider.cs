using UnityEngine;

namespace InGame.Game
{
    public class ShieldCollider : MonoBehaviour
    {
        public Animator animator;
        public bool hasShield;

        public float currentInvulnerabilityTime;


        private void Update()
        {
            if (currentInvulnerabilityTime > 0) currentInvulnerabilityTime -= Time.deltaTime;
        }
        public void Charge()
        {
            hasShield = true;
            animator.Play("ShieldCharge");
        }
        public void Use()
        {
            hasShield = false;
            currentInvulnerabilityTime = 1;
            animator.Play("ShieldUse");
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Killer"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}