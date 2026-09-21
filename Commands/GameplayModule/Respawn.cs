using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Respawn(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /respawn <delay>", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (!int.TryParse(tokens[0], out int parsed))
            {
                Game.ShowChatMessage($"Invalid amount '{tokens[0]}'.", Color.Red, args.User.UserIdentifier);
                return;
            }

            uint delay = parsed <= 0 ? 0 : (uint)parsed;

            RespawnRule.Delay = delay;

            if (delay > 0)
                Game.ShowChatMessage($"Respawn delay set to {delay}.", Color.Green, args.User.UserIdentifier);
            else
                Game.ShowChatMessage("Custom respawn disabled.", Color.Green, args.User.UserIdentifier);
        }
    }
}
