using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public abstract class GameScriptInterfaceExtended : GameScriptInterface
{
    protected static readonly IGame Game;
}

public partial class GameScript : GameScriptInterfaceExtended
{
    public void OnStartup()
    {
        CommandHandler.ActiveCommands.Add(new CommandHandler.Command("cfx", CommandHandler.DisplayHelp));
        ModuleRegistry.RegisterAll();

        Game.ShowChatMessage("Commands++", Color.Yellow);
    }
}
