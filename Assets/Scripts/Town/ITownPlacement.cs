namespace LittleKingdom
{
    public interface ITownPlacement
    {
        /// <summary>
        /// Perform pre-placement logic such as getting user input or calculating placement position.
        /// </summary>
        public void BeginPlacement(Town town);

        public void FinalisePlacement();
    }
}