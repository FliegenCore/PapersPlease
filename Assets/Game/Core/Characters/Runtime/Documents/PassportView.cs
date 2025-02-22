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

        public void SetPassportInfo(Sprite sprite, string nme, string passportNumber, int day, int month, int year)
        {
            _name.text = nme;
            _passportNumber.text = passportNumber;
            _burthDataText.text = string.Format("{0:D2}.{1:D2}.{2:D4}", day, month, year);
            _faceSprite.sprite = sprite;
        }
    }
}
