using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class SpectationModule
    {
        private static void SpectateRmWhitelist(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /spectate_rm_whitelist <account>", Color.Red, uid);
                return;
            }

            if (tokens[0] == "*")
            {
                SpectationRule.Whitelist = [];

                Game.ShowChatMessage("Whitelist cleared.", Color.Green, uid);
                return;
            }

            if (!SpectationRule.Whitelist.Contains(tokens[0]))
            {
                Game.ShowChatMessage($"'{tokens[0]}' is not in the whitelist.", Color.Yellow, uid);
                return;
            }

            SpectationRule.Whitelist = [.. SpectationRule.Whitelist.Where(n => n != tokens[0])];

            Game.ShowChatMessage($"Removed '{tokens[0]}' from the whitelist.", Color.Green, uid);
        }
    }
}
