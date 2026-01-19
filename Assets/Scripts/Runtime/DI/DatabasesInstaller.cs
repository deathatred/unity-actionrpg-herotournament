using UnityEngine;
using Zenject;

public class DatabasesInstaller : MonoInstaller
{
    [SerializeField] private TalentDatabase _talentDatabase;
    [SerializeField] private ItemsDatabase _itemsDatabase;
    [SerializeField] private EnemyDatabase _enemyDatabase;

    public override void InstallBindings()
    {
        _talentDatabase.Init();
        Container.Bind<TalentDatabase>().FromInstance(_talentDatabase).AsSingle();
        _itemsDatabase.Init();
        Container.Bind<ItemsDatabase>().FromInstance(_itemsDatabase).AsSingle();
        _enemyDatabase.Init();
        Container.Bind<EnemyDatabase>().FromInstance(_enemyDatabase).AsSingle();
    }
}
