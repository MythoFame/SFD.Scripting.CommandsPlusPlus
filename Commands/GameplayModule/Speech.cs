using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Speech(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /speech [bool]", Color.Red, args.User.UserIdentifier);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !SpeechRule.IsEnabled;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /speech [bool]", Color.Red, args.User.UserIdentifier);
                return;
            }
            else
            {
                SpeechRule.PlaySound = enable;
            }

            SpeechRule.IsEnabled = enable;

            string silent = enable && !SpeechRule.PlaySound ? " (silent)" : string.Empty;

            Game.ShowChatMessage($"Speech bubbles {(enable ? "enabled" : "disabled")}{silent}.", Color.Green, args.User.UserIdentifier);
        }
    }
}
