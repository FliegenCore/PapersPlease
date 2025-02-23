using System;
using UnityEngine;

namespace Core.UI
{
    public class FloorButton : CoreButton
    {
        public event Action<int> OnChooseClick;

        public int Index;

        protected override void Click()
        {
            OnChooseClick?.Invoke(Index);
        }
    }
}
