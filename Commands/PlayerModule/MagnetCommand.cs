using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void MagnetCommand(UserMessageCallbackArgs args) => MagnetControl(args, attract: true);

        /// <summary>
        /// Shared core for <c>/magnet</c> and <c>/repulse</c>. Each command
        /// toggles its own mode: re-invoking while that mode is active
        /// disables it, otherwise the command's direction is applied (with
        /// any provided settings) and the ability is enabled.
        /// </summary>
        private static void MagnetControl(UserMessageCallbackArgs args, bool attract)
        {
            string command = attract ? "magnet" : "repulse";
            string label = attract ? "Magnet" : "Repulse";
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length == 0 || tokens.Length > 3)
            {
                Game.ShowChatMessage($"Usage: /{command} <player> [area_size] [players|objects|all]", Color.Red, args.User.UserIdentifier);
                return;
            }

            float areaSize = 0f;

            if (tokens.Length > 1 && (!float.TryParse(tokens[1], out areaSize) || areaSize <= 0))
            {
                Game.ShowChatMessage("Area size must be a positive number.", Color.Red, args.User.UserIdentifier);
                return;
            }

            Magnet.Target target = Magnet.Target.All;

            if (tokens.Length > 2)
            {
                if (string.Equals(tokens[2], "players", StringComparison.OrdinalIgnoreCase))
                    target = Magnet.Target.Players;
                else if (string.Equals(tokens[2], "objects", StringComparison.OrdinalIgnoreCase))
                    target = Magnet.Target.Objects;
                else if (!string.Equals(tokens[2], "all", StringComparison.OrdinalIgnoreCase))
                {
                    Game.ShowChatMessage("Filter must be 'players', 'objects' or 'all'.", Color.Red, args.User.UserIdentifier);
                    return;
                }
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            // Drop abilities bound to gone players so stale references never linger.
            Magnet.PurgeStale();

            int affected = 0;
            string lastName = string.Empty;
            bool lastState = false;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                // Fresh instances start disabled (see constructor), so the
                // toggle below only ever fires on previously known ones.
                Magnet existing = Magnet.Find(player.UniqueID) ?? new Magnet(player);

                if (tokens.Length > 1)
                    existing.AreaSize = areaSize;

                existing.Targets = target;

                if (existing.Enabled && Math.Sign(existing.Force) == (attract ? 1 : -1))
                    existing.Enabled = false;
                else
                {
                    existing.Force = attract ? Math.Abs(existing.Force) : -Math.Abs(existing.Force);
                    existing.Enabled = true;
                }

                lastName = player.Name;
                lastState = existing.Enabled;
                affected++;
            }

            if (affected == 0) return;

            string scope = target == Magnet.Target.All ? "all targets" : $"{target.ToString().ToLowerInvariant()} only";

            if (affected == 1)
            {
                Game.ShowChatMessage($"{label} {(lastState ? "enabled" : "disabled")} for {lastName} ({scope}).",
                    Color.Green, args.User.UserIdentifier);
            }
            else
            {
                Game.ShowChatMessage($"{label} updated for {affected} player(s) ({scope}).",
                    Color.Green, args.User.UserIdentifier);
            }
        }
    }
}
