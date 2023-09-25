using System.Collections.Generic;
using Zenject;

namespace LittleKingdom.Factories
{
    public class CustomPlayerFactory : IFactory<IPlayer>
    {
        private int index = 0;

        public static List<IPlayer> Players { get; set; }

        public IPlayer Create() =>
            Players[index++];
    }
}