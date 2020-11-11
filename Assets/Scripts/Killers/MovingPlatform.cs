using UnityEngine;

namespace InGame.Level
{
    public class MovingPlatform : MonoBehaviour
    {
        public float width = -1;
        public float speed;
        public bool leftIsStartDirection;

        [Range(-1, 1)]
        public float platfromPosition;

        private float currentTarget, currentSpeed;
        private float platformAvailableWidth;

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

            // Checking
            //sliderSprite.size = new Vector2(width, .3f);

            platformAvailableWidth = width - platform.GetComponent<SizeablePlatform>().size.x;

            Update();
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

            platformAvailableWidth = width - platform.GetComponent<SizeablePlatform>().size.x;
        }

        private void Update()
        {
            if (platform == null) return;

            
            platform.localPosition = new Vector3(platfromPosition * platformAvailableWidth / 2f, 0);


            if (Application.isEditor && !Application.isPlaying) return;


            platfromPosition += currentSpeed * Time.deltaTime * 3f / platformAvailableWidth;

            if ((platfromPosition >= 1 && currentTarget >= 0) || (platfromPosition <= -1 && currentTarget < 0))
            {
                currentTarget = -currentTarget;
            }
            
            currentSpeed = speed * (currentTarget > 0 ? 1 : -1);
        }
    }
}