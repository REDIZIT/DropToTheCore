using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    [RequireComponent(typeof(Text))]
    public class FPSText : MonoBehaviour
    {
        private Text text;

        private void Start()
        {
            text = GetComponent<Text>();
        }
        private void Update()
        {
            text.text = (int)(1f / Time.smoothDeltaTime) + " fps";
        }
    }
}