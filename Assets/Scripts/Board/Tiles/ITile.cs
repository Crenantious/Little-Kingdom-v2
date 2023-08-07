namespace LittleKingdom.Board
{
    public interface ITile
    {
        // TODO: JR - make this a flag enum.
        /// <summary>
        /// The resource that this tile produces.
        /// </summary>
        public string ResourceName { get; }

        /// <summary>
        /// The index of the column on the board that this tile is located.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// The index of the row on the board that this tile is located.
        /// </summary>
        public int Row { get; set; }

#nullable enable

        /// <summary>
        /// The town that occupies this tile.
        /// </summary>
        public ITown? Town { get; set; }

#nullable disable
    }
}