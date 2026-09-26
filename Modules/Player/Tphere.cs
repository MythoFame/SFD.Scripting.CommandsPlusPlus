using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Tphere(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /tphere <player>", Color.Red, uid);
                return;
            }

            IPlayer self = args.User.GetPlayer();

            if (self == null || self.IsRemoved)
            {
                Game.ShowChatMessage("You have no live player to teleport to.", Color.Red, uid);
                return;
            }

            IPlayer[] matched = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (matched.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            IPlayer[] sources = [.. matched.Where(p => p != null && p.UniqueID != self.UniqueID)];

            if (sources.Length == 0)
            {
                Game.ShowChatMessage("You cannot teleport yourself to yourself.", Color.Red, uid);
                return;
            }

            TeleportPlayers(args, sources, self.GetWorldPosition(), "you");
        }
    }
}
