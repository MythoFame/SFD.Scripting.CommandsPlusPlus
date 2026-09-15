using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Dmg(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 2)
            {
                Game.ShowChatMessage("Usage: /dmg <player> <amount>", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (!float.TryParse(tokens[1], out float amount))
            {
                Game.ShowChatMessage($"Invalid amount '{tokens[1]}'.", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (amount <= 0)
            {
                Game.ShowChatMessage("Amount must be positive.", Color.Red, args.User.UserIdentifier);
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

                player.DealDamage(amount);
                affected++;
            }

            if (affected == 0) return;

            Game.ShowChatMessage($"Dealt {amount} damage to {affected} player(s).", Color.Green, args.User.UserIdentifier);
        }
    }
}
