using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Respawn(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /respawn <delay>", Color.Red, uid);
                return;
            }

            if (!int.TryParse(tokens[0], out int parsed))
            {
                Game.ShowChatMessage($"Invalid amount '{tokens[0]}'.", Color.Red, uid);
                return;
            }

            uint delay = parsed <= 0 ? 0 : (uint)parsed;

            RespawnRule.Delay = delay;

            if (delay > 0)
                Game.ShowChatMessage($"Respawn delay set to {delay}.", Color.Green, uid);
            else
                Game.ShowChatMessage("Custom respawn disabled.", Color.Green, uid);
        }
    }
}
