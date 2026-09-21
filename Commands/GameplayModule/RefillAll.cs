using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void RefillAll(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /refill_all [bool]", Color.Red, uid);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !RefillAllRule.IsEnabled;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /refill_all [bool]", Color.Red, uid);
                return;
            }

            if (enable == RefillAllRule.IsEnabled)
            {
                Game.ShowChatMessage($"Ammo refilling is already {(enable ? "enabled" : "disabled")}.", Color.Yellow, uid);
                return;
            }

            RefillAllRule.IsEnabled = enable;

            Game.ShowChatMessage($"Ammo refilling {(enable ? "enabled" : "disabled")}.", Color.Green, uid);
        }
    }
}
