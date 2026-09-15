using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void ShowCommands(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /commands [module]", Color.Red, args.User.UserIdentifier);
                return;
            }

            IEnumerable<CommandsModule> modules = ModuleRegistry.All
                .OrderBy(module => module.Name != "Management")
                .ThenBy(module => module.Name);

            if (tokens.Length == 1)
            {
                if (!ModuleRegistry.TryGet(tokens[0], out CommandsModule module))
                {
                    Game.ShowChatMessage($"Module '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                    return;
                }

                modules = [module];
            }

            foreach (CommandsModule module in modules)
            {
                Game.ShowChatMessage($"{module.Name}: {module.Description}", Color.Green, args.User.UserIdentifier);

                foreach (CommandHandler.Command command in module.Commands.OrderBy(command => command.Name))
                {
                    if (command.ModeratorOnly && !args.User.IsModerator) continue;
                    if (command.HostOnly && !args.User.IsHost) continue;

                    string displayText = $"/{command.Name} ";

                    if (command.Description != null)
                        displayText += command.Description;

                    Color color = command.HostOnly ? Color.Magenta
                        : command.ModeratorOnly ? Color.Yellow
                        : Color.Green;

                    Game.ShowChatMessage(displayText, color, args.User.UserIdentifier);
                }
            }
        }
    }
}
