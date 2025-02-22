using UnityEngine;

namespace Core.PlayerInput
{
    public interface IDragAndDrop
    {
        void DragStart();
        void Drag();
        void DragEnd();
    }
}
