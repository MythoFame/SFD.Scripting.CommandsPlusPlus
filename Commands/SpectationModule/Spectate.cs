using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class SpectationModule
    {
        private static void Spectate(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /spectate [user]", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (tokens.Length == 0)
            {
                string account = args.User.AccountName;

                if (SpectationRule.Spectating.Contains(account, StringComparer.OrdinalIgnoreCase))
                {
                    SpectationRule.Spectating = [.. SpectationRule.Spectating
                        .Where(n => !string.Equals(n, account, StringComparison.OrdinalIgnoreCase))];

                    Game.ShowChatMessage("You will play next round.", Color.Green, args.User.UserIdentifier);
                }
                else
                {
                    SpectationRule.Spectating = [.. SpectationRule.Spectating, account];

                    Game.ShowChatMessage("You will spectate next round.", Color.Green, args.User.UserIdentifier);
                }

                return;
            }

            if (!args.User.IsModerator)
            {
                Game.ShowChatMessage("You don't have permission to use this command.", Color.Red, args.User.UserIdentifier);
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

            if (target == null)
            {
                Game.ShowChatMessage($"User '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            string targetAccount = target.AccountName;

            if (SpectationRule.Spectating.Contains(targetAccount, StringComparer.OrdinalIgnoreCase))
            {
                SpectationRule.Spectating = [.. SpectationRule.Spectating
                    .Where(n => !string.Equals(n, targetAccount, StringComparison.OrdinalIgnoreCase))];

                Game.ShowChatMessage($"{target.Name} will play next round.", Color.Green, args.User.UserIdentifier);
                return;
            }

            SpectationRule.Spectating = [.. SpectationRule.Spectating, targetAccount];

            Game.ShowChatMessage($"{target.Name} will spectate next round.", Color.Green, args.User.UserIdentifier);
        }
    }
}
