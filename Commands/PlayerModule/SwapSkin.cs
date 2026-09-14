using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void SwapSkin(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 2)
            {
                Game.ShowChatMessage("Usage: /swap_skin <from> <to>", Color.Red, args.User.UserIdentifier);
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

            IProfile sourceProfile = source.GetProfile();
            IProfile targetProfile = target.GetProfile();

            source.SetProfile(targetProfile);
            target.SetProfile(sourceProfile);

            Game.ShowChatMessage($"Swapped profiles of {source.Name} and {target.Name}.", Color.Green, args.User.UserIdentifier);
        }
    }
}
