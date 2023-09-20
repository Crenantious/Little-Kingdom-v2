using System;

namespace LittleKingdom.Resources
{
    [Flags]
	public enum ResourceType
    {
        None  = 0,
        Stone = 1 << 0,
        Wood  = 1 << 1,
        Brick = 1 << 2,
        Glass = 1 << 3,
        Metal = 1 << 4
    }
}