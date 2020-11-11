using InGame.Secrets;
using UnityEngine;

namespace InGame.Menu
{
    public class PlayerPreviewManager : MonoBehaviour
    {
        public Rigidbody2D player;
        public Transform elevator;
        public GameObject jumpEffectPrefab;

        public float jumpPower = 1000;
        public float strafePower = 2000;

        private void Start()
        {
            jumpPower += jumpPower * SecretsManager.Secrets.JumpPower / 5f;
        }
        private void Update()
        {
            if (player.transform.localPosition.y <= -10)
            {
                //JumpUp();
            }

            //elevator.transform.position += new Vector3(0, Time.deltaTime, 0);

            float velocityOffset = player.velocity.y / 50f;

            currentVelocityOffset += (velocityOffset - currentVelocityOffset) * 0.04f;

            elevator.position = new Vector3(0, -(player.transform.position.y + offset - currentVelocityOffset), transform.position.z);
        }

        public float offset = -12;

        private float currentVelocityOffset;


        private void JumpUp()
        {
            float percents = Input.mousePosition.x / (float)Screen.width;
            float normalized = percents - 0.5f;

            player.velocity = Vector2.up * jumpPower * (1 / 60f);

            //player.velocity += Vector2.right * normalized * strafePower * (1 / 60f);


            GameObject effect = Instantiate(jumpEffectPrefab, elevator);
            effect.transform.position = player.transform.position;
            Destroy(effect, 3);
        }
    }
}