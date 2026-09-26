using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Team(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 2)
            {
                Game.ShowChatMessage("Usage: /team <player> <team>", Color.Red, uid);
                return;
            }

            if (!Enum.TryParse(tokens[1], true, out PlayerTeam team) || !Enum.IsDefined(team))
            {
                Game.ShowChatMessage("Invalid team. Use independent (or 0) or team 1-8.", Color.Red, uid);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            int affected = 0;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                player.SetTeam(team);
                affected++;
            }

            Game.ShowChatMessage($"Set {affected} player(s) to {team}.", Color.Green, uid);
        }
    }
}
