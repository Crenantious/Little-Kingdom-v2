using LittleKingdom.Board;
using System;
using UnityEngine;

namespace LittleKingdom.Extensions
{
    public static class BoardExtensions
    {
        public static TileMono GetTownOriginFromPointerPosition(this BoardMono board, ITown town, Vector2 position)
        {
            (int column, int row) = board.Tiles.GetNearestIndex(position);

            float maxColumnIndex = (board.Tiles.Width - 1) - MathF.Ceiling((float)town.Width / 2);
            float minRowIndex = MathF.Ceiling((float)town.Height / 2);

            column = (int)Mathf.Clamp(column, 0, maxColumnIndex);
            row = (int)Mathf.Clamp(row, minRowIndex, board.Tiles.Height - 1);

            return board.Tiles.Get(column, row);
        }
    }
}