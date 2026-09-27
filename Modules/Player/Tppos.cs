using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Tppos(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 3)
            {
                Game.ShowChatMessage("Usage: /tppos <player> <x> <y>", Color.Red, uid);
                return;
            }

            if (!float.TryParse(tokens[1], out float x))
            {
                Game.ShowChatMessage($"Invalid x coordinate '{tokens[1]}'.", Color.Red, uid);
                return;
            }

            if (!float.TryParse(tokens[2], out float y))
            {
                Game.ShowChatMessage($"Invalid y coordinate '{tokens[2]}'.", Color.Red, uid);
                return;
            }

            IPlayer[] sources = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (sources.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            TeleportPlayers(args, sources, new(x, y), $"({x}, {y})");
        }
    }
}
