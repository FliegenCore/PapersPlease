using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.PlayerInput
{
    public class ImageInteractable : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private Material _material;

        private Material _oldMaterial;

        private void OnMouseUp()
        {
            StartCoroutine(WaitOneFrame()); 
        }

        private IEnumerator WaitOneFrame()
        {
            yield return new WaitForEndOfFrame();
            if (_image != null)
            {
                if (!_image.GetComponent<Button>().interactable)
                {
                    _image.material = null;
                }
            }
        }

        private void OnMouseEnter()
        {
            if (_image != null)
            {
                if (_image.GetComponent<Button>().interactable)
                { 
                    _image.material = _material;
                }
            }
            if (_spriteRenderer != null)
            {
                _oldMaterial = new Material(_spriteRenderer.material);

                _spriteRenderer.material = _material;
            }
        }

        private void OnMouseExit()
        {
            if (_image != null)
            {
                _image.material = null;
            }
            if (_spriteRenderer != null)
            {
                _spriteRenderer.material = _oldMaterial;
            }
        }
    }
}
