namespace LittleKingdom.DataStructures
{
    public class SizedGrid<TElement> : Grid<TElement>
    {
        public float CellWidth { get; }
        public float CellHeight { get; }

        public SizedGrid(int width, int height, float cellWidth, float cellHeight) : base(width, height)
        {
            CellWidth = cellWidth;
            CellHeight = cellHeight;
        }
    }
}