using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Tphere(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /tphere <player>", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer self = args.User.GetPlayer();

            if (self == null || self.IsRemoved)
            {
                Game.ShowChatMessage("You have no live player to teleport to.", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] matched = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (matched.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] sources = [.. matched.Where(p => p != null && p.UniqueID != self.UniqueID)];

            if (sources.Length == 0)
            {
                Game.ShowChatMessage("You cannot teleport yourself to yourself.", Color.Red, args.User.UserIdentifier);
                return;
            }

            TeleportPlayers(args, sources, self.GetWorldPosition(), "you");
        }
    }
}
