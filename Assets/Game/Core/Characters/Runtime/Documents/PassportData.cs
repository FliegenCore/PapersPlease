using System;
using UnityEngine;

namespace Core.Entities
{
    [Serializable]
    public class PassportData 
    {
        [SerializeField] private string _name;
        [SerializeField] private string _passportNumber;
        [SerializeField] private int _burthDay;
        [SerializeField] private int _burthMounth;
        [SerializeField] private int _burthYear;

        public string Name => _name;
        public string PassportNumber => _passportNumber;
        public int Year => _burthYear;
        public int Day => _burthDay;
        public int Mounth => _burthMounth;
    }
}
