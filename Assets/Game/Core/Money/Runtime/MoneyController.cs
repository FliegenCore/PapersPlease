using TMPro;
using UnityEngine;

namespace Core.Money
{
    [Order(0)]
    public class MoneyController : MonoBehaviour, IControllerEntity
    {
        [SerializeField] private TMP_Text _moneyText;
        public int CurrentMoney;

        public void PreInit()
        {

        }

        public void Init()
        {
            _moneyText.text = CurrentMoney.ToString() + "$";
        }

        public void AddMoney(int money)
        {
            CurrentMoney += money;

            _moneyText.text = CurrentMoney.ToString() + "$";
        }

        public void RemoveMoney(int money)
        {
            CurrentMoney -= money;

            _moneyText.text = CurrentMoney.ToString() + "$";
        }
    }
}
