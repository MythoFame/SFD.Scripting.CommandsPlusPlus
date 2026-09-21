using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Gmover(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /gmover {true|false|players}", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (string.Equals(tokens[0], "players", StringComparison.OrdinalIgnoreCase))
            {
                GmoverRule.IsEnabled = !GmoverRule.IsEnabled;

                Game.ShowChatMessage($"Players-only game over {(GmoverRule.IsEnabled ? "enabled" : "disabled")}.", Color.Green, args.User.UserIdentifier);
                return;
            }

            if (!bool.TryParse(tokens[0], out bool enable))
            {
                Game.ShowChatMessage("Usage: /gmover {true|false|players}", Color.Red, args.User.UserIdentifier);
                return;
            }

            Game.AutoVictoryConditionEnabled = enable;

            Game.ShowChatMessage($"Game over {(enable ? "enabled" : "disabled")} for the current round.", Color.Green, args.User.UserIdentifier);
        }
    }
}
