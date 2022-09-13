using InGame.Analytics;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace InGame.DI
{
    public class ProjectInstaller : MonoInstaller
    {
        public override async void InstallBindings()
        {
            Debug.Log("Project installation started");

            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();

            Container.Bind<GameAnalytics>().AsSingle();

            Debug.Log("Project installed");
        }
    }
}