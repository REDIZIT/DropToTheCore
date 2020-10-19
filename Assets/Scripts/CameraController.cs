using UnityEngine;

namespace InGame.Camera
{
    public class CameraController : MonoBehaviour
    {
        public PlayerController player;
        public float offset = -12;

        private float currentVelocityOffset;

        private void Update()
        {
            float velocityOffset = player.rigidbody.velocity.y / 50f;

            currentVelocityOffset += (velocityOffset - currentVelocityOffset) * 0.04f;

            transform.position = new Vector3(0, player.transform.position.y + offset - currentVelocityOffset, transform.position.z);
        }
    }
}
