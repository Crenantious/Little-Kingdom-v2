using System;

namespace LittleKingdom.Board
{
    [Flags]
	public enum ResourceType
    {
        None = 0,
        Wood = 1,
        Stone = 2,
        Brick = 4,
        Glass = 8,
        Metal = 16
    }
}