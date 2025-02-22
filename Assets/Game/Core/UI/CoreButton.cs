using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class CoreButton : MonoBehaviour
    {
        public event Action OnClick;
        public event Action OnDisableInteractable;
        public event Action OnEnableInteractable;

        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(Click);
        }

        public void Disable()
        {
            _button.gameObject.SetActive(false);
        }

        public void Enable()
        {
            _button.gameObject.SetActive(true);
        }

        public void EnableInteractable(bool withCallback = true)
        {
            _button.interactable = true;

            if (withCallback)
            {
                OnEnableInteractable?.Invoke();
            }
        }

        public void DisableInteractable(bool withCallback = true)
        {
            _button.interactable = false;

            if (withCallback)
            {
                OnDisableInteractable?.Invoke();
            }
        }

        private void Click()
        {
            OnClick?.Invoke();
        }
    }
}
