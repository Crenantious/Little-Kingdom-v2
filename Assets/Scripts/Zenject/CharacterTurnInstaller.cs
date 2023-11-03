using LittleKingdom.CharacterTurns;
using LittleKingdom.Factories;

namespace LittleKingdom
{
    public class CharacterTurnInstaller : Installer<CharacterTurnInstaller.BindType, CharacterTurnInstaller>
    {
        public enum BindType
        {
            CharacterTurnOrder,
            ICharacterTurnTransitions,
            CharacterTurnFactory
        }

        public override void InstallBindings()
        {
            Install(BindType.CharacterTurnOrder, () => Container.Bind<CharacterTurnOrder>().AsTransient());
            Install(BindType.ICharacterTurnTransitions, () => Container.Bind<ICharacterTurnTransitions>().To<CharacterTurnTransitions>().AsSingle());
            Install(BindType.CharacterTurnFactory, () => Container.BindFactory<ICharacter, ICharacterTurn, CharacterTurnFactory>()
                                                                .FromFactory<CustomCharacterTurnFactory>());
        }
    }
}