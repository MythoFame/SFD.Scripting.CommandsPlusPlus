using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Revive(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /revive <player>", Color.Red, uid);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            int affected = 0;
            int skipped = 0;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                if (PlayerHelper.Revive(player) != null)
                    affected++;
                else
                    skipped++;
            }

            if (affected > 0)
            {
                Game.ShowChatMessage($"Revived {affected} player(s).", Color.Green, uid);
            }

            if (skipped > 0)
            {
                Game.ShowChatMessage($"{skipped} player(s) were not dead.", Color.Yellow, uid);
            }
        }
    }
}
