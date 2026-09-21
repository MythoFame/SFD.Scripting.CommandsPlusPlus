using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void ThrowCommand(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /throw [bool]", Color.Red, args.User.UserIdentifier);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !ThrowRule.IsEnabled;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /throw [bool]", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (enable == ThrowRule.IsEnabled)
            {
                Game.ShowChatMessage($"Throwing is already {(enable ? "disabled" : "enabled")}.", Color.Yellow, args.User.UserIdentifier);
                return;
            }

            ThrowRule.IsEnabled = enable;

            Game.ShowChatMessage($"Throwing {(enable ? "disabled" : "enabled")}.", Color.Green, args.User.UserIdentifier);
        }
    }
}
