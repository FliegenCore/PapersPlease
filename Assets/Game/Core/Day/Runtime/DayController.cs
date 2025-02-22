using Core.Common;
using Core.Fade;
using System;
using TMPro;
using UnityEngine;

namespace Core.World
{
    [Order(-999)]
    public class DayController : MonoBehaviour, IControllerEntity
    {
        public event Action OnDayChanged;

        [SerializeField] private TMP_Text _dayText;
        [Inject] private EventManager _eventManager;

        private const int LAST_DAY = 3;

        public int CurrentDay;

        public void PreInit()
        {

        }

        public void Init()
        {
            ChangeDay();
        }

        public void ChangeDay()
        {
            //add fade

            

            _eventManager.TriggerEvenet<UnfadeSignal, Action>(null);
            _dayText.text = $"Day {CurrentDay + 1}";

            OnDayChanged?.Invoke();

            if (CurrentDay >= LAST_DAY)
            {
                //check kwota
                return;
            }
        }

        public void EndDay()
        { 
            
        }

        private void OnDestroy()
        {
            
        }
    }
}
