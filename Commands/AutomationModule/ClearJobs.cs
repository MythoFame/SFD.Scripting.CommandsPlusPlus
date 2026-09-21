using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class AutomationModule
    {
        private static void ClearJobs(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 0)
            {
                Game.ShowChatMessage("Usage: /clear_jobs", Color.Red, uid);
                return;
            }

            JobsRule.Clear();

            Game.ShowChatMessage("Removed all jobs.", Color.Green, uid);
        }
    }
}
