namespace LittleKingdom
{
    public interface IPlayer
    {
        public ITown Town { get; }

        /// <summary>
        /// The amount of players existing at the time of creation. E.g. 0 indicates this is the first player to be created.
        /// </summary>
        public int Number { get; }
    }
}