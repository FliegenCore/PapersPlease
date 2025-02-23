using TMPro;
using UnityEngine;

namespace Core.Dialogues
{
    public class DialogueWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _dialogueText;

        public void SetDialogueText(string description)
        {
            _dialogueText.text = description;
        }
    }
}
