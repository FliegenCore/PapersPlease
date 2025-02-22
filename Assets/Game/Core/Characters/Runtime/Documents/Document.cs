using Core.PlayerInput;
using UnityEngine;

namespace Core.PlayerExpirience
{
    public class Document : MonoBehaviour, IDragAndDrop
    {
        public void Drag()
        {
        }

        public void DragEnd()
        {
        }

        public void DragStart()
        {
        }

        public void Enable()
        { 
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

    }
}
