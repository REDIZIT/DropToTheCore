using InGame.Secrets;
using InGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Menu
{
    public class UpgradeUI : MonoBehaviour
    {
        public Text coinsText;
        public UpgradeSlider gravitySlider, jumpSlider;


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
                }
            };
        }

        private void Update()
        {
            coinsText.text = SecretsManager.Secrets.Coins.ToString();
        }
    }
}