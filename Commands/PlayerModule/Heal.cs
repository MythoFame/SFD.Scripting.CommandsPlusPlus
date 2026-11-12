using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static readonly Color _healColor = new(0xb0, 0xed, 0x17);
        private static readonly Vector2 _healFxOffset = new(0, 26);

        private static void Heal(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length < 1 || tokens.Length > 2)
            {
                Game.ShowChatMessage("Usage: /heal <player> [amount]", Color.Red, uid);
                return;
            }

            float? amount = null;

            if (tokens.Length == 2)
            {
                if (!float.TryParse(tokens[1], out float parsed) || parsed <= 0)
                {
                    Game.ShowChatMessage($"Invalid amount '{tokens[1]}'.", Color.Red, uid);
                    return;
                }

                amount = parsed;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            int affected = 0;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                float current = player.GetHealth();
                float max = player.GetMaxHealth();

                float healed = Math.Max(amount == null
                    ? max - current
                    : Math.Min(amount.Value, max - current), 0);

                player.SetHealth(current + healed);

                if (healed > 0)
                {
                    Vector2 pos = player.GetWorldPosition() + _healFxOffset;

                    EffectsWrapper.PlayCustomFloatText(pos, $"+{MathF.Round(healed)}", _healColor);
                    Game.PlaySound(SoundsDatabase.GetHealthLarge, pos);
                }

                affected++;
            }

            if (affected == 0) return;

            Game.ShowChatMessage($"Healed {affected} player(s).", Color.Green, uid);
        }
    }
}
