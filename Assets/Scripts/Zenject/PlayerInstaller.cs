using LittleKingdom.Factories;
using Zenject;

namespace LittleKingdom
{
    public class PlayerInstaller : Installer<PlayerInstaller>
    {
        public static Player Player { get; set; }

        public override void InstallBindings()
        {
            Container.BindInstance(Player).AsSingle();
            Container.BindFactory<IPlayer, PlayerFactory>().FromFactory<CustomPlayerMonoFactory>();
        }
    }
}