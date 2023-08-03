using LittleKingdom.Board;
using LittleKingdom.Input;
using LittleKingdom.UI;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class Installer : MonoInstaller
    {
        public MonoSimulator monoSimulator;
        public Camera camera;
        public BoardMono boardMono;
        public DialogBox dialogBox;

        public override void InstallBindings()
        {
            Container.Bind<Inputs>().AsSingle();
            Container.Bind<InGameInput>().AsSingle();
            Container.Bind<UIInput>().AsSingle();
            Container.Bind<InputUtility>().AsSingle();
            Container.Bind<TownPlacement>().AsSingle();

            Container.BindInstance(monoSimulator).AsSingle();
            Container.BindInstance(camera).AsSingle();
            Container.BindInstance(boardMono).AsSingle();
            Container.BindInstance(dialogBox).AsSingle();
        }
    }
}