using Core.Common;
using Core.End;
using Core.Money;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.World
{
    [Order(-1000)]
    public class QuotaController : MonoBehaviour, IControllerEntity
    {
        [Inject] private MoneyController _moneyController;
        [Inject] private DayController _dayController;
        [Inject] private EventManager _eventManager;
        [Inject] private WriterController _writerController;

        [SerializeField] private TMP_Text _quotaText;

        private int _quoutaCount;
        private bool _loose;

        public bool Loose => _loose;

        public void PreInit()
        {
            _dayController.OnLastDay += CheckQuota;
            _dayController.OnDayChanged += UpdateText;
        }

        public void Init()
        {
        }

        private void UpdateText()
        {
            _quotaText.text = $"Quota: {_dayController.DayData.Quota}";
        }

        private void CheckQuota()
        { 
            StartCoroutine(WaitCheckQuota());
        }

        private IEnumerator WaitCheckQuota()
        {
            _quoutaCount++;

            if (_moneyController.CurrentMoney >= _dayController.DayData.Quota)
            {
                _moneyController.RemoveMoney(_dayController.DayData.Quota);
                _writerController.WirteText($"Excellent, we paid {_dayController.DayData.Quota}$");
                yield return new WaitForSeconds(2f);
                if (_quoutaCount >= 2)
                {
                    yield return new WaitForSeconds(2f);
                    SceneManager.LoadScene(1);
                }
            }
            else
            {
                _loose = true;
                _eventManager.TriggerEvenet<LooseGameSignal>();
            }
        }

        private void OnDestroy()
        {
            _dayController.OnLastDay -= CheckQuota;
        }
    }
}
