using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Grab(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /grab [bool]", Color.Red, uid);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !GrabRule.IsEnabled;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /grab [bool]", Color.Red, uid);
                return;
            }

            if (enable == GrabRule.IsEnabled)
            {
                Game.ShowChatMessage($"Grabbing is already {(enable ? "disabled" : "enabled")}.", Color.Yellow, uid);
                return;
            }

            GrabRule.IsEnabled = enable;

            Game.ShowChatMessage($"Grabbing {(enable ? "disabled" : "enabled")}.", Color.Green, uid);
        }
    }
}
