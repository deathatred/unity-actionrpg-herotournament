using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

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
