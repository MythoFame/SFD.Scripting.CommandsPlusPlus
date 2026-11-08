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
                Game.ShowChatMessage("Usage: /grab [true|false]", Color.Red, uid);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !GrabRule.IsAllowed;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /grab [true|false]", Color.Red, uid);
                return;
            }

            if (enable == GrabRule.IsAllowed)
            {
                Game.ShowChatMessage($"Grabbing is already {(enable ? "enabled" : "disabled")}.", Color.Yellow, uid);
                return;
            }

            GrabRule.IsAllowed = enable;

            Game.ShowChatMessage($"Grabbing {(enable ? "enabled" : "disabled")}.", Color.Green, uid);
        }
    }
}
