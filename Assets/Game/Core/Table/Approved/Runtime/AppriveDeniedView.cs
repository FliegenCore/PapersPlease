using Core.UI;
using UnityEngine;

namespace Core.PlayerExpirience
{
    public class AppriveDeniedView : MonoBehaviour
    {
        [SerializeField] private CoreButton _approvedButton;
        [SerializeField] private CoreButton _deniedButton;
        [SerializeField] private CoreButton _inviteButton;

        public CoreButton ApprovedButton => _approvedButton;
        public CoreButton DeniedButton => _deniedButton;
        public CoreButton InviteButton => _inviteButton;
    }
}
