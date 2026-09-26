using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Utils.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Player;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Swap(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 2)
            {
                Game.ShowChatMessage("Usage: /swap <from> <to>", Color.Red, uid);
                return;
            }

            IPlayer[] sources = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            IPlayer[] targets = [.. ParseHelper.ParsePlayers(tokens[1], args.User)];

            if (sources.Length != 1 || targets.Length != 1)
            {
                Game.ShowChatMessage("Target a single player on each side.", Color.Red, uid);
                return;
            }

            IPlayer source = sources[0];
            IPlayer target = targets[0];

            if (source == null || source.IsRemoved || target == null || target.IsRemoved)
            {
                Game.ShowChatMessage("Target a single live player on each side.", Color.Red, uid);
                return;
            }

            IProfile sourceProfile = source.GetProfile();
            IProfile targetProfile = target.GetProfile();

            source.SetProfile(targetProfile);
            target.SetProfile(sourceProfile);

            Game.ShowChatMessage($"Swapped profiles of {source.Name} and {target.Name}.", Color.Green, uid);
        }
    }
}
