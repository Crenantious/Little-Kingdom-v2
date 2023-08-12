using LittleKingdom.Board;
using UnityEngine;

namespace LittleKingdom.Extensions
{
    public static class BoardExtensions
    {
        public static ITile GetTownOriginFromPointerPosition(this IBoard board, ITown town, Vector2 position)
        {
            (int column, int row) = board.Tiles.GetNearestIndex(position);

            int maxColumnIndex = (board.Tiles.Width - 1) - (town.Width - 1);
            int minRowIndex = town.Height - 1;

            column = Mathf.Clamp(column, 0, maxColumnIndex);
            row = Mathf.Clamp(row, minRowIndex, board.Tiles.Height - 1);

            return board.Tiles.Get(column, row);
        }
    }
}