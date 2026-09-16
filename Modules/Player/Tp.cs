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

            if (tokens.Length == 0 || tokens.Length > 2)
            {
                Game.ShowChatMessage("Usage: /tp <from> [to]", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] sources = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (sources.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            Vector2 targetPos;
            string targetLabel;

            if (tokens.Length == 2)
            {
                IPlayer target = null;

                foreach (IPlayer candidate in ParseHelper.ParsePlayers(tokens[1], args.User))
                {
                    if (candidate != null && !candidate.IsRemoved)
                    {
                        target = candidate;
                        break;
                    }
                }

                if (target == null)
                {
                    Game.ShowChatMessage($"Player '{tokens[1]}' not found.", Color.Red, args.User.UserIdentifier);
                    return;
                }

                targetPos = target.GetWorldPosition();
                targetLabel = target.Name;
            }
            else
            {
                IPlayer self = args.User.GetPlayer();

                if (self == null || self.IsRemoved)
                {
                    Game.ShowChatMessage("You have no live player to teleport to.", Color.Red, args.User.UserIdentifier);
                    return;
                }

                targetPos = self.GetWorldPosition();
                targetLabel = "you";
            }

            int affected = 0;

            foreach (IPlayer player in sources)
            {
                if (player == null || player.IsRemoved) continue;

                Vector2 from = player.GetWorldPosition();
                player.SetWorldPosition(targetPos);
                affected++;

                PointShape.Trail(pos => Game.PlayEffect(EffectName.ItemGleam, pos), from, targetPos, 15f);
            }

            Game.ShowChatMessage($"Teleported {affected} player(s) to {targetLabel}.", Color.Green, args.User.UserIdentifier);
        }
    }
}
