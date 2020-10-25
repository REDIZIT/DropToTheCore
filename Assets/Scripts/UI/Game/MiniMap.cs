using InGame.Game;
using InGame.Level.Generation;
using UnityEngine;

namespace InGame.UI.Game
{
    public class MiniMap : MonoBehaviour
    {
        public RectTransform mapContainer;

        public RectTransform playerImage;
        public RectTransform claimedCheckpoint, nextCheckpoint;
        public RectTransform filledBackground;

        public float mapTopPadding = 0.2f;
        public float animationSpeed = 0.02f;



        private float mapScaling = 300;
        private float mapOffset = 0;
        private float currentMapOffset = 0;
        private CheckpointLevelGenerator gen;


        private void Awake()
        {
            if (!(GameManager.instance.generator is CheckpointLevelGenerator))
            {
                enabled = false;
            }
            else
            {
                gen = GameManager.instance.generator as CheckpointLevelGenerator;
            }
        }
        private void Update()
        {
            float currentCheckpointDepth = GameManager.instance.currentCheckpoint ? -GameManager.instance.currentCheckpoint.transform.position.y : 0;
            float nextCheckpointDepth = gen.GetNextCheckpointDepth(GameManager.instance.depth);
            float playerDepth = -PlayerController.instance.transform.position.y;


            mapScaling = nextCheckpointDepth - currentCheckpointDepth + 200;

            mapOffset = currentCheckpointDepth - mapScaling * mapTopPadding;

            currentMapOffset += (mapOffset - currentMapOffset) * animationSpeed;





            SetImageToDepth(claimedCheckpoint, currentCheckpointDepth);
            SetImageToDepth(nextCheckpoint, nextCheckpointDepth);
            SetImageToDepth(playerImage, playerDepth);


            //SetImageToDepth(filledBackground, currentCheckpointDepth);
            filledBackground.sizeDelta = new Vector2(12, -playerImage.anchoredPosition.y);
        }

        private void SetImageToDepth(RectTransform image, float realDepth)
        {
            float normalizedY = (realDepth - currentMapOffset) / mapScaling;

            image.anchoredPosition = new Vector2(0, -Mathf.Clamp01(normalizedY) * mapContainer.rect.height);
        }
    }
}