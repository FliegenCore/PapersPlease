using Core.Common;
using Core.Entities;
using Core.Fade;
using Core.PlayerExpirience;
using Core.UI;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Core.World
{
    [Order(-999)]
    public class DayController : MonoBehaviour, IControllerEntity
    {
        public event Action OnDayChanged;
        public event Action OnLastDay;

        [SerializeField] private MouseDownClick _calendarButton;
        [SerializeField] private TMP_Text _dayText;

        [Inject] private CharactersController _charactersController;
        [Inject] private QuotaController _quotaController;
        [Inject] private EventManager _eventManager;
        [Inject] private WriterController _writerController;

        private const int LAST_QUOTA_DAY = 3;
        private const int LAST_GAME_DAY = 6;

        private int _currentQuotaDay;

        public DayData DayData;
        public int CurrentDay;

        public void PreInit()
        {
            _calendarButton.OnClick += EndDay;
        }

        public void Init()
        {
            ChangeDay();
        }

        private void ChangeDay()
        {
            CurrentDay++;
            _currentQuotaDay++;
            _dayText.text = $"Day: {CurrentDay}";

            DayData = JsonUtility.FromJson<DayData>(Resources.Load<TextAsset>($"Days\\Day{CurrentDay}").text);

            OnDayChanged?.Invoke();

            _eventManager.TriggerEvenet<UnfadeSignal, Action>(null);
        }

        private void EndDay()
        {
            if (_charactersController.CharactersLeft > 0)
            {
                _writerController.WirteText("Many more visitors, get to work");
                return;
            }

            StartCoroutine(WaitEndDay());
        }

        private IEnumerator WaitEndDay()
        {
            if (_currentQuotaDay >= LAST_QUOTA_DAY)
            {
                _currentQuotaDay = 0;
                OnLastDay?.Invoke();
                if (_quotaController.Loose)
                {
                    yield break;
                }
            }

            _eventManager.TriggerEvenet<FadeSignal, Action>(() =>
            {
                _eventManager.TriggerEvenet<HideCoreCanvasSignal>();
                _eventManager.TriggerEvenet<ShowHotelCanvasSignal>();
                _eventManager.TriggerEvenet<SetCameraXPosition, float>(50);
                ChangeDay();
            });
        }

        private void OnDestroy()
        {
            
        }
    }
}
