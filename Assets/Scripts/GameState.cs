using System;

namespace LittleKingdom
{
    [Flags]
    public enum GameState
    {
        StandardInGame  = 1 << 0,
        UILocked        = 1 << 1,
    }
}