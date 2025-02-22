using Core.Common;
using Core.Entities;
using UnityEngine;

namespace Core.PlayerExpirience
{
    [Order(-1)]
    public class TableController : MonoBehaviour, IControllerEntity
    {
        [SerializeField] private AppriveDeniedView _appriveDeniedView;

        [Inject] private EventManager _eventManager;
        [Inject] private CharactersController _charactersController;

        public AppriveDeniedView AppriveDeniedView => _appriveDeniedView;

        public void PreInit()
        {
            _appriveDeniedView.ApprovedButton.OnClick += OffButtonsInteractable;
            _appriveDeniedView.DeniedButton.OnClick += OffButtonsInteractable;
            _appriveDeniedView.InviteButton.OnClick += OffInviteInteractable;

            _charactersController.OnDenied += OnInviteInteractable;
            _charactersController.OnApproved += OnInviteInteractable;
        }

        public void Init()
        {
            _eventManager.Subscribe<OnApproveDeniedButtonsSignal>(this, OnButtonsInteractable);
        }

        private void OffButtonsInteractable()
        {
            _appriveDeniedView.ApprovedButton.DisableInteractable(true);
            _appriveDeniedView.DeniedButton.DisableInteractable(true);
        }

        private void OnButtonsInteractable()
        {
            _appriveDeniedView.ApprovedButton.EnableInteractable(true);
            _appriveDeniedView.DeniedButton.EnableInteractable(true);
        }

        private void OffInviteInteractable()
        {
            _appriveDeniedView.InviteButton.DisableInteractable(true);
        }

        private void OnInviteInteractable()
        {
            _appriveDeniedView.InviteButton.EnableInteractable(true);
        }

        private void OnDestroy()
        {
            _appriveDeniedView.ApprovedButton.OnClick -= OffButtonsInteractable;
            _appriveDeniedView.DeniedButton.OnClick -= OffButtonsInteractable;
            _appriveDeniedView.InviteButton.OnClick -= OffInviteInteractable;

            _charactersController.OnDenied -= OnInviteInteractable;
            _charactersController.OnApproved -= OnInviteInteractable;

            _eventManager.Unsubscribe<OnApproveDeniedButtonsSignal>(this);
        }
    }
}
