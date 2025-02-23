using DG.Tweening;
using UnityEngine;

namespace Core.Money
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private Transform _moneyWindow;

        private Sequence _openSequece;

        private void OnMouseEnter()
        {
            _openSequece?.Kill();
            _openSequece = DOTween.Sequence();

            _openSequece.Append(_moneyWindow.DOScaleY(0.4623f, 0.3f));

        }

        private void OnMouseExit()
        {
            _openSequece?.Kill();
            _openSequece = DOTween.Sequence();

            _openSequece.Append(_moneyWindow.DOScaleY(0, 0.3f));
        }
    }
}
