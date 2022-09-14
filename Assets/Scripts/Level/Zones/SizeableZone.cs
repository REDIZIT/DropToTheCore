using InGame.Game;
using UnityEngine;

namespace InGame.Level
{
    public class SizeableZone : MonoBehaviour
    {
        public float height = 30;

        public new BoxCollider2D collider;
        public SpriteRenderer spriteRenderer;
        public Transform mask;
        public ParticleSystem particles;

        private void Start()
        {
            Rebuild();
        }

        private void Rebuild()
        {
            collider.size = new Vector2(1, height);
            spriteRenderer.size = collider.size;
            mask.transform.localScale = new Vector3(1, height);
            var sh = particles.shape;
            sh.scale = new Vector3(GameManager.WORLD_WIDTH, 1, height);
        }
    }
}