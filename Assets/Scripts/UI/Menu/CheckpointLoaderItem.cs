using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Menu
{
    public class CheckpointLoaderItem : MonoBehaviour
    {
        public Text depthText;
        public float depth;

        private Action<float> onClickCallback;

        public void Refresh(float depth, Action<float> onClickCallback)
        {
            this.depth = depth;
            this.onClickCallback = onClickCallback;

            depthText.text = depth + "m";
        }

        public void OnClick()
        {
            onClickCallback?.Invoke(depth);
        }
    }
}