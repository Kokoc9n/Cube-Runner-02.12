using Zenject;
using UnityEngine;
using Services;

namespace Core
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
#if UNITY_EDITOR
            Container.Bind<InputService>().To<StandaloneInputService>().AsSingle();
#else
            Container.Bind<InputService>().To<MobileInputService>().AsSingle();
#endif
        }
    }
}