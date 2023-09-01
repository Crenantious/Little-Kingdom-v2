using System;

namespace LittleKingdom.Resources
{
    [Flags]
	public enum ResourceType
    {
        None = 0,
        Wood = 1 << 0,
        Stone = 1 << 2,
        Brick = 1 << 3,
        Glass = 1 << 4,
        Metal = 1 << 5
    }
}