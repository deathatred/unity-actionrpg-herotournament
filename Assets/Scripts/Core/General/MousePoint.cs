using UnityEngine;

namespace Assets.Scripts.Core.General
{
    public class MousePoint : MonoBehaviour
    {
        public static MousePoint Instance { get; private set; }

        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _rayDistance = 100f;

        private void Awake()
        {
            InitSingleton();
            RefreshCameraIfNeeded();
        }

        private void InitSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void RefreshCameraIfNeeded()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
        }

        public bool TryGetPointerWorldPosition(Vector3 pointer, out Vector3 worldPos)
        {
            worldPos = default;

            if (_mainCamera == null)
                _mainCamera = Camera.main;

            if (_mainCamera == null)
                return false;

            Ray ray = _mainCamera.ScreenPointToRay(pointer);

            if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _groundMask))
            {
                worldPos = hit.point;
                return true;
            }

            return false;
        }
    }
}