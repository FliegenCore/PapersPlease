using Core.World;
using UnityEngine;

namespace Core.Entities
{
    [Order(2)]
    public class HeadController : MonoBehaviour, IControllerEntity
    {
        [Inject] private CharactersController _characterController;
        [Inject] private MagnifyingGlassController _glassController;

        public void PreInit()
        {
        }

        public void Init()
        {
        }

        private void Update()
        {
            if(Input.GetMouseButtonUp(0))
            {
                CheckFactor();
            }
        }

        private void CheckFactor()
        {
            if (_glassController.WithLoop)
            {
                float distance = (transform.position - _glassController.transform.position).sqrMagnitude;
                if (distance <= 2)
                {
                    _characterController.CheckFactor();
                }
            }
        }
    }
}
