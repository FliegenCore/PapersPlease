using UnityEngine;

namespace Core.Dialogues
{
    public class DialogueWindow : MonoBehaviour
    {
        private DialogueData _dialogueData;

        public void SetDialogueText(string speacker, string description)
        {
            _dialogueData.Name = speacker;
            _dialogueData.Description = description;
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
