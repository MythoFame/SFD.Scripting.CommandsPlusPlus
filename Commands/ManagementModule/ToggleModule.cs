using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void ToggleModule(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /toggle_module <module>", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (!ModuleRegistry.TryGet(tokens[0], out CommandsModule module))
            {
                Game.ShowChatMessage($"Module '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (string.Equals(module.Name, "Management", StringComparison.OrdinalIgnoreCase))
            {
                Game.ShowChatMessage("The management module cannot be restricted.", Color.Red, args.User.UserIdentifier);
                return;
            }

            bool restricted = module.Toggle();

            Game.ShowChatMessage($"Module {module.Name} {(restricted ? "restricted" : "allowed")}.",
                restricted ? Color.Yellow : Color.Green, args.User.UserIdentifier);
        }
    }
}
