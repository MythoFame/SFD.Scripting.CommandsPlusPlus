using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Gib(UserMessageCallbackArgs args)
        {
            IPlayer[] players = [.. ParseHelper.ParsePlayers(args.CommandArguments, args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{args.CommandArguments[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            int affected = 0;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                player.Gib();
                affected++;
            }

            Game.ShowChatMessage($"Gibbed {affected} player(s).", Color.Green, args.User.UserIdentifier);
        }
    }
}
