using InGame.Secrets;
using InGame.Settings;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class UpgradeSlider : MonoBehaviour
    {
        public int startIndex;


        public int Value
        {
            get { return _value; }
            set { _value = Mathf.Clamp(value, MinValue, MaxValue); Refresh(); }
        }
        private int _value;

        
        public int MaxValue { get; private set; }
        public int MinValue { get; private set; }
        public int costMultiplier;

        public Action<int, int> OnValueChanged { get; set; }
        public Action<int> OnButtonClicked { get; set; }



        [SerializeField] private Image[] images;
        [SerializeField] private Button minusBtn, plusBtn;
        [SerializeField] private CostText minusCostText, plusCostText;
        [SerializeField] private Color lowColor, highColor;
        [SerializeField] private Color lowHighlightedColor, highHighlightedColor;





        private void Start()
        {
            Refresh();
        }
        private void OnValidate()
        {
            Refresh();
        }


        public void Refresh()
        {
            int halfLength = Mathf.FloorToInt(images.Length / 2f);
            MinValue = -(halfLength + startIndex);
            MaxValue = halfLength - startIndex;

            for (int i = 0; i < images.Length; i++)
            {
                int ii = i - startIndex;
                int relativeI = ii - halfLength;
                bool isHighlighted = IsInRange(relativeI);

                if (ii < halfLength)
                {
                    images[i].color = isHighlighted ? lowHighlightedColor : lowColor;
                }
                else if (ii > halfLength)
                {
                    images[i].color = isHighlighted ? highHighlightedColor : highColor;
                }
                else
                {
                    images[i].color = Color.white;
                }
            }



            plusBtn.interactable = Value < MaxValue;
            plusCostText.gameObject.SetActive(Value < MaxValue);
            minusBtn.interactable = Value > MinValue;
            minusCostText.gameObject.SetActive(Value > MinValue);

            int cost = (Mathf.Abs(Value) + 1) * costMultiplier;
            if (cost > SecretsManager.Secrets.Coins)
            {
                plusBtn.interactable = false;
                minusBtn.interactable = false;
            }

            int absValue = Mathf.Abs(Value);
            plusCostText.cost = (Mathf.Pow(absValue + 1, Value > 0 ? 2 : 1)) * costMultiplier;
            minusCostText.cost = (Mathf.Pow(absValue + 1, Value < 0 ? 2 : 1)) * costMultiplier;
            plusCostText.Refresh();
            minusCostText.Refresh();
        }

        public void SetValueWithoutNotify(int value)
        {
            _value = value;
            Refresh();
        }

        public void OnPlusClick()
        {
            OnValueChanged?.Invoke(Value + 1, (int)plusCostText.cost);
            Value += 1;
        }
        public void OnMinusClick()
        {
            OnValueChanged?.Invoke(Value - 1, (int)minusCostText.cost);
            Value -= 1;
        }


        private bool IsInRange(int i)
        {
            int min = Mathf.Min(0, Value);
            int max = Mathf.Max(0, Value);

            return i >= min && i <= max;
        }
    }
}