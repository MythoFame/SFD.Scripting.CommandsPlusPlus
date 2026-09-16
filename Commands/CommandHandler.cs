using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Modules.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Commands;

public partial class GameScript : GameScriptInterfaceExtended
{
    public static class CommandHandler
    {
        private static Events.UserMessageCallback _callback = null;

        public static void Init()
        {
            if (_callback != null)
            {
                Game.WriteToConsoleF("CommandHandler already initialized.");
                return;
            }

            _callback = Game.Events.StartUserMessageCallback(OnUserMessage);
            Game.WriteToConsoleF("CommandHandler initialized.");
        }

        private static void OnUserMessage(UserMessageCallbackArgs args)
        {
            if (!args.IsCommand) return;

            IUser user = args.User;
            Command commandActivated = null;

            foreach (CommandsModule module in ModuleRegistry.Modules)
            {
                if (!module.IsEnabled && !args.User.IsHost) continue;

                foreach (Command command in module.Commands)
                {
                    if (args.Command == command.Name)
                    {
                        commandActivated = command;
                        break;
                    }
                }
            }

            if (commandActivated == null) return;

            if ((commandActivated.Permissions == CommandPermision.Host) && user.IsHost ||
                commandActivated.Permissions == CommandPermision.Moderator && (user.IsHost || user.IsModerator) ||
                commandActivated.Permissions == CommandPermision.Everyone)
            {
                commandActivated.OnCommand.Invoke(args);
            }
            else
            {
                Game.ShowChatMessage("You don't have permission to use this command.", Color.Red, user.UserIdentifier);
            }
        }
    }
}
