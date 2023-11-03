using LittleKingdom.Factories;

namespace LittleKingdom
{
    public class PlayerInstaller : Installer<PlayerInstaller.BindType, PlayerInstaller>
    {
        public static Player Player { get; set; }

        public enum BindType
        {
            Player,
            PlayerFactory
        }

        public override void InstallBindings()
        {
            Install(BindType.Player, () => Container.BindInstance(Player).AsSingle());
            Install(BindType.PlayerFactory, () => Container.BindFactory<string, IPlayer, PlayerFactory>().FromFactory<PlayerFactory>());
        }
    }
}