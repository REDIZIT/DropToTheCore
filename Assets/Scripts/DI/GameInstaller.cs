using InGame.Level;
using UnityEngine;
using Zenject;

namespace InGame.DI
{
    public class GameInstaller : MonoInstaller
	{
		[SerializeField] private new CameraController camera;

        public override void InstallBindings()
        {
            Container.Bind<CameraController>().FromInstance(camera).AsSingle();
        }
    }
}