using LittleKingdom.CharacterTurns;
using LittleKingdom.Factories;
using Zenject;

namespace LittleKingdom
{
    public class CharacterTurnInstaller : Installer<CharacterTurnInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CharacterTurnOrder>().AsTransient();
            Container.Bind<ICharacterTurnTransitions>().To<CharacterTurnTransitions>().AsSingle();
            Container.BindFactory<ICharacter, ICharacterTurn, CharacterTurnFactory>().FromFactory<CustomCharacterTurnFactory>();
        }
    }
}