using UnityEngine;
using YG;

namespace InGame
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DeviceEnabler : MonoBehaviour
    {
        [SerializeField] private bool mobileOnly;

        private CanvasGroup group;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }
        private void Update()
        {
            bool enable;
            if (YandexGame.EnvironmentData.deviceType == "mobile")
            {
                enable = mobileOnly;
            }
            else
            {
                enable = mobileOnly == false;
            }

            group.alpha = enable ? 1 : 0;
            group.interactable = enable;
            group.blocksRaycasts = enable;
        }
    }
}