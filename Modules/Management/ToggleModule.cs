using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Modules.GameScript;
using static SFD.Scripting.CommandsPlusPlus.Utils.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Management;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void ToggleModule(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /toggle_module <module>", Color.Red, uid);
                return;
            }

            if (!ModuleRegistry.TryGet(tokens[0], out CommandsModule module))
            {
                Game.ShowChatMessage($"Module '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            if (string.Equals(module.Name, "Management", StringComparison.OrdinalIgnoreCase))
            {
                Game.ShowChatMessage("The management module cannot be restricted.", Color.Red, uid);
                return;
            }

            bool restricted = module.Toggle();

            Game.ShowChatMessage($"Module {module.Name} {(restricted ? "restricted" : "allowed")}.",
                restricted ? Color.Yellow : Color.Green, uid);
        }
    }
}
