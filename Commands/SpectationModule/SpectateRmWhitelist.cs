using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class SpectationModule
    {
        private static void SpectateRmWhitelist(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /spectate_rm_whitelist <account>", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (tokens[0] == "*")
            {
                SpectationRule.Whitelist = [];

                Game.ShowChatMessage("Whitelist cleared.", Color.Green, args.User.UserIdentifier);
                return;
            }

            if (!SpectationRule.Whitelist.Contains(tokens[0]))
            {
                Game.ShowChatMessage($"'{tokens[0]}' is not in the whitelist.", Color.Yellow, args.User.UserIdentifier);
                return;
            }

            SpectationRule.Whitelist = [.. SpectationRule.Whitelist.Where(n => n != tokens[0])];

            Game.ShowChatMessage($"Removed '{tokens[0]}' from the whitelist.", Color.Green, args.User.UserIdentifier);
        }
    }
}
