using InGame.Secrets;
using InGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Menu
{
    public class UpgradeUI : MonoBehaviour
    {
        public Text coinsText;
        public UpgradeSlider gravitySlider, jumpSlider, shieldSlider;


        private void Awake()
        {
            GlobalEvents.onSaveDataLoaded += Start;
        }
        private void Start()
        {
            gravitySlider.SetValueWithoutNotify(SecretsManager.Secrets.GravityPower);
            gravitySlider.OnValueChanged += (value, cost) =>
            {
                if (SecretsManager.Secrets.Coins >= cost)
                {
                    SecretsManager.Secrets.Coins -= cost;
                    SecretsManager.Secrets.GravityPower = value;
                    SecretsManager.Save();

                    RefreshSliders();
                }
            };

            jumpSlider.SetValueWithoutNotify(SecretsManager.Secrets.JumpPower);
            jumpSlider.OnValueChanged += (value, cost) =>
            {
                if (SecretsManager.Secrets.Coins >= cost)
                {
                    SecretsManager.Secrets.Coins -= cost;
                    SecretsManager.Secrets.JumpPower = value;
                    SecretsManager.Save();

                    RefreshSliders();
                }
            };

            shieldSlider.SetValueWithoutNotify(SecretsManager.Secrets.ShieldLevel);
            shieldSlider.OnValueChanged += (value, cost) =>
            {
                if (SecretsManager.Secrets.Coins >= cost)
                {
                    SecretsManager.Secrets.Coins -= cost;
                    SecretsManager.Secrets.ShieldLevel = value;
                    SecretsManager.Save();

                    RefreshSliders();
                }
            };
        }

        private void Update()
        {
            coinsText.text = SecretsManager.Secrets.Coins.ToString();
        }

        private void RefreshSliders()
        {
            gravitySlider.Refresh();
            jumpSlider.Refresh();
            shieldSlider.Refresh();
        }
    }
}