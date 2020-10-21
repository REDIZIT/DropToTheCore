using InGame.Game;
using TMPro;
using UnityEngine;

namespace InGame.Level
{
    public class CheckpointController : MonoBehaviour
    {
        public Animator animator;
        public HeightBorder border;
        public TextMeshPro depthText, depthTextSmall;

        public bool isClaimed;

        private PlayerController player;

        private void Start()
        {
            player = PlayerController.instance;
        }

        private void Update()
        {
            if (player.transform.position.y < border.transform.position.y && !isClaimed)
            {
                isClaimed = true;
                animator.Play("CheckpointClaim");

                GameManager.instance.currentCheckpoint = this;
            }
        }


        public void Refresh(float depth)
        {
            depthText.text = depth + "m";
            depthTextSmall.text = depthText.text;
        }
    }
}