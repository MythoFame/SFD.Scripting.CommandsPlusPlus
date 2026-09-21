using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class SpectationModule
    {
        private static void SpectateWhitelist(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 0)
            {
                Game.ShowChatMessage("Usage: /spectate_whitelist", Color.Red, uid);
                return;
            }

            SpectationRule.WhitelistOnly = !SpectationRule.WhitelistOnly;

            Game.ShowChatMessage($"Whitelist {(SpectationRule.WhitelistOnly ? "enabled" : "disabled")}.", Color.Green, uid);

            string[] whitelist = SpectationRule.Whitelist;

            if (SpectationRule.WhitelistOnly && whitelist.Length > 0)
                Game.ShowChatMessage($"Whitelisted: {string.Join(", ", whitelist)}.", Color.Green, uid);
        }
    }
}
