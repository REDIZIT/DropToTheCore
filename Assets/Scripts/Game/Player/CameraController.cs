using System;
using UnityEngine;

namespace InGame.Camera
{
    public class CameraController : MonoBehaviour, IDisposable
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private float offset = -12;

        [Tooltip("Height which camera won't be following player after jump")]
        [SerializeField] private float cameraMinVerticalOffset = 10;

        private float currentVelocityOffset;

        private float prevJumpY;
        private float flatEdgeY = float.MinValue;

        /// <summary>Player velocity camera offset factor (than more, then less effect)</summary>
        private const float VELOCITY_FACTOR = 50;
        /// <summary>Velocity camera offset change factor (than more, then lower speed)</summary>
        private const float VELOCITY_INCREASE_FACTOR = 25;


        private void Awake()
        {
            player.onJump += OnPlayerJump;
        }
        private void Update()
        {
            float playerY = player.transform.position.y;

            // Select highest Y
            // flatEdgeY - camera fix point after player jump
            // If player is higher than defined, then follow camera to player
            // Else, leave camera at same Y (flatEdgeY)
            flatEdgeY = Mathf.Max(flatEdgeY, playerY - cameraMinVerticalOffset);

            float cameraY;

            // If player higher flatEdgeY (camera fix point)
            if (playerY > flatEdgeY)
            {
                // Stop following, stay on same Y, and go higher sometimes
                cameraY = flatEdgeY;
            }
            else
            {
                // Follow player down
                cameraY = playerY;
            }



            // Apply other effects
            currentVelocityOffset += (player.rigidbody.velocity.y - currentVelocityOffset) / VELOCITY_INCREASE_FACTOR;

            cameraY += offset - currentVelocityOffset / VELOCITY_FACTOR;



            transform.position = new Vector3(0, cameraY, transform.position.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(new Vector3(0, flatEdgeY), new Vector3(40, 0.2f));
        }

        public void Reset()
        {
            flatEdgeY = player.transform.position.y;
        }

        private void OnPlayerJump()
        {
            prevJumpY = player.transform.position.y;
            flatEdgeY = Mathf.Min(prevJumpY, flatEdgeY);
        }

        public void Dispose()
        {
            player.onJump -= OnPlayerJump;
        }
    }
}
