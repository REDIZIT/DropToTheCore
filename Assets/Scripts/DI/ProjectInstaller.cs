using InGame.Analytics;
using UnityEngine;
using Zenject;

namespace InGame.DI
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CoreAnalytics>().AsSingle().NonLazy();
        }
    }
}