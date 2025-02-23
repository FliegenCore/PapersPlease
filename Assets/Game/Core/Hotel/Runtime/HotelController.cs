using Core.Entities;
using Core.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core.Hotel
{
    public struct MyValues
    {
        public object Value;
        public object Value2;
    }

    [Order(200)]
    public class HotelController : MonoBehaviour, IControllerEntity
    {
        public event Action OnRoomChoosed;

        [SerializeField] private Transform _chooseFloorView;
        [SerializeField] private TMP_Text _chooseText;
        [SerializeField] private FloorButton[] _floorButtons;

        [Inject] private CharactersController _charactersController;

        private Dictionary<(int flor, int roms), CharacterType> _livers = new Dictionary<(int floor, int rooms), CharacterType>();
        private int[,] _rooms;

        private int _currentChoosedFloor;
        private int _currentChoosedRoom;

        private bool _chooseFloor;

        public Dictionary<(int flor, int roms), CharacterType> Livers => _livers;

        public void PreInit()
        {
            _chooseFloor = true;
            _currentChoosedFloor = -1;
            _currentChoosedRoom = -1;
            _rooms = new int[6, 6];

            foreach (var button in _floorButtons)
            {
                button.OnChooseClick += Choose;
            }

            DeleteFloor(0);
        }

        public void Init()
        {
        }

        public void EnableChooseFreeFloor()
        {
            _chooseFloorView.gameObject.SetActive(true);
            _chooseText.text = "Choose floor:";

            List<int> freeFloor = GetFloorsWithFreeRooms();

            foreach (var button in _floorButtons)
            {
                button.Disable();
            }

            foreach (var free in freeFloor)
            {
                _floorButtons[free].Enable();
            }
        }

        public void DeleteFloor(int floor)
        {
            for (int i = 0; i < _rooms.GetLength(1); i++)
            {
                _rooms[floor, i] = -1;

                var key = (floor, i);

                if(_livers.ContainsKey(key))
                {
                    _livers.Remove(key);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                EnableChooseFreeFloor();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                foreach (var info in _livers)
                {
                    Debug.Log($"{info.Key}   {info.Value}");
                }
            }
        }
#endif
        private void AddLiver(CharacterType type, int floor, int room)
        {
            _rooms[floor, room] = 1;
            _chooseFloor = true;
            _chooseFloorView.gameObject.SetActive(false);

            _livers[(floor, room)] = type;
            OnRoomChoosed?.Invoke();
        }

        private void Choose(int index)
        {
            Debug.Log(index);
            if (_chooseFloor)
            {
                _currentChoosedFloor = index;
                _chooseFloor = false;
                EnableChooseFreeRoom();
            }
            else
            {
                _currentChoosedRoom = index;

                AddLiver(_charactersController.CharacterType, _currentChoosedFloor, _currentChoosedRoom);
            }
        }

        private void EnableChooseFreeRoom()
        {
            _chooseText.text = "Choose room:";

            List<int> freeRoom = GetFreeRooms();

            foreach (var button in _floorButtons)
            {
                button.Disable();
            }

            foreach (var free in freeRoom)
            {
                _floorButtons[free].Enable();
            }
        }

        private List<int> GetFloorsWithFreeRooms()
        {
            List<int> floorsWithFreeRooms = new List<int>();

            for (int i = 0; i < _rooms.GetLength(0); i++)
            {
                bool hasFreeRoom = false;

                for (int j = 0; j < _rooms.GetLength(1); j++) 
                {
                    if (_rooms[i, j] == -1)
                    {
                        break;
                    }

                    if (_rooms[i, j] == 0)
                    {
                        hasFreeRoom = true;
                        break; 
                    }
                }

                if (hasFreeRoom)
                {
                    floorsWithFreeRooms.Add(i);
                }
            }

            return floorsWithFreeRooms;
        }

        private List<int> GetFreeRooms()
        {
            List<int> freeRooms = new List<int>();
            for (int i = 0; i < _rooms.GetLength(1); i++)
            {
                if (_rooms[_currentChoosedFloor, i] == 0)
                {
                    freeRooms.Add(i); 
                }
            }
            return freeRooms;
        }
    }
}
