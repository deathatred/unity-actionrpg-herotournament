using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Transform _playerTransform;
    public override void InstallBindings()
    {
        Container.Bind<PlayerStateFactory>().AsSingle();
        Container.Bind<PlayerClassHolder>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerAudio>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerStats>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerWallCheck>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerAnimations>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerHealthSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerInteractions>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerAttackSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerTalentSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerInputHandler>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerSpellCasting>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerStateMachine>().FromComponentInHierarchy().AsSingle().NonLazy();  
        Container.Bind<PlayerLevelSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerInventory>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerItemUsingSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerSpellbook>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerConfigurator>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<Transform>().FromInstance(_playerTransform).AsSingle().NonLazy();
    }
}
