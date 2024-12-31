using System.ComponentModel;

namespace Game.Core;

public enum CellType
{
    [Description(".")]
    Empty,

    [Description("#")]
    Wall,

    [Description("T")]
    Treasure
}
