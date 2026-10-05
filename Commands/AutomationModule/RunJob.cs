using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class AutomationModule
    {
        private static void RunJob(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /run_job <index>", Color.Red, uid);
                return;
            }

            if (!int.TryParse(tokens[0], out int index) || !JobsRule.Run(index))
            {
                Game.ShowChatMessage("Usage: /run_job <index>", Color.Red, uid);
                return;
            }

            Game.ShowChatMessage($"Ran job {index}.", Color.Green, uid);
        }
    }
}
