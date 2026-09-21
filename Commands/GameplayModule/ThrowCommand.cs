using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void ThrowCommand(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /throw [bool]", Color.Red, uid);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !ThrowRule.IsEnabled;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /throw [bool]", Color.Red, uid);
                return;
            }

            if (enable == ThrowRule.IsEnabled)
            {
                Game.ShowChatMessage($"Throwing is already {(enable ? "disabled" : "enabled")}.", Color.Yellow, uid);
                return;
            }

            ThrowRule.IsEnabled = enable;

            Game.ShowChatMessage($"Throwing {(enable ? "disabled" : "enabled")}.", Color.Green, uid);
        }
    }
}
