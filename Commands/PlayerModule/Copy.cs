using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Copy(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 2)
            {
                Game.ShowChatMessage("Usage: /copy <from> <to>", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer source = null;

            foreach (IPlayer candidate in ParseHelper.ParsePlayers(tokens[0], args.User))
            {
                if (candidate != null && !candidate.IsRemoved)
                {
                    source = candidate;
                    break;
                }
            }

            if (source == null)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] targets = [.. ParseHelper.ParsePlayers(tokens[1], args.User)];

            if (targets.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[1]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            IProfile profile = source.GetProfile();
            int affected = 0;

            foreach (IPlayer target in targets)
            {
                if (target == null || target.IsRemoved) continue;

                target.SetProfile(profile);
                affected++;
            }

            Game.ShowChatMessage($"Copied {source.Name}'s profile onto {affected} player(s).", Color.Green, args.User.UserIdentifier);
        }
    }
}
