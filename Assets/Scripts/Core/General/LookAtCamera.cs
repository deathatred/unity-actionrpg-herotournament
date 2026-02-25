using UnityEngine;

namespace Assets.Scripts.Core.General
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.transform.forward);
        }
    }
}