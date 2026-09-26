using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class AutomationModule
    {
        private static void Jobs(UserMessageCallbackArgs args)
        {
            int uid = args.User?.UserIdentifier ?? -1;
            if (JobsRule.Jobs.Count == 0)
            {
                Game.ShowChatMessage("No jobs.", Color.Green, uid);
                return;
            }

            for (int i = 0; i < JobsRule.Jobs.Count; i++)
            {
                JobsRule.Job job = JobsRule.Jobs[i];
                string detail = string.IsNullOrEmpty(job.TriggerArgs) ? job.Trigger.ToString() : $"{job.Trigger} {job.TriggerArgs}";
                string command = string.IsNullOrEmpty(job.CommandArgs) ? job.Command : $"{job.Command} {job.CommandArgs}";

                Game.ShowChatMessage($"{i}: [{detail}] /{command}", Color.Green, uid);
            }
        }
    }
}
