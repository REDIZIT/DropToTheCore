using System;
using UnityEngine;

namespace InGame.Camera
{
    public class CameraController : MonoBehaviour, IDisposable
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private float offset = -12;
        [SerializeField] private float cameraMinVerticalOffset = 4;

        private float currentVelocityOffset;

        private float prevJumpY;
        private float flatEdgeY = float.MinValue;

        private void Awake()
        {
            player.onJump += OnPlayerJump;
        }
        private void Update()
        {
            float velocity = player.rigidbody.velocity.y;
            float velocityOffset = velocity / 50f;

            currentVelocityOffset += (velocityOffset - currentVelocityOffset) * 0.04f;



            float playerY = player.transform.position.y;

            flatEdgeY = Mathf.Max(flatEdgeY, playerY - cameraMinVerticalOffset);

            float cameraY;

            if (playerY > flatEdgeY)
            {
                cameraY = flatEdgeY + offset;
            }
            else
            {
                cameraY = playerY + offset;
            }

            transform.position = new Vector3(0, cameraY, transform.position.z);
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
