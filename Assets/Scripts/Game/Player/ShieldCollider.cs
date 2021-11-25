using System.Collections;
using TMPro;
using UnityEngine;

namespace InGame.Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ShieldCollider : MonoBehaviour
    {
        public float currentInvulnerabilityTime;


        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer timeLeftGroup;
        [SerializeField] private TextMeshPro timeLeftText;

        private bool hasShield;
        private float useTimeLeft;
        private SpriteRenderer sprite;

        private const float SHIELD_SIZE = 60;
        private const float ANIMATION_TIME = 0.35f;
        private const float DEFAULT_USE_TIME = 10;


        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            if (currentInvulnerabilityTime > 0) currentInvulnerabilityTime -= Time.deltaTime;

            if (useTimeLeft > 0) useTimeLeft -= Time.deltaTime;
            else hasShield = false;

            timeLeftText.alpha = useTimeLeft > 0 ? 1 : 0;
            timeLeftText.text = Mathf.CeilToInt(useTimeLeft).ToString();
            timeLeftGroup.color = new Color(timeLeftGroup.color.r, timeLeftGroup.color.g, timeLeftGroup.color.b, timeLeftText.alpha);
        }
        public void Charge()
        {
            hasShield = true;
            useTimeLeft = DEFAULT_USE_TIME;
        }
        public bool TryUse()
        {
            if (hasShield == false) return false;
            StartCoroutine(IEUseShield());
            return true;
        }

        private IEnumerator IEUseShield()
        {
            hasShield = false;
            useTimeLeft = 0;

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