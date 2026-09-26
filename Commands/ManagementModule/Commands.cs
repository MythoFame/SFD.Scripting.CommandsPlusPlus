using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void ShowCommands(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /commands [module]", Color.Red, uid);
                return;
            }

            IEnumerable<CommandsModule> modules = ModuleRegistry.All
                .OrderBy(module => module.Name != "Management")
                .ThenBy(module => module.Name);

            if (tokens.Length == 1)
            {
                if (!ModuleRegistry.TryGet(tokens[0], out CommandsModule module))
                {
                    Game.ShowChatMessage($"Module '{tokens[0]}' not found.", Color.Red, uid);
                    return;
                }

                modules = [module];
            }

            foreach (CommandsModule module in modules)
            {
                CommandHandler.Command[] visible = [.. module.Commands
                    .OrderBy(command => command.Name)
                    .Where(command => (!command.ModeratorOnly || args.User.IsModerator)
                        && (!command.HostOnly || args.User.IsHost))];

                if (visible.Length == 0) continue;

                Game.ShowChatMessage($"{module.Name}: {module.Description}", Color.Green, uid);

                foreach (CommandHandler.Command command in visible)
                {

                    string displayText = $"/{command.Name} ";

                    if (command.Description != null)
                        displayText += command.Description;

                    Color color = command.HostOnly ? Color.Magenta
                        : command.ModeratorOnly ? Color.Yellow
                        : Color.Green;

                    Game.ShowChatMessage(displayText, color, uid);
                }
            }
        }
    }
}
