using UnityEngine;

namespace InGame.Level
{
    public class MovingPlatform : MonoBehaviour
    {
        public float width = -1;
        public float speed;
        public bool leftIsStartDirection;

        private float currentTarget, currentSpeed;

        public Transform platform;
        public Transform leftEdge, rightEdge, slider;

        private void OnValidate()
        {
            if (SizeablePlatform.IsPrefab(gameObject)) return;

            SpriteRenderer sliderSprite = slider.GetComponent<SpriteRenderer>();
            if (width < 0)
            {
                width = sliderSprite.size.x;
            }

            leftEdge.transform.localPosition = new Vector3(-width / 2f, 0);
            rightEdge.transform.localPosition = new Vector3(width / 2f, 0);

            
            sliderSprite.size = new Vector2(width, .3f);
        }
        private void Start()
        {
            leftEdge.localPosition = new Vector3(-width / 2f, 0);
            rightEdge.localPosition = new Vector3(width / 2f, 0);

            slider.localPosition = Vector3.zero;
            slider.localScale = new Vector3(width, 0.1f);
            slider.GetComponent<SpriteRenderer>().size = new Vector2(1, 1);

            currentSpeed = speed;
            currentTarget = width / 2f - platform.GetComponent<SpriteRenderer>().size.x / 2f;
            currentTarget *= leftIsStartDirection ? -1 : 1;
        }

        private void Update()
        {
            if (platform == null) return;


            platform.localPosition += new Vector3(currentSpeed * Time.deltaTime, 0);

            if (currentTarget > 0)
            {
                if (platform.localPosition.x >= currentTarget)
                {
                    currentTarget = -currentTarget;
                }
            }
            else
            {
                if (currentTarget >= platform.localPosition.x)
                {
                    currentTarget = -currentTarget;
                }
            }
            

            currentSpeed = speed * (currentTarget > 0 ? 1 : -1);
        }
    }
}