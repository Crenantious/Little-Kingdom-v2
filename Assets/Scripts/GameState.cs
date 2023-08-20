using System;

namespace LittleKingdom
{
    [Flags]
    public enum GameState
    {
        None            = 0,
        StandardInGame  = 1 << 0,
        UILocked        = 1 << 1,
        LoadingScreen   = 1 << 3,
        PlacingTowns    = 1 << 4,
    }
}