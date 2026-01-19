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

    [Inject]
    [UsedImplicitly]
    private void Construct(PlayerStats stats,
        PlayerTalentSystem playerTalentSystem,
        PlayerAudio audio)
    {
        _playerStats = stats;
        _playerTalentSystem = playerTalentSystem;
        _playerAudio = audio;
    }
    [Inject] private DiContainer _container; 

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
        var defaultPlayerPos = new Vector3(0, -1, 0);
        instance.transform.position = defaultPlayerPos;
        _playerStats.InitDefaultStats(classSO.DefaultStatsSO);
        _playerTalentSystem.InitCurrentClass(classSO);
        _playerAudio.InitCharacterSounds(classSO.ClassSound);
    }

    private GameObject GetPrefabForClass(PlayerClass playerClass, int skinID)
    {
        switch (playerClass)
        {
            case PlayerClass.Knight:
                return _playerSkins[0];
            case PlayerClass.Mage:
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
            case PlayerClass.Mage:
                return _allClassSOsList[1];
        }
        return null;
    }
}

