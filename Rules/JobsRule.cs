using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Runs stored commands on triggers. The rule itself is always enabled;
    /// it only returns early when the Automation module is restricted.
    /// </summary>
    public static class JobsRule
    {
        public enum JobTrigger
        {
            Startup,
            Shutdown,
            GameOver,
            Spawn,
            Time
        }

        public class Job
        {
            public JobTrigger Trigger;
            public string TriggerArgs = string.Empty;
            public string Command = string.Empty;
            public string CommandArgs = string.Empty;

            /// <summary>Timer token for time jobs. In-memory only, restarted on every load.</summary>
            public Events.UpdateCallback Timer = null;
        }

        private const string TRIGGERS_KEY = "CommandsPlusPlus.Jobs.Triggers";
        private const string TRIGGER_ARGS_KEY = "CommandsPlusPlus.Jobs.TriggerArgs";
        private const string COMMANDS_KEY = "CommandsPlusPlus.Jobs.Commands";
        private const string COMMAND_ARGS_KEY = "CommandsPlusPlus.Jobs.CommandArgs";

        /// <summary>Placeholder replaced with the spawned player's name in spawn jobs.</summary>
        private const string PlayerToken = "@@PLAYER@@";

        /// <summary>Whether trigger hooks are live. Timers only start when initialized.</summary>
        private static bool _initialized;

        /// <summary>All stored jobs.</summary>
        public static readonly List<Job> Jobs = [];

        /// <summary>
        /// Loads jobs and starts trigger hooks. Call once when the world is
        /// live. Startup jobs run directly.
        /// </summary>
        public static void Initialize()
        {
            if (ModuleRegistry.TryGet("Automation", out CommandsModule module) && module.IsRestricted)
                return;

            Load();

            foreach (Job job in Jobs)
            {
                if (job.Trigger == JobTrigger.Startup)
                    Run(job);
            }

            Game.Events.StartPlayerCreatedCallback(OnPlayerCreated);
            GameOverCallback.Start(() => Fire(JobTrigger.GameOver));

            foreach (Job job in Jobs)
            {
                if (job.Trigger == JobTrigger.Time)
                    StartTimer(job);
            }

            _initialized = true;
        }

        /// <summary>Runs shutdown jobs. Call when the script shuts down.</summary>
        public static void Shutdown() => Fire(JobTrigger.Shutdown);

        private static void Load()
        {
            Jobs.Clear();

            if (!Game.LocalStorage.TryGetItemIntArr(TRIGGERS_KEY, out int[] triggers) || triggers == null)
                return;

            Game.LocalStorage.TryGetItemStringArr(TRIGGER_ARGS_KEY, out string[] triggerArgs);
            Game.LocalStorage.TryGetItemStringArr(COMMANDS_KEY, out string[] commands);
            Game.LocalStorage.TryGetItemStringArr(COMMAND_ARGS_KEY, out string[] commandArgs);

            for (int i = 0; i < triggers.Length; i++)
            {
                if (!Enum.IsDefined(typeof(JobTrigger), triggers[i])) continue;

                Jobs.Add(new Job
                {
                    Trigger = (JobTrigger)triggers[i],
                    TriggerArgs = triggerArgs != null && i < triggerArgs.Length ? triggerArgs[i] : string.Empty,
                    Command = commands != null && i < commands.Length ? commands[i] : string.Empty,
                    CommandArgs = commandArgs != null && i < commandArgs.Length ? commandArgs[i] : string.Empty
                });
            }
        }

        private static void Save()
        {
            Game.LocalStorage.SetItem(TRIGGERS_KEY, Jobs.Select(j => (int)j.Trigger).ToArray());
            Game.LocalStorage.SetItem(TRIGGER_ARGS_KEY, Jobs.Select(j => j.TriggerArgs).ToArray());
            Game.LocalStorage.SetItem(COMMANDS_KEY, Jobs.Select(j => j.Command).ToArray());
            Game.LocalStorage.SetItem(COMMAND_ARGS_KEY, Jobs.Select(j => j.CommandArgs).ToArray());
        }

        /// <summary>Adds a job, persisting it. Time jobs start firing immediately when initialized.</summary>
        public static void Add(Job job)
        {
            Jobs.Add(job);

            if (_initialized && job.Trigger == JobTrigger.Time)
                StartTimer(job);

            Save();
        }

        /// <summary>Removes the job at the given index, persisting. Returns false when out of range.</summary>
        public static bool Remove(int index)
        {
            if (index < 0 || index >= Jobs.Count)
                return false;

            Jobs[index].Timer?.Stop();

            Jobs.RemoveAt(index);
            Save();
            return true;
        }

        /// <summary>Removes all jobs, persisting.</summary>
        public static void Clear()
        {
            foreach (Job job in Jobs)
            {
                job.Timer?.Stop();
            }

            Jobs.Clear();
            Save();
        }

        /// <summary>
        /// Starts a per-job timer firing every interval in milliseconds, up
        /// to the repeat count (0 = unlimited). Timing is handled natively
        /// by the engine.
        /// </summary>
        private static void StartTimer(Job job)
        {
            string[] parts = job.TriggerArgs.Split(' ');

            if (parts.Length != 2) return;
            if (!uint.TryParse(parts[0], out uint interval) || interval < 1) return;
            if (!int.TryParse(parts[1], out int repeats) || repeats < 0) return;

            ushort count = repeats > ushort.MaxValue ? ushort.MaxValue : (ushort)repeats;

            job.Timer = Game.Events.StartUpdateCallback(_ => Run(job), interval, count);
        }

        private static void OnPlayerCreated(IPlayer[] players)
        {
            foreach (IPlayer player in players)
            {
                foreach (Job job in Jobs)
                {
                    if (job.Trigger != JobTrigger.Spawn) continue;

                    Run(job, player.Name ?? string.Empty);
                }
            }
        }

        private static void Fire(JobTrigger trigger)
        {
            foreach (Job job in Jobs)
            {
                if (job.Trigger != trigger) continue;

                Run(job);
            }
        }

        private static void Run(Job job, string playerName = null)
        {
            string commandArgs = job.CommandArgs;

            if (!string.IsNullOrEmpty(playerName))
                commandArgs = commandArgs.Replace(PlayerToken, playerName, StringComparison.Ordinal);

            string message = string.IsNullOrEmpty(commandArgs)
                ? "/" + job.Command
                : "/" + job.Command + " " + commandArgs;

            UserMessageCallbackArgs args = new(null, message);

            Command command = ModuleRegistry.Commands
                .FirstOrDefault(c => c.Name == args.Command);

            if (command == null)
            {
                Game.RunCommand(message);
                return;
            }

            command.OnCommand.Invoke(args);
        }
    }
}
