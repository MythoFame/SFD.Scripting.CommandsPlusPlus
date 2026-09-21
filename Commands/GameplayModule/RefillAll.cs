using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void RefillAll(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /refill_all [bool]", Color.Red, args.User.UserIdentifier);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !RefillAllRule.IsEnabled;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /refill_all [bool]", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (enable == RefillAllRule.IsEnabled)
            {
                Game.ShowChatMessage($"Ammo refilling is already {(enable ? "enabled" : "disabled")}.", Color.Yellow, args.User.UserIdentifier);
                return;
            }

            RefillAllRule.IsEnabled = enable;

            Game.ShowChatMessage($"Ammo refilling {(enable ? "enabled" : "disabled")}.", Color.Green, args.User.UserIdentifier);
        }
    }
}
