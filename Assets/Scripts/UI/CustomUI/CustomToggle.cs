using System;
using UnityEngine;

namespace InGame.UI.Custom
{
    public class CustomToggle : MonoBehaviour
    {
        public Animator animator;
        public Action<bool> OnStateChanged;

        public bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                _isOn = value;

                PlayAnimation(_isOn, false);
                OnStateChanged(_isOn);
            }
        }
        private bool _isOn;


        private void Awake()
        {
            //GlobalEvents.onSaveDataLoaded += Start;
        }
        private void Start()
        {
            SetIsOnWithoutAnimation(IsOn);
        }

        public void SetIsOnWithoutAnimation(bool isOn)
        {
            _isOn = isOn;
            PlayAnimation(isOn, true);
        }
        public void OnToggleClick()
        {
            IsOn = !IsOn;
        }

        private void PlayAnimation(bool isBecameOn, bool force)
        {
            animator.Play("Switch" + (isBecameOn ? "On" : "Off") + (force ? "Force" : ""));
        }
    }
}