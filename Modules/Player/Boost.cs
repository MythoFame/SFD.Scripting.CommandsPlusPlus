using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private const float DEFAULT_SPEED = 15f;

        private static void Boost(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length < 2 || tokens.Length > 3)
            {
                Game.ShowChatMessage("Usage: /boost <player> <left|down|up|right> [speed]", Color.Red, uid);
                return;
            }

            Vector2 direction;
            bool horizontal = false;

            if (string.Equals(tokens[1], "left", StringComparison.OrdinalIgnoreCase))
            {
                direction = Vector2Helper.Left;
                horizontal = true;
            }
            else if (string.Equals(tokens[1], "right", StringComparison.OrdinalIgnoreCase))
            {
                direction = Vector2Helper.Right;
                horizontal = true;
            }
            else if (string.Equals(tokens[1], "up", StringComparison.OrdinalIgnoreCase))
                direction = Vector2Helper.Up;
            else if (string.Equals(tokens[1], "down", StringComparison.OrdinalIgnoreCase))
                direction = Vector2Helper.Down;
            else
            {
                Game.ShowChatMessage("Direction must be 'left', 'down', 'up' or 'right'.", Color.Red, uid);
                return;
            }

            float speed = DEFAULT_SPEED;

            if (tokens.Length > 2 && !float.TryParse(tokens[2], out speed))
            {
                Game.ShowChatMessage($"Invalid speed '{tokens[2]}'.", Color.Red, uid);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            int affected = 0;
            string lastName = string.Empty;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                PlayerHelper.Unstick(player);

                Vector2 velocity = direction * speed;

                if (horizontal)
                    velocity = new(velocity.X, speed / 2f);

                player.SetLinearVelocity(velocity);
                EffectsWrapper.PlayTraceSpawner(player, EffectName.DustTrail, 2f);

                lastName = player.Name;
                affected++;
            }

            if (affected == 0) return;

            if (affected == 1)
                Game.ShowChatMessage($"Boosted {lastName} {tokens[1].ToLowerInvariant()} at {speed}.", Color.Green, uid);
            else
                Game.ShowChatMessage($"Boosted {affected} player(s) {tokens[1].ToLowerInvariant()} at {speed}.", Color.Green, uid);
        }
    }
}
