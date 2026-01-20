using JetBrains.Annotations;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class PlayerConfigurator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playerSkins = new List<GameObject>();
    [SerializeField] private List<ClassSO> _allClassSOsList = new List<ClassSO>();  
    [SerializeField] private Transform _playerObject;
    private PlayerStats _playerStats;
    private PlayerTalentSystem _playerTalentSystem;
    private PlayerAudio _playerAudio;
    private PlayerAnimations _playerAnimations;
    private EventBus _eventBus;
    [Inject] private DiContainer _container;
    [Inject]
    [UsedImplicitly]
    private void Construct(PlayerStats stats,
        PlayerTalentSystem playerTalentSystem,
        PlayerAudio audio,
        PlayerAnimations playerAnimations,
        EventBus eventBus)
    {
        _playerStats = stats;
        _playerTalentSystem = playerTalentSystem;
        _playerAudio = audio;
        _playerAnimations = playerAnimations;
        _eventBus = eventBus;
    }
    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<ClassSelectedEvent>(ClassSelectedSubscriber);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Subscribe<ClassSelectedEvent>(ClassSelectedSubscriber);
    }
    private void ClassSelectedSubscriber(ClassSelectedEvent e)
    {
        ConfigurePlayer(e.PlayerClass);
    }
    public void ConfigurePlayer(PlayerClass playerClass, int skinID = 0)
    {
        GameObject prefabToSpawn = GetPrefabForClass(playerClass, skinID);
        ClassSO classSO = GetClassSO(playerClass);
        if (prefabToSpawn == null || classSO == null)
        {
            Debug.LogError("Player prefab or classSO was not found");
            return;
        }
        GameObject instance = _container.InstantiatePrefab(prefabToSpawn, _playerObject.position, _playerObject.rotation, _playerObject);

        instance.transform.position = new Vector3(instance.transform.position.x, -1, instance.transform.position.z);
        _playerStats.InitDefaultStats(classSO.DefaultStatsSO);
        _playerTalentSystem.InitCurrentClass(classSO);
        _playerAudio.InitCharacterSounds(classSO.ClassSound);
        var animator = instance.GetComponent<Animator>();
        _playerAnimations.SetAnimator(animator);
        _eventBus.Publish(new PlayerConfiguredEvent());
        Debug.Log("Configured");
    }

    private GameObject GetPrefabForClass(PlayerClass playerClass, int skinID)
    {
        switch (playerClass)
        {
            case PlayerClass.Knight:
                return _playerSkins[0];
            case PlayerClass.Wizard:
                return _playerSkins[1];
        }
        if (skinID >= 0 && skinID < _playerSkins.Count)
            return _playerSkins[skinID];

        return null;
    }
    private ClassSO GetClassSO(PlayerClass playerClass)
    {
        switch (playerClass)
        {
            case PlayerClass.Knight:
                return _allClassSOsList[0];
            case PlayerClass.Wizard:
                return _allClassSOsList[1];
        }
        return null;
    }
}

