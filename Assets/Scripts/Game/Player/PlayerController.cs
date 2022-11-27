using InGame.Analytics;
using InGame.Game;
using InGame.Secrets;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InGame
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        public float Depth => -transform.position.y;

        public float jumpPower;
        public float strafePower;
        public bool canMove = true;

        public TrailRenderer trail;

        public GameObject jumpEffectPrefab;
        public AudioYB audioSource;
        public string jumpAudioClipName;

        public ParticleSystem deathParticles;
        public SpriteRenderer spriteRenderer;
        public Animator deathAnimator;


        public ShieldCollider shield;
        public Action onJump;


        [HideInInspector] public new Rigidbody2D rigidbody;

        private CoreAnalytics analytics;
        private CameraController cam;


        [Inject]
        private void Construct(CoreAnalytics analytics, CameraController cam)
        {
            this.analytics = analytics;
            this.cam = cam;
        }
        private void Awake()
        {
            instance = this;
            rigidbody = GetComponent<Rigidbody2D>();

            InitStats();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && Time.timeScale != 0)
            {
                if ((GameManager.instance == null || GameManager.instance.isAlive) && canMove)
                {
                    Jump();
                }
            }
        }
        private void InitStats()
        {
            rigidbody.gravityScale = 8 + rigidbody.gravityScale * SecretsManager.Secrets.GravityPower / 8f;
            jumpPower = 1000 + 1000 * SecretsManager.Secrets.JumpPower / 5f;
        }
        private void Jump()
        {
            onJump?.Invoke();

            float blackBorderWidth = (Screen.width - cam.ScreenWidth) / 2f;
            float clickX = Mathf.Clamp(Input.mousePosition.x - blackBorderWidth, 0, cam.ScreenWidth);

            float percents = clickX / (float)cam.ScreenWidth;
            float normalized = percents - 0.5f;

            rigidbody.velocity = (1 / 60f) * jumpPower * Vector2.up;

            rigidbody.velocity += (1 / 60f) * normalized * strafePower * Vector2.right;


            GameObject effect = Instantiate(jumpEffectPrefab);
            effect.transform.position = transform.position;
            Destroy(effect, 3);
            audioSource.PlayOneShot(jumpAudioClipName);
        }

        public void Revive()
        {
            rigidbody.isKinematic = false;
            rigidbody.velocity = Vector2.zero;
            spriteRenderer.enabled = true;
            deathParticles.Clear();
            trail.Clear();

            InitStats();
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
            if (GameManager.instance == null)
            {
                transform.position = new Vector3(0, 0);
                trail.Clear();
                return;
            }
            if (!GameManager.instance.isAlive) return;
            if (shield.currentInvulnerabilityTime > 0) return;

            if (shield.TryUse() == false)
            {
                deathParticles.Play();
                deathAnimator.Play("DeathCircleEffect");

                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector2.zero;
                spriteRenderer.enabled = false;
                GameManager.instance.DeadActions();

                analytics.SendPlayerDeath((int)Depth);
            }
        }
    }
}
