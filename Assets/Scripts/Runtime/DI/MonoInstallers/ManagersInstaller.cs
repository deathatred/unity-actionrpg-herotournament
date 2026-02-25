using Assets.Scripts.Runtime.Firebase;
using Assets.Scripts.Runtime.Managers;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.DI.MonoInstallers
{
    public class ManagersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<FirebaseManager>().FromNew().AsSingle().NonLazy();
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<AudioManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<EnemySpawnManager>().FromNew().AsSingle().NonLazy();
            Container.Bind<ItemSpawnManager>().FromNew().AsSingle().NonLazy();
            Container.Bind<PlayerSaveManager>().FromNew().AsSingle().NonLazy();
            Container.Bind<LevelSaveManager>().FromNew().AsSingle().NonLazy();
            Container.Bind<GlobalSaveManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<ViewManager>().FromNew().AsSingle().NonLazy();
        }
    }
}