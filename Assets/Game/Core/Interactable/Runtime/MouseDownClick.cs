using System;
using UnityEngine;

namespace Core.PlayerExpirience
{
    public class MouseDownClick : MonoBehaviour
    {
        public event Action OnClick;

        private void OnMouseDown()
        {
            OnClick?.Invoke();
        }
    }
}
