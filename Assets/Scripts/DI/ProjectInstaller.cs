using InGame.Analytics;
using UnityEngine;
using Zenject;

namespace InGame.DI
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("Project installation started");

            Container.Bind<GameAnalytics>().AsSingle();

            Debug.Log("Project installed");
        }
    }
}