using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Utils.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Player;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void User(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 2)
            {
                Game.ShowChatMessage("Usage: /user <from> <to>", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] sources = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            IPlayer[] targets = [.. ParseHelper.ParsePlayers(tokens[1], args.User)];

            if (sources.Length != 1 || targets.Length != 1)
            {
                Game.ShowChatMessage("Target a single player on each side.", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer source = sources[0];
            IPlayer target = targets[0];

            if (source == null || source.IsRemoved || target == null || target.IsRemoved)
            {
                Game.ShowChatMessage("Target a single live player on each side.", Color.Red, args.User.UserIdentifier);
                return;
            }

            IUser fromUser = source.GetUser();
            IUser toUser = target.GetUser();

            if (fromUser == null || toUser == null)
            {
                Game.ShowChatMessage("Both players must have a user.", Color.Red, args.User.UserIdentifier);
                return;
            }

            source.SetUser(toUser);
            target.SetUser(fromUser);

            Game.ShowChatMessage($"Swapped users of {source.Name} and {target.Name}.", Color.Green, args.User.UserIdentifier);
        }
    }
}
