using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class CostText : MonoBehaviour
    {
        public Text text;
        public RectTransform image;

        public float cost;

        public float spacing;

        private RectTransform textRect;


        private void OnValidate()
        {
            Refresh();
        }

        private void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (textRect == null) textRect = text.GetComponent<RectTransform>();

            text.text = cost.ToString();
            textRect.offsetMax = new Vector2(-image.sizeDelta.x - spacing, 0);
        }
    }
}