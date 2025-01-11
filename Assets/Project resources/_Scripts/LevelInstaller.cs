using UnityEngine;
using Zenject;

namespace AviGames
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private PlayArea _playArea;
        public override void InstallBindings()
        {
            Container.Bind<PlayArea>().FromInstance(_playArea).AsSingle();
        }
    }
}
