using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI.Game
{
	[RequireComponent(typeof(Text), typeof(CanvasGroup))]
    [ExecuteInEditMode]
	public class UnpauseCountdownText : MonoBehaviour
	{
        public bool IsCounted => timeLeft <= 0;

		private Text text;
        private CanvasGroup group;
        private float timeLeft;

        private void Awake()
        {
            text = GetComponent<Text>();
            group = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (timeLeft <= 0) return;

            timeLeft -= Time.unscaledDeltaTime * 1.75f;

            if (timeLeft > 0)
            {
                int ceil = Mathf.CeilToInt(timeLeft);
                text.text = ceil.ToString();
                transform.localScale = Vector3.one * Mathf.Lerp(0.85f, 1.95f, timeLeft % 1f);

                group.alpha = 1;
            }
            else
            {
                group.alpha = 0;
            }
        }

        public void StartCountdown()
        {
            timeLeft = 3;
        }
    }
}