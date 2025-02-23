using System;
using UnityEngine;

namespace Core.World
{
    [Serializable]
    public class DayData
    {
        [SerializeField] private int _demons;
        [SerializeField] private int _humans;
        [SerializeField] private int _quota;

        public int Demons => _demons;
        public int Humans => _humans;
        public int Quota => _quota;
    }
}
