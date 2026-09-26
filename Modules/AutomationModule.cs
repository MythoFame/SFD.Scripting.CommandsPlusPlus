namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Schedule commands to run automatically when events happen. Great for
    /// announcements, cleanup or timed effects. Host-only, persisted.
    /// Command implementations live in <c>Commands/AutomationModule/</c> as
    /// partial declarations of this class.
    /// </summary>
    public sealed partial class AutomationModule : CommandsModule
    {
        public override string Name => "Automation";

        public override string Description => "Schedule commands to run automatically on game events.";

        public AutomationModule()
        {
            // AddCommand("jobs", Jobs,
            //     "- Lists all jobs along with their index, trigger, arguments and command.",
            //     hostOnly: true);
            // AddCommand("add_job", AddJob,
            //     "{startup|shutdown|gameover|spawn|time} <args> <command...> - Adds a job that runs a command on a certain trigger. Arguments depend on the trigger.",
            //     hostOnly: true);
            // AddCommand("remove_job", RemoveJob,
            //     "<index> - Removes the job with the given index, * removes all.",
            //     hostOnly: true);
        }
    }
}
