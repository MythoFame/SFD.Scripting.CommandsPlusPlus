
using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Player interaction commands (kill, revive, teleport, ...).
    /// </summary>
    public sealed class PlayerModule : CommandsModule
    {
        public override string Name => "Player";

        public override string Description => "Player module";

        public PlayerModule()
        {
            AddCommand("kill", Kill,
                "- Kills a player, optionally gibbing or removing them",
                moderatorOnly: true);
            AddCommand("tp", Tp,
                "- Teleports a player to another player, or to you if no target is given",
                moderatorOnly: true);
        }

        public override void OnEnable()
        {
        }

        public override void OnDisable()
        {
        }

        private static void Kill(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length == 0 || tokens.Length > 2)
            {
                Game.ShowChatMessage("Usage: /kill <player> [gib|remove]", Color.Red, args.User.UserIdentifier);
                return;
            }

            string mode = tokens.Length == 2 ? tokens[1] : string.Empty;

            if (mode.Length != 0
                && !string.Equals(mode, "gib", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(mode, "remove", StringComparison.OrdinalIgnoreCase))
            {
                Game.ShowChatMessage("Usage: /kill <player> [gib|remove]", Color.Red, args.User.UserIdentifier);
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

                if (string.Equals(mode, "gib", StringComparison.OrdinalIgnoreCase))
                    player.Gib();
                else if (string.Equals(mode, "remove", StringComparison.OrdinalIgnoreCase))
                    player.Remove();
                else
                    player.Kill();

                affected++;
            }

            string verb = string.Equals(mode, "gib", StringComparison.OrdinalIgnoreCase) ? "Gibbed"
                : string.Equals(mode, "remove", StringComparison.OrdinalIgnoreCase) ? "Removed"
                : "Killed";

            Game.ShowChatMessage($"{verb} {affected} player(s).", Color.Green, args.User.UserIdentifier);
        }

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

                player.SetWorldPosition(targetPos);
                affected++;
            }

            Game.ShowChatMessage($"Teleported {affected} player(s) to {targetLabel}.", Color.Green, args.User.UserIdentifier);
        }
    }
}
