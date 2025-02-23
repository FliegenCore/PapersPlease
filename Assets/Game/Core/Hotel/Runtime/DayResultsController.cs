using Core.Common;
using Core.Entities;
using Core.Fade;
using Core.Hotel;
using Core.Money;
using Core.UI;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core.World
{
    [Order(999)]
    public class DayResultsController : MonoBehaviour, IControllerEntity
    {
        [SerializeField] private TMP_Text _humansInHotelText;
        [SerializeField] private TMP_Text _moneyIncomeTodayText;
        [SerializeField] private TMP_Text _daysLivedText;
        [SerializeField] private TMP_Text _demonsCountText;
        [SerializeField] private CoreButton _coreButton;
        [SerializeField] private RectTransform _window;

        [SerializeField] private GameObject _fire;
        [SerializeField] private GameObject[] _floorsView;
        [SerializeField] private GameObject[] _floorsBlackView;

        [SerializeField] private float[] _floorHeight;

        [Inject] private HotelController _hotelController;
        [Inject] private MoneyController _moneyController;
        [Inject] private DayController _dayController;
        [Inject] private EventManager _eventManager;

        private int _moneyCount;

        public void PreInit()
        {
            _dayController.OnDayChanged += StartCalculateResults;
            _coreButton.OnClick += Click;
            _window.anchoredPosition = new Vector3(350, 0, 0);
        }


        public void Init()
        {
        }

        public void StartCalculateResults()
        {
            _humansInHotelText.text = $"Humans in hotel: {0}";
            _moneyIncomeTodayText.text = $"Money for today: {0}$";
            _daysLivedText.text = $"Days lived: {0}";
            _demonsCountText.text = $"Demons in hotel: {0}";

            _window.DOAnchorPosX(-280f, 0.5f).OnComplete(() =>
            {
                StartCoroutine(SetHumansInHotel(() =>
                {
                    StartCoroutine(CalculateMoney(() =>
                    {
                        StartCoroutine(SetDayLived(() =>
                        {
                            StartCoroutine(SetDemonsText(() =>
                            {


                                StartCoroutine(SetHumansInHotel(() =>
                                {
                                    StartCoroutine(CalculateMoney(() =>
                                    {
                                        StartCoroutine(SetDayLived(() =>
                                        {
                                            StartCoroutine(SetDemonsText(() =>
                                            {
                                                _coreButton.Enable();
                                            }));
                                        }));
                                    }));
                                }));


                            }));
                        }));
                    }));
                }));
            });
            
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                StartCalculateResults();
            }
        }
#endif

        private IEnumerator SetHumansInHotel(Action action)
        {
            int liversCount = 0;

            _humansInHotelText.text = $"Humans in hotel: {liversCount}";

            foreach (var liver in _hotelController.Livers)
            {
                yield return new WaitForSeconds(0.1f);
                if (liver.Value == CharacterType.Human)
                {
                    liversCount++;
                    _humansInHotelText.text = $"Humans in hotel: {liversCount}";
                }
            }

            yield return new WaitForSeconds(1f);
            action?.Invoke();
        }

        private IEnumerator CalculateMoney(Action action)
        {
            int moneyCount = 0;

            _moneyIncomeTodayText.text = $"Money for today: {moneyCount}$";

            foreach (var liver in _hotelController.Livers)
            {
                yield return new WaitForSeconds(0.1f);
                if (liver.Value == CharacterType.Human)
                {
                    moneyCount += 100;
                    _moneyIncomeTodayText.text = $"Money for today: {moneyCount}$";
                }
            }
            _moneyCount = moneyCount;

            yield return new WaitForSeconds(1f);
            action?.Invoke();
        }

        private IEnumerator SetDayLived(Action action)
        {
            _daysLivedText.text = $"Days lived: {_dayController.CurrentDay-1}";
            yield return new WaitForSeconds(2f);

            action?.Invoke();
        }

        private IEnumerator SetDemonsText(Action action)
        {
            int liversCount = 0;
            _demonsCountText.text = $"Demons in hotel: {liversCount}";

            // Создаем временный список для хранения элементов, которые нужно удалить
            var demonsToRemove = new List<int>();

            int demons = 0;

            foreach (var liver in _hotelController.Livers)
            {
                if (liver.Value == CharacterType.Demon)
                {
                    demons++;
                }
                    
            }

            if (demons == 0)
            {
                _coreButton.Enable();
                yield break;
            }

            foreach (var liver in _hotelController.Livers)
            {
                yield return new WaitForSeconds(0.1f);
                if (liver.Value == CharacterType.Demon)
                {
                    liversCount++;
                    _demonsCountText.text = $"Demons in hotel: {liversCount}";
                    Debug.Log(liver.Key.flor);
                    _fire.transform.position = new Vector3(_fire.transform.position.x, _floorHeight[liver.Key.flor - 1]);
                    _fire.gameObject.SetActive(true);
                    yield return new WaitForSeconds(3f);

                    _fire.gameObject.SetActive(false);
                    _floorsBlackView[liver.Key.flor - 1].SetActive(true);

                    demonsToRemove.Add(liver.Key.flor);
                }
            }

            // Удаляем элементы после завершения итерации
            foreach (var key in demonsToRemove)
            {
                _hotelController.DeleteFloor(key);
            }

            action?.Invoke();

            //if floor count = 0 => game end
        }

        private void Click()
        {
            _moneyController.AddMoney(_moneyCount);

            _window.anchoredPosition = new Vector3(350, 0, 0);
            _coreButton.Disable();
            _moneyCount = 0;

            _eventManager.TriggerEvenet<FadeSignal, Action>(() =>
            {
                _eventManager.TriggerEvenet<SetCameraXPosition, float>(0);
                _eventManager.TriggerEvenet<HideHotelCanvasSignal>();
                _eventManager.TriggerEvenet<ShowCoreCanvasSignal>();

                _eventManager.TriggerEvenet<UnfadeSignal, Action>(null);
            });
        }

        private void OnDestroy()
        {

        }
    }
}
