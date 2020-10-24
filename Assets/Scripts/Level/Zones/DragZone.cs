using UnityEngine;

namespace InGame.Level
{
    public class DragZone : SizeableZone
    {
        public float drag = 10;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                player.rigidbody.drag *= drag;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                player.rigidbody.drag /= drag;
            }
        }
    }
}