using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Events.GameLevelEvents;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.LevelsLogic.Portals
{
    public class Portal : MonoBehaviour, IInteractable
    {
        [Inject] private EventBus _eventBus;
        [SerializeField] private Material _enabledMat;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Collider _portalCollider;

        public Transform Transform => transform;


        public void EnablePortal()
        {
            var mats = meshRenderer.materials;
            var list = new System.Collections.Generic.List<Material>(mats);

            if (!list.Contains(_enabledMat))
                list.Add(_enabledMat);

            meshRenderer.materials = list.ToArray();
            _portalCollider.enabled = true;

        }
        public void DisablePortal()
        {
            var mats = meshRenderer.materials;
            var list = new System.Collections.Generic.List<Material>(mats);

            if (list.Contains(_enabledMat))
                list.Remove(_enabledMat);

            meshRenderer.materials = list.ToArray();
            _portalCollider.enabled = false;
        }

        public void Interact()
        {
            _eventBus.Publish(new PortalInteractedEvent());
        }

    }
}