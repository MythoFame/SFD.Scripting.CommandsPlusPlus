
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
                "<player> [gib|remove] - Kills a player, optionally gibbing or removing them",
                moderatorOnly: true);
            AddCommand("tp", Tp,
                "<from> [to] - Teleports a player to another player, or to you if no target is given",
                moderatorOnly: true);
            AddCommand("trip", Trip,
                "<player> - Trips a player, knocking them down",
                moderatorOnly: true);
            AddCommand("input", Input,
                "<player> - Toggles whether a player can provide input",
                moderatorOnly: true);
            AddCommand("team", Team,
                "<player> <team> - Sets the team of a player",
                moderatorOnly: true);
            AddCommand("burn", Burn,
                "<player> - Toggles whether a player is burning",
                moderatorOnly: true);
            AddCommand("spawn", Spawn,
                "<id> - Spawns an object with the given ID at your position",
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

                Vector2 from = player.GetWorldPosition();
                player.SetWorldPosition(targetPos);
                affected++;

                PointShape.Trail(pos => Game.PlayEffect(EffectName.ItemGleam, pos), from, targetPos, 15f);
            }

            Game.ShowChatMessage($"Teleported {affected} player(s) to {targetLabel}.", Color.Green, args.User.UserIdentifier);
        }

        private static void Trip(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /trip <player>", Color.Red, args.User.UserIdentifier);
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

                player.Fall();
                affected++;
            }

            Game.ShowChatMessage($"Tripped {affected} player(s).", Color.Green, args.User.UserIdentifier);
        }

        private static void Input(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /input <player>", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            int affected = 0;
            string lastName = string.Empty;
            bool lastState = false;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                player.SetInputEnabled(!player.IsInputEnabled);
                lastName = player.Name;
                lastState = player.IsInputEnabled;
                affected++;
            }

            if (affected == 0) return;

            if (affected == 1)
            {
                Game.ShowChatMessage($"Input {(lastState ? "enabled" : "disabled")} for {lastName}.",
                    Color.Green, args.User.UserIdentifier);
            }
            else
            {
                Game.ShowChatMessage($"Toggled input for {affected} player(s).",
                    Color.Green, args.User.UserIdentifier);
            }
        }

        private static void Team(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 2)
            {
                Game.ShowChatMessage("Usage: /team <player> <team>", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (!Enum.TryParse(tokens[1], true, out PlayerTeam team) || !Enum.IsDefined(team))
            {
                Game.ShowChatMessage("Invalid team. Use independent (or 0) or team 1-8.", Color.Red, args.User.UserIdentifier);
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

                player.SetTeam(team);
                affected++;
            }

            Game.ShowChatMessage($"Set {affected} player(s) to {team}.", Color.Green, args.User.UserIdentifier);
        }

        private static void Burn(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /burn <player>", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            int affected = 0;
            string lastName = string.Empty;
            bool lastState = false;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                if (player.IsBurning)
                    player.ClearFire();
                else
                    player.SetMaxFire();

                lastName = player.Name;
                lastState = player.IsBurning;
                affected++;
            }

            if (affected == 0) return;

            if (affected == 1)
            {
                Game.ShowChatMessage($"{lastName} is {(lastState ? "now" : "no longer")} burning.",
                    Color.Green, args.User.UserIdentifier);
            }
            else
            {
                Game.ShowChatMessage($"Toggled burning for {affected} player(s).",
                    Color.Green, args.User.UserIdentifier);
            }
        }

        private static void Spawn(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /spawn <id>", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer self = args.User.GetPlayer();

            if (self == null || self.IsRemoved)
            {
                Game.ShowChatMessage("You have no live player to spawn at.", Color.Red, args.User.UserIdentifier);
                return;
            }

            IObject obj = Game.CreateObject(tokens[0], Vector2.Zero);

            Area aabb = obj.GetAABB();
            Vector2 playerPos = self.GetWorldPosition();
            obj.SetWorldPosition(new(playerPos.X + (aabb.Width / 2f + 16f) * self.FacingDirection, playerPos.Y));

            Game.ShowChatMessage($"Spawned '{tokens[0]}'.", Color.Green, args.User.UserIdentifier);
        }
    }
}
