using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Gmover(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /gmover {true|false|players}", Color.Red, uid);
                return;
            }

            if (string.Equals(tokens[0], "players", StringComparison.OrdinalIgnoreCase))
            {
                GmoverRule.IsEnabled = !GmoverRule.IsEnabled;

                Game.ShowChatMessage($"Players-only game over {(GmoverRule.IsEnabled ? "enabled" : "disabled")}.", Color.Green, uid);
                return;
            }

            if (!bool.TryParse(tokens[0], out bool enable))
            {
                Game.ShowChatMessage("Usage: /gmover {true|false|players}", Color.Red, uid);
                return;
            }

            Game.AutoVictoryConditionEnabled = enable;

            Game.ShowChatMessage($"Game over {(enable ? "enabled" : "disabled")} for the current round.", Color.Green, uid);
        }
    }
}
