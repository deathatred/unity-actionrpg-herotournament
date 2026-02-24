using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class CharacterSelectionRoom : MonoBehaviour
{

    [SerializeField] private GameObject _characterCreationRoom;
    [SerializeField] private Camera _characterCreationRoomCamera;
    [SerializeField] private Camera _mainCamera;

    private EventBus _eventBus;
    [Inject] 
    private void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    private void OnEnable()
    {
        SubscribeToEvents();      
    }
    private void Awake()
    {
        SetupMainCamera();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<ClassSelectionMenuOpenedEvent>(ShowSelectionRoom);
        _eventBus.Subscribe<ClassSelectedEvent>(HideSelectionRoom);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Subscribe<ClassSelectionMenuOpenedEvent>(ShowSelectionRoom);
        _eventBus.Subscribe<ClassSelectedEvent>(HideSelectionRoom);
    }
    private void SetupMainCamera()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
    }
    private void HideSelectionRoom(ClassSelectedEvent e)
    {
        if (_mainCamera != null)
            _mainCamera.gameObject.SetActive(true);

        _characterCreationRoomCamera.gameObject.SetActive(false);
        _characterCreationRoom.SetActive(false);
    }
    private void ShowSelectionRoom(ClassSelectionMenuOpenedEvent e)
    {
        if (_mainCamera != null)
            _mainCamera.gameObject.SetActive(false);

        _characterCreationRoomCamera.gameObject.SetActive(true);
        _characterCreationRoom.SetActive(true);
    }
}
