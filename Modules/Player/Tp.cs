using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Utils.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Player;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Tp(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /tp <to>", Color.Red, uid);
                return;
            }

            IPlayer self = args.User.GetPlayer();

            if (self == null || self.IsRemoved)
            {
                Game.ShowChatMessage("You have no live player to teleport.", Color.Red, uid);
                return;
            }

            IPlayer target = null;

            foreach (IPlayer candidate in ParseHelper.ParsePlayers(tokens[0], args.User))
            {
                if (candidate != null && !candidate.IsRemoved)
                {
                    target = candidate;
                    break;
                }
            }

            if (target == null)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            if (target.UniqueID == self.UniqueID)
            {
                Game.ShowChatMessage("You cannot teleport to yourself.", Color.Red, uid);
                return;
            }

            TeleportPlayers(args, [self], target.GetWorldPosition(), target.Name);
        }

        /// <summary>
        /// Shared teleport core for <c>/tp</c>, <c>/tphere</c> and <c>/tppos</c>:
        /// moves every live source player to the destination with a gleam trail
        /// and reports the count to the caller.
        /// </summary>
        private static void TeleportPlayers(UserMessageCallbackArgs args, IPlayer[] sources, Vector2 targetPos, string targetLabel)
        {
            int uid = args.User?.UserIdentifier ?? -1;
            int affected = 0;

            foreach (IPlayer player in sources)
            {
                if (player == null || player.IsRemoved) continue;

                Vector2 from = player.GetWorldPosition();
                player.SetWorldPosition(targetPos);
                affected++;

                PointShape.Trail(pos => Game.PlayEffect(EffectName.ItemGleam, pos), from, targetPos, 15f);
            }

            Game.ShowChatMessage($"Teleported {affected} player(s) to {targetLabel}.", Color.Green, uid);
        }
    }
}
