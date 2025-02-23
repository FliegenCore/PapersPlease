using Core.Entities;
using System.Collections;
using UnityEngine;

namespace Core.World
{
    [Order(10000)]
    public class MagnifyingGlassController : MonoBehaviour, IControllerEntity
    {
        [SerializeField] private Camera _camera; 
        [SerializeField] private LineRenderer _lineRenderer; 
        [SerializeField] private float _returnSpeed = 10f;

        [Inject] private CharactersController _charactersController;

        private Vector3 _initialPosition; 
        private bool _isDragging = false;
        private Vector3 _offset;

        private bool _withLoop;

        public bool WithLoop => _withLoop;

        public void PreInit()
        {
            _initialPosition = transform.position; 
            _lineRenderer.positionCount = 2;
            _lineRenderer.enabled = false; 
        }

        public void Init()
        {
        }

        private void Update()
        {
            HandleDrag();
        }

        private void HandleDrag()
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                StartDrag();
            }

            if (_isDragging)
            {
                if (Input.GetMouseButton(0)) 
                {
                    ContinueDrag();
                }
                else 
                {
                    EndDrag();
                }
            }
        }

        private void StartDrag()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform) 
                {
                    _isDragging = true;
                    _withLoop = true;
                    _lineRenderer.enabled = true;

                    Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.WorldToScreenPoint(transform.position).z));
                    _offset = transform.position - mouseWorldPos;
                }
            }
        }

        private void ContinueDrag()
        {
            Vector3 mouseWorldPos = 
                _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, _camera.WorldToScreenPoint(transform.position).z));
            transform.position = mouseWorldPos + _offset;

            _lineRenderer.SetPosition(0, _initialPosition);
            _lineRenderer.SetPosition(1, transform.position); 
        }

        private void EndDrag()
        {
            _isDragging = false;
            _lineRenderer.enabled = false;
            StartCoroutine(WaitDoFalse());
            StartCoroutine(ReturnToInitialPosition()); 
        }

        private IEnumerator WaitDoFalse()
        { 
            yield return new WaitForEndOfFrame();
            _withLoop = false;
        }

        private IEnumerator ReturnToInitialPosition()
        {
            while (Vector3.Distance(transform.position, _initialPosition) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, _initialPosition, Time.deltaTime * _returnSpeed);
                yield return null;
            }

            transform.position = _initialPosition;
        }
    }
}