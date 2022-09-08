using InGame.Game;
using InGame.Secrets;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
        public AudioSource audioSource;
        public AudioClip jumpAudioClip;

        public ParticleSystem deathParticles;
        public SpriteRenderer spriteRenderer;
        public Animator deathAnimator;


        public ShieldCollider shield;
        public Action onJump;


        [HideInInspector] public new Rigidbody2D rigidbody;

        private void Awake()
        {
            instance = this;
            rigidbody = GetComponent<Rigidbody2D>();

            rigidbody.gravityScale += rigidbody.gravityScale * SecretsManager.Secrets.GravityPower / 8f;
            jumpPower += jumpPower * SecretsManager.Secrets.JumpPower / 5f;
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
        private void Jump()
        {
            onJump?.Invoke();

            float percents = Input.mousePosition.x / (float)Screen.width;
            float normalized = percents - 0.5f;

            rigidbody.velocity = (1 / 60f) * jumpPower * Vector2.up;

            rigidbody.velocity += (1 / 60f) * normalized * strafePower * Vector2.right;


            GameObject effect = Instantiate(jumpEffectPrefab);
            effect.transform.position = transform.position;
            Destroy(effect, 3);
            audioSource.PlayOneShot(jumpAudioClip);
        }

        public void Revive()
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
            }
        }
    }
}
