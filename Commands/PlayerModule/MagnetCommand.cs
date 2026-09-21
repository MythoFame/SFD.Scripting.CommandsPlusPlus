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
                Game.ShowChatMessage($"Usage: /{command} <player> [area_size] [players|objects]", Color.Red, args.User.UserIdentifier);
                return;
            }

            float areaSize = 0f;

            if (tokens.Length > 1 && (!float.TryParse(tokens[1], out areaSize) || areaSize <= 0))
            {
                Game.ShowChatMessage("Area size must be a positive number.", Color.Red, args.User.UserIdentifier);
                return;
            }

            bool playersOnly = false;
            bool objectsOnly = false;

            if (tokens.Length > 2)
            {
                if (string.Equals(tokens[2], "players", StringComparison.OrdinalIgnoreCase))
                    playersOnly = true;
                else if (string.Equals(tokens[2], "objects", StringComparison.OrdinalIgnoreCase))
                    objectsOnly = true;
                else
                {
                    Game.ShowChatMessage("Filter must be 'players' or 'objects'.", Color.Red, args.User.UserIdentifier);
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

                if (playersOnly || objectsOnly)
                {
                    existing.AffectPlayers = playersOnly;
                    existing.AffectObjects = objectsOnly;
                }

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

            if (affected == 1)
            {
                Game.ShowChatMessage($"{label} {(lastState ? "enabled" : "disabled")} for {lastName}.",
                    Color.Green, args.User.UserIdentifier);
            }
            else
            {
                Game.ShowChatMessage($"{label} updated for {affected} player(s).",
                    Color.Green, args.User.UserIdentifier);
            }
        }
    }
}
