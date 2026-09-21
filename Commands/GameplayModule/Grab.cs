using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Grab(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /grab [bool]", Color.Red, args.User.UserIdentifier);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !GrabRule.IsEnabled;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /grab [bool]", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (enable == GrabRule.IsEnabled)
            {
                Game.ShowChatMessage($"Grabbing is already {(enable ? "disabled" : "enabled")}.", Color.Yellow, args.User.UserIdentifier);
                return;
            }

            GrabRule.IsEnabled = enable;

            Game.ShowChatMessage($"Grabbing {(enable ? "disabled" : "enabled")}.", Color.Green, args.User.UserIdentifier);
        }
    }
}
