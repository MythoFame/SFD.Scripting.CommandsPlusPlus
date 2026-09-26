using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Speech(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /speech [true|false]", Color.Red, uid);
                return;
            }

            bool enable;

            if (tokens.Length == 0)
            {
                enable = !SpeechRule.IsEnabled;
            }
            else if (!bool.TryParse(tokens[0], out enable))
            {
                Game.ShowChatMessage("Usage: /speech [true|false]", Color.Red, uid);
                return;
            }
            else
            {
                SpeechRule.PlaySound = enable;
            }

            SpeechRule.IsEnabled = enable;

            string silent = enable && !SpeechRule.PlaySound ? " (silent)" : string.Empty;

            Game.ShowChatMessage($"Speech bubbles {(enable ? "enabled" : "disabled")}{silent}.", Color.Green, uid);
        }
    }
}
