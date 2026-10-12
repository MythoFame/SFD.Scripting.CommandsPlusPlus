using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Win(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /win <user>", Color.Red, uid);
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
                Game.ShowChatMessage($"User '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            target.IncreaseScore();

            Game.ShowChatMessage($"{target.Name} will win upon game over.", Color.Green, uid);
        }
    }
}
