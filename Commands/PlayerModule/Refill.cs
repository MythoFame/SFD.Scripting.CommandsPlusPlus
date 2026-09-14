using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Refill(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /refill <player>", Color.Red, args.User.UserIdentifier);
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

                player.SetCurrentPrimaryWeaponAmmo(int.MaxValue);
                player.SetCurrentSecondaryWeaponAmmo(int.MaxValue);
                player.SetCurrentThrownItemAmmo(int.MaxValue);
                affected++;
            }

            if (affected == 0) return;

            Game.ShowChatMessage($"Refilled ammo for {affected} player(s).", Color.Green, args.User.UserIdentifier);
        }
    }
}
