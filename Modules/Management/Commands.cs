using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Commands.GameScript;
using static SFD.Scripting.CommandsPlusPlus.Modules.GameScript;
using static SFD.Scripting.CommandsPlusPlus.Utils.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Management;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void ShowCommands(UserMessageCallbackArgs args)
        {
            IUser user = args.User;
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /commands [module]", Color.Red, user.UserIdentifier);
                return;
            }

            IEnumerable<CommandsModule> modules = ModuleRegistry.Modules
                .OrderBy(module => module.Name != "Management")
                .ThenBy(module => module.Name);

            if (tokens.Length == 1)
            {
                if (!ModuleRegistry.TryGet(tokens[0], out CommandsModule module))
                {
                    Game.ShowChatMessage($"Module '{tokens[0]}' not found.", Color.Red, user.UserIdentifier);
                    return;
                }

                modules = [module];
            }

            foreach (CommandsModule module in modules)
            {
                Game.ShowChatMessage($"{module.Name}: {module.Description}", Color.Green, user.UserIdentifier);

                foreach (Command command in module.Commands.OrderBy(command => command.Name))
                {
                    if (command.Permissions == CommandPermision.Host && !user.IsHost) continue;
                    if (command.Permissions == CommandPermision.Moderator && !(user.IsHost || user.IsModerator)) continue;

                    string displayText = $"/{command.Name} ";

                    if (command.Description != null)
                        displayText += command.Description;


                    Color msgColor = Color.White;
                    switch (command.Permissions)
                    {
                        case CommandPermision.Host:
                            msgColor = Color.Magenta;
                            break;

                        case CommandPermision.Moderator:
                            msgColor = Color.Yellow;
                            break;

                        case CommandPermision.Everyone:
                        default:
                            msgColor = Color.Green;
                            break;
                    }

                    Game.ShowChatMessage(displayText, msgColor, user.UserIdentifier);
                }
            }
        }
    }
}
