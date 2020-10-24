using UnityEngine;

namespace InGame.Level
{
    public class GravityZone : SizeableZone
    {
        public float gravityMultiplier = 1;
        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                player.rigidbody.gravityScale *= gravityMultiplier;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                player.rigidbody.gravityScale /= gravityMultiplier;
            }
        }
    }
}