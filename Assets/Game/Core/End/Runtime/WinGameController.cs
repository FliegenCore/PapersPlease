using Core.Common;
using Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.End
{
    [Order(100)]
    public class WinGameController : MonoBehaviour, IControllerEntity
    {
        [SerializeField] private GameObject _winWindow;
        [SerializeField] private CoreButton _openWinSceneButton;

        [Inject] private EventManager _eventManager;

        public void PreInit()
        {
            _openWinSceneButton.OnClick += OpenWinScene;
            _eventManager.Subscribe<ShowWinWindowSignal>(this, OnWin);
        }

        public void Init()
        {
        }

        private void OnWin()
        { 
            _winWindow.SetActive(true);
        }

        private void OpenWinScene()
        {
            SceneManager.LoadScene(1);
        }

        private void OnDestroy()
        {
            _eventManager.Unsubscribe<ShowWinWindowSignal>(this);
        }
    }
}
