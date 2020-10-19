using InGame.Game;
using InGame.Game.Bonuses;
using UnityEngine;

namespace InGame
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        public float jumpPower;
        public float strafePower;

        public TrailRenderer trail;

        public GameObject jumpEffectPrefab;
        public AudioSource audioSource;
        public AudioClip jumpAudioClip;

        public ParticleSystem deathParticles;
        public SpriteRenderer spriteRenderer;
        public Animator deathAnimator;


        public ShieldCollider shield;
        


        [HideInInspector] public new Rigidbody2D rigidbody;

        private void Awake()
        {
            instance = this;
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.instance.isAlive)
                {
                    Jump();
                }
            }
        }
        private void Jump()
        {
            float percents = Input.mousePosition.x / (float)Screen.width;
            float normalized = percents - 0.5f;

            rigidbody.velocity = Vector2.up * jumpPower * (1 / 60f);

            rigidbody.velocity += Vector2.right * normalized * strafePower * (1 / 60f);


            GameObject effect = Instantiate(jumpEffectPrefab);
            effect.transform.position = transform.position;
            Destroy(effect, 3);
            audioSource.PlayOneShot(jumpAudioClip);
        }

        public void Relive()
        {
            rigidbody.isKinematic = false;
            rigidbody.velocity = Vector2.zero;
            spriteRenderer.enabled = true;
            deathParticles.Clear();
            trail.Clear();
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Killer"))
            {
                Die();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Killer"))
            {
                Die();
            }
        }

        private void Die()
        {
            if (!GameManager.instance.isAlive) return;
            if (shield.currentInvulnerabilityTime > 0) return;

            if (shield.hasShield)
            {
                shield.Use();
            }
            else
            {
                deathParticles.Play();
                deathAnimator.Play("DeathCircleEffect");

                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector2.zero;
                spriteRenderer.enabled = false;

                GameManager.instance.DeadActions();
            }
        }
    }
}
