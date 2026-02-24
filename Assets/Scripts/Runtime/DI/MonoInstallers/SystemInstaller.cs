using Assets.Scripts.Core.Observer;
using Assets.Scripts.Core.Pools;
using UnityEngine;
using Zenject;

public class SystemInstaller : MonoInstaller
{  
    public override void InstallBindings()
    {
        Container.Bind<EventBus>().AsSingle().NonLazy();     
        Container.Bind<ProjectilePool>().FromComponentInHierarchy().AsSingle().NonLazy();     
    }
}
