using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core.PlayerExpirience
{
    public class PassportView : Document
    {
        [SerializeField] private SpriteRenderer _faceSprite;
        [SerializeField] private TMP_Text _burthDataText;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _passportNumber;

        private Sequence _openSequence;
        private float _startYPos;

        private bool _isOpen;

        private void Awake()
        {
            _startYPos = transform.position.y;
        }

        public void SetPassportInfo(Sprite sprite, string nme, string passportNumber, int day, int month, int year)
        {
            _name.text = nme;
            _passportNumber.text = passportNumber;
            _burthDataText.text = string.Format("{0:D2}.{1:D2}.{2:D4}", day, month, year);
            _faceSprite.sprite = sprite;
        }

        private void OnMouseDown()
        {
            if (!_isOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        private void Open()
        {
            _openSequence?.Kill();
            _openSequence = DOTween.Sequence();
            _isOpen = true;

            _openSequence.Append(transform.DOScale(new Vector3(1,1,1), 0.5f));
            _openSequence.Append(transform.DOLocalMoveY(_startYPos + 1f, 0.5f));
        }

        private void Close()
        {
            _openSequence?.Kill();
            _openSequence = DOTween.Sequence();
            _isOpen = false;

            _openSequence.Append(transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f));
            _openSequence.Append(transform.DOLocalMoveY(_startYPos, 0.5f));
        }
    }
}
