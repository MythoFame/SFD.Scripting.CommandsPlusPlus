using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Kill(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length == 0 || tokens.Length > 2)
            {
                Game.ShowChatMessage("Usage: /kill <player> [gib|rm]", Color.Red, args.User.UserIdentifier);
                return;
            }

            string mode = tokens.Length == 2 ? tokens[1] : string.Empty;

            if (mode.Length != 0
                && !string.Equals(mode, "gib", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(mode, "rm", StringComparison.OrdinalIgnoreCase))
            {
                Game.ShowChatMessage("Usage: /kill <player> [gib|rm]", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            int affected = 0;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                if (string.Equals(mode, "gib", StringComparison.OrdinalIgnoreCase))
                    player.Gib();
                else if (string.Equals(mode, "rm", StringComparison.OrdinalIgnoreCase))
                    player.Remove();
                else
                    player.Kill();

                affected++;
            }

            string verb = string.Equals(mode, "gib", StringComparison.OrdinalIgnoreCase) ? "Gibbed"
                : string.Equals(mode, "rm", StringComparison.OrdinalIgnoreCase) ? "Removed"
                : "Killed";

            Game.ShowChatMessage($"{verb} {affected} player(s).", Color.Green, args.User.UserIdentifier);
        }
    }
}
