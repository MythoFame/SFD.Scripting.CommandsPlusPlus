using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public abstract class GameScriptInterfaceExtended : GameScriptInterface
{
    protected static readonly IGame Game;
}

public partial class GameScript : GameScriptInterfaceExtended
{
    public static void OnStartup()
    {
        // Temporary debug help command. Will replace later with display help function that supports modules.
        CommandHandler.ActiveCommands.Add(new("cfx", CommandHandler.DisplayHelp));

        ModuleRegistry.RegisterAll();

        //Game.ShowChatMessage("Commands++", Color.Yellow);
    }
}
