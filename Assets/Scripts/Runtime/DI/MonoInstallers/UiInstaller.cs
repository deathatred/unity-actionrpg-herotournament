using UnityEngine;
using Zenject;

public class UiInstaller : MonoInstaller
{
    [SerializeField] private Canvas[] _views;
    [SerializeField] private CharacterSelectionRoom _characterSelectionRoom; 
    public override void InstallBindings()
    {
        Container.Bind<Canvas[]>().FromInstance(_views).AsSingle();
        Container.Bind<CharacterSelectionRoom>().FromInstance(_characterSelectionRoom).AsSingle();
    }
}
