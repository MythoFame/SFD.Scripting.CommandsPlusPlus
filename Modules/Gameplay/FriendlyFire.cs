using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void FriendlyFire(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /friendly_fire [true|false]", Color.Red, uid);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !FriendlyFireRule.IsEnabled;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /friendly_fire [true|false]", Color.Red, uid);
                return;
            }

            if (enable == FriendlyFireRule.IsEnabled)
            {
                Game.ShowChatMessage($"Friendly fire is already {(enable ? "disabled" : "enabled")}.", Color.Yellow, uid);
                return;
            }

            FriendlyFireRule.IsEnabled = enable;

            Game.ShowChatMessage($"Friendly fire {(enable ? "disabled" : "enabled")}.", Color.Green, uid);
        }
    }
}
