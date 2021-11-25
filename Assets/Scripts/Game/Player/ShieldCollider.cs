using System.Collections;
using UnityEngine;

namespace InGame.Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ShieldCollider : MonoBehaviour
    {
        public Animator animator;
        public bool hasShield;

        public float currentInvulnerabilityTime;

        private SpriteRenderer sprite;

        private const float SHIELD_SIZE = 60;
        private const float ANIMATION_TIME = 0.35f;


        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
        }
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
            StartCoroutine(IEUseShield());
        }

        private IEnumerator IEUseShield()
        {
            hasShield = false;
            currentInvulnerabilityTime = ANIMATION_TIME;

            while(currentInvulnerabilityTime > 0)
            {
                currentInvulnerabilityTime -= Time.unscaledDeltaTime;

                float t = currentInvulnerabilityTime / ANIMATION_TIME;
                transform.localScale = 2 * Mathf.Lerp(2, SHIELD_SIZE, 1 - t) * Vector3.one;

                Color clr = sprite.color;
                clr.a = t;
                sprite.color = clr;


                foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, SHIELD_SIZE * t))
                {
                    if (collider.CompareTag("Killer"))
                    {
                        Destroy(collider.gameObject);
                    }
                }

                yield return null;
            }
        }
    }
}