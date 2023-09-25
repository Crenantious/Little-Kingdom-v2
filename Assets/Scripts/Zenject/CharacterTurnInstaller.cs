using LittleKingdom.CharacterTurns;
using LittleKingdom.Factories;
using Zenject;

namespace LittleKingdom
{
    public class CharacterTurnInstaller : Installer<CharacterTurnInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CharacterTurnTransitions>().AsSingle();
            Container.Bind<CharacterTurnOrder>().AsTransient();
            Container.BindFactory<ICharacter, ICharacterTurn, CharacterTurnFactory>().FromFactory<CustomCharacterTurnFactory>();
        }
    }
}