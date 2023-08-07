namespace LittleKingdom
{
    public interface ITownPlacement
    {
        /// <summary>
        /// Position the town on the board and assign the containing tiles.
        /// </summary>
        public void Place(Town town);
    }
}