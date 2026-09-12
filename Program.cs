using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public abstract class GameScriptInterfaceExtended : GameScriptInterface
{
    protected static readonly IGame Game;
}

public partial class GameScript : GameScriptInterfaceExtended
{
    // ...
}
