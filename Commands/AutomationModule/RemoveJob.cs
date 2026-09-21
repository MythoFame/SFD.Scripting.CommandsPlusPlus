using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class AutomationModule
    {
        private static void RemoveJob(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1 || !int.TryParse(tokens[0], out int index) || !JobsRule.Remove(index))
            {
                Game.ShowChatMessage("Usage: /remove_job <index>", Color.Red, uid);
                return;
            }

            Game.ShowChatMessage($"Removed job {index}.", Color.Green, uid);
        }
    }
}
