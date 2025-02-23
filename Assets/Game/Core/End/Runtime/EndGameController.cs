using Core.Common;
using Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.End
{
    [Order(10)]
    public class EndGameController : MonoBehaviour, IControllerEntity
    {
        [SerializeField] private GameObject _endWindow;
        [SerializeField] private CoreButton _endButton;
        [Inject] private EventManager _eventManager;

        public void Init()
        {
            _endButton.OnClick += RestartGame;
        }

        public void PreInit()
        {
            _eventManager.Subscribe<LooseGameSignal>(this, EndGame);
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(0);
        }

        private void EndGame()
        {
            _endWindow.SetActive(true);
        }

        private void OnDestroy()
        {
            _eventManager.Unsubscribe<LooseGameSignal>(this);
        }
    }
}
