using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Speed(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length < 1 || tokens.Length > 2)
            {
                Game.ShowChatMessage("Usage: /speed <player> [time]", Color.Red, uid);
                return;
            }

            float time = DEFAULT_BOOST_TIME;

            if (tokens.Length == 2 && !float.TryParse(tokens[1], out time))
            {
                Game.ShowChatMessage($"Invalid time '{tokens[1]}'.", Color.Red, uid);
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

                player.SetSpeedBoostTime(time);
                affected++;
            }

            if (affected == 0) return;

            Game.ShowChatMessage($"Gave speed boost ({time} ms) to {affected} player(s).", Color.Green, uid);
        }
    }
}
