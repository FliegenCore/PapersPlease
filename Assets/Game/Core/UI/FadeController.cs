using Core.Common;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Fade
{
    [Order(-1000)]
    public class FadeController : MonoBehaviour, IControllerEntity
    {
        [SerializeField] private Image _fadeImage;

        [Inject] private EventManager _eventManager;

        public void PreInit()
        {
            _eventManager.Subscribe<FadeSignal, Action>(this, Fade);
            _eventManager.Subscribe<UnfadeSignal, Action>(this, Unfade);

            _fadeImage.DOFade(1, 0);
        }

        public void Init()
        {

        }

        private void Fade(Action action)
        {
            _fadeImage.DOFade(1, 2f).OnComplete(() =>
            {
                action?.Invoke();
            });   

        }

        private void Unfade(Action action)
        {
            _fadeImage.DOFade(0, 2f).OnComplete(() =>
            {
                action?.Invoke();
            });
        }

        private void OnDestroy()
        {
            _eventManager.Unsubscribe<FadeSignal>(this);
            _eventManager.Unsubscribe<UnfadeSignal>(this);
        }
    }
}
