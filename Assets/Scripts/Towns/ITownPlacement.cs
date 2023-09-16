namespace LittleKingdom
{
    public interface ITownPlacement
    {
        public event SimpleEventHandler<ITown> TownPlaced;

        /// <summary>
        /// Placement may take multiple frames so be sure to listen to <see cref="TownPlaced"/> to know when it's finished.
        /// </summary>
        /// <param name="town"></param>
        public void Place(ITown town);
    }
}