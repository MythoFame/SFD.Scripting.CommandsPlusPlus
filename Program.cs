using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Modules.GameScript;

namespace SFD.Scripting.CommandsPlusPlus;

public abstract class GameScriptInterfaceExtended : GameScriptInterface
{
    protected static readonly IGame Game;
}

public partial class GameScript : GameScriptInterfaceExtended
{
    public static void OnStartup()
    {
        ModuleRegistry.RegisterAll();
    }
}
