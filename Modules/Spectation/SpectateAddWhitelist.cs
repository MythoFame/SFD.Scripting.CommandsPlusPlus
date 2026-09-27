using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class SpectationModule
    {
        private static void SpectateAddWhitelist(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /spectate_add_whitelist <user>", Color.Red, uid);
                return;
            }

            IUser target = null;

            foreach (IUser candidate in ParseHelper.ParseUsers(tokens[0], args.User))
            {
                if (candidate != null && !candidate.IsRemoved)
                {
                    target = candidate;
                    break;
                }
            }

            string account;
            string label;

            if (target == null)
            {
                account = tokens[0];
                label = $"'{tokens[0]}'";
            }
            else
            {
                account = target.AccountName;
                label = target.Name;
            }

            if (SpectationRule.Whitelist.Contains(account))
            {
                Game.ShowChatMessage($"{label} is already whitelisted.", Color.Yellow, uid);
                return;
            }

            SpectationRule.Whitelist = [.. SpectationRule.Whitelist, account];

            if (target == null)
                Game.ShowChatMessage($"Player '{tokens[0]}' not found. Added it to the whitelist as an account name.", Color.Green, uid);
            else
                Game.ShowChatMessage($"Added {target.Name} to the whitelist.", Color.Green, uid);
        }
    }
}
