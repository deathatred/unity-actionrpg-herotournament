using UnityEngine;
using Zenject;

public class UiInstaller : MonoInstaller
{
    [SerializeField] private Canvas[] _views;
    public override void InstallBindings()
    {
        Container.Bind<Canvas[]>().FromInstance(_views).AsSingle();
    }
}
