using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void ActionCommand(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 2)
            {
                Game.ShowChatMessage("Usage: /action <player> <action>", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (!Enum.TryParse(tokens[1], true, out PlayerCommandType action) || !Enum.IsDefined(action))
            {
                Game.ShowChatMessage("Invalid action.", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            int affected = 0;
            int skipped = 0;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                if (player.IsInputEnabled)
                {
                    skipped++;
                    continue;
                }

                player.AddCommand(new PlayerCommand(action));
                affected++;
            }

            if (affected > 0)
            {
                Game.ShowChatMessage($"Queued {action} for {affected} player(s).", Color.Green, args.User.UserIdentifier);
            }

            if (skipped > 0)
            {
                Game.ShowChatMessage($"{skipped} player(s) must have their input disabled.", Color.Yellow, args.User.UserIdentifier);
            }
        }
    }
}
