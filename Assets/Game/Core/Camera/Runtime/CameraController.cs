using Core.Common;
using UnityEngine;

namespace Core.World
{
    [Order(-100)]
    public class CameraController : MonoBehaviour, IControllerEntity
    {
        [Inject] private EventManager _eventManager;

        public void PreInit()
        {
            _eventManager.Subscribe<SetCameraXPosition, float>(this, SetPositionX);
        }

        public void Init()
        {
        }

        private void SetPositionX(float xPos)
        {
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }

        private void OnDestroy()
        {
            _eventManager.Unsubscribe<SetCameraXPosition>(this);
        }
    }
}
