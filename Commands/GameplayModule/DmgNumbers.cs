using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void DmgNumbers(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length > 2)
            {
                Game.ShowChatMessage("Usage: /dmg_numbers [bool] [players|objects|all]", Color.Red, args.User.UserIdentifier);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !DmgNumbersRule.IsEnabled;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /dmg_numbers [bool] [players|objects|all]", Color.Red, args.User.UserIdentifier);
                return;
            }

            DmgNumbersRule.DamageTarget target = DmgNumbersRule.DamageTarget.All;

            if (tokens.Length > 1)
            {
                if (string.Equals(tokens[1], "players", StringComparison.OrdinalIgnoreCase))
                    target = DmgNumbersRule.DamageTarget.Players;
                else if (string.Equals(tokens[1], "objects", StringComparison.OrdinalIgnoreCase))
                    target = DmgNumbersRule.DamageTarget.Objects;
                else if (!string.Equals(tokens[1], "all", StringComparison.OrdinalIgnoreCase))
                {
                    Game.ShowChatMessage("Filter must be 'players', 'objects' or 'all'.", Color.Red, args.User.UserIdentifier);
                    return;
                }
            }

            DmgNumbersRule.Target = target;

            if (tokens.Length < 2 && enable == DmgNumbersRule.IsEnabled)
            {
                Game.ShowChatMessage($"Damage numbers are already {(enable ? "displayed" : "hidden")}.", Color.Yellow, args.User.UserIdentifier);
                return;
            }

            DmgNumbersRule.IsEnabled = enable;

            string scope = enable && target != DmgNumbersRule.DamageTarget.All
                ? $" for {target.ToString().ToLowerInvariant()} only"
                : string.Empty;

            Game.ShowChatMessage($"Damage numbers {(enable ? "displayed" : "hidden")}{scope}.", Color.Green, args.User.UserIdentifier);
        }
    }
}
