using Core.Common;
using UnityEngine;

namespace Core.UI
{
    [Order(-9999)]
    public class CoreUIController : MonoBehaviour, IControllerEntity
    {
        [SerializeField] private Canvas _coreCanvas;
        [SerializeField] private Canvas _hotelCanvas;

        [Inject] private EventManager _eventManager;

        public void PreInit()
        {

        }

        public void Init()
        {
            _eventManager.Subscribe<HideCoreCanvasSignal>(this, HideCoreCanvas);
            _eventManager.Subscribe<HideHotelCanvasSignal>(this, HideHotelCanvas);
            _eventManager.Subscribe<ShowCoreCanvasSignal>(this, ShowCoreCanvas);
            _eventManager.Subscribe<ShowHotelCanvasSignal>(this, ShowHotelCanvas);
        }

        private void HideCoreCanvas()
        {
            _coreCanvas.enabled = false;
        }

        private void HideHotelCanvas()
        { 
            _hotelCanvas.enabled = false;
        }

        private void ShowCoreCanvas()
        {
            _coreCanvas.enabled = true;
        }

        private void ShowHotelCanvas()
        {
            _hotelCanvas.enabled = true;
        }

        private void OnDestroy()
        {
            _eventManager.Unsubscribe<HideCoreCanvasSignal>(this);
            _eventManager.Unsubscribe<HideHotelCanvasSignal>(this);
            _eventManager.Unsubscribe<ShowCoreCanvasSignal>(this);
            _eventManager.Unsubscribe<ShowHotelCanvasSignal>(this);
        }
    }
}
