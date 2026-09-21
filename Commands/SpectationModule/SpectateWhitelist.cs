using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class SpectationModule
    {
        private static void SpectateWhitelist(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 0)
            {
                Game.ShowChatMessage("Usage: /spectate_whitelist", Color.Red, args.User.UserIdentifier);
                return;
            }

            SpectationRule.WhitelistOnly = !SpectationRule.WhitelistOnly;

            Game.ShowChatMessage($"Whitelist {(SpectationRule.WhitelistOnly ? "enabled" : "disabled")}.", Color.Green, args.User.UserIdentifier);

            string[] whitelist = SpectationRule.Whitelist;

            if (SpectationRule.WhitelistOnly && whitelist.Length > 0)
                Game.ShowChatMessage($"Whitelisted: {string.Join(", ", whitelist)}.", Color.Green, args.User.UserIdentifier);
        }
    }
}
