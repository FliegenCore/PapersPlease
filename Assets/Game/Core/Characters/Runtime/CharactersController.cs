using Core.Common;
using Core.PlayerExpirience;
using System;
using UnityEngine;

namespace Core.Entities
{
    [Order(1)]
    public class CharactersController : MonoBehaviour, IControllerEntity
    {
        public event Action OnEnterInRoom;
        public event Action OnApproved;
        public event Action OnDenied;

        [SerializeField] private Character _currentCharacter;

        [Inject] private TableController _tableController;
        [Inject] private EventManager _eventManager;
        [Inject] private DocumentsController _documentsController;

        private int _characteresLeft;

        public int CharactersLeft => _characteresLeft;

        public void PreInit()
        {
            _tableController.AppriveDeniedView.ApprovedButton.OnClick += ApprovedCurrentCharacter;
            _tableController.AppriveDeniedView.DeniedButton.OnClick += DeniedCurrentCharacter;
            _tableController.AppriveDeniedView.InviteButton.OnClick += ChangeCharacter;

            _characteresLeft = 2;
        }

        public void Init()
        {
            _currentCharacter.Initialize();
        }

        public void SetCharactersCount(int count)
        {
            _characteresLeft = count;
        }

        public void DeniedCurrentCharacter()
        {
            _currentCharacter.Denied(OnDenied);
        }

        public void ApprovedCurrentCharacter()
        {
            _currentCharacter.Approved(OnApproved);
        }

        private void ChangeCharacter()
        {
            PassportData passportData = _documentsController.LoadPassport();

            if (passportData == null)
            {
                return;
            }

            string nme = passportData.Name.Split('\n')[0].Trim('"');

            _currentCharacter.SetSprite(LoadCharacterSprite(nme));

            _currentCharacter.InviteInRoom(() =>
            {
                OnEnterInRoom?.Invoke();
                _eventManager.TriggerEvenet<OnApproveDeniedButtonsSignal>();
            });
        }

        public static Sprite LoadCharacterSprite(string nme)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprites\\Characters\\{nme}");

            if (sprite == null)
            {
                return null;
            }
            
            return sprite;
        }

        private void OnDestroy()
        {
            _tableController.AppriveDeniedView.ApprovedButton.OnClick -= ApprovedCurrentCharacter;
            _tableController.AppriveDeniedView.DeniedButton.OnClick -= DeniedCurrentCharacter;
        }
    }
}

