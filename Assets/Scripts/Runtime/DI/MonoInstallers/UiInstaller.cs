using Assets.Scripts.Runtime.UI;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.DI.MonoInstallers
{
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
}