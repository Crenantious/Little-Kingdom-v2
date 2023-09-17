using System.Collections.Generic;

namespace LittleKingdom
{
    public interface IPlayer
    {
        public Resources.Resources Resources { get; }

        public List<IPowerCard> OffensiveCards { get; }

        public List<IPowerCard> DefensiveCards { get; }

        public List<IPowerCard> UtilityCards { get; }

        public ITown Town { get; }

        /// <summary>
        /// The amount of players existing at the time of creation. E.g. 0 indicates this is the first player to be created.
        /// </summary>
        public int Number { get; }
    }
}