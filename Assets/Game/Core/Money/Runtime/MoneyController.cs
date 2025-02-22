using UnityEngine;

namespace Core.Money
{
    [Order(0)]
    public class MoneyController : MonoBehaviour, IControllerEntity
    {
        [SerializeField] private int _currentMoney;

        public void PreInit()
        {

        }

        public void Init()
        {

        }

        public void AddMoney()
        { 
            
        }

        public void RemoveMoney()
        {

        }
    }
}
