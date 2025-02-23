using System.Collections;
using TMPro;
using UnityEngine;

namespace Core.World
{
    [Order(2132)]
    public class WriterController : MonoBehaviour, IControllerEntity
    {
        [SerializeField] private TMP_Text _writerText;

        private Coroutine _clearTextCoroutine;

        public void PreInit()
        {
        }

        public void Init()
        {
        }

        public void WirteText(string text)
        {
            if (_clearTextCoroutine != null)
            {
                StopCoroutine(_clearTextCoroutine);
            }

            _writerText.text = text;

            _clearTextCoroutine = StartCoroutine(ClearText());
        }


        private IEnumerator ClearText()
        { 
            yield return new WaitForSeconds(2f);
            _writerText.text = "";
        }
    }
}
