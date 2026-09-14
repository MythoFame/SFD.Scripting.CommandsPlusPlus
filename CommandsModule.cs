using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Base class for a Commands++ module. Each module owns its
    /// <see cref="CommandHandler.Command"/> instances in a plain list and
    /// exposes them via <see cref="CommandHandler.ActiveCommands"/> only while
    /// enabled. <see cref="Autostart"/> persists the desired state in
    /// <see cref="IGame.LocalStorage"/> and defaults to enabled; the runtime
    /// <see cref="IsEnabled"/> flag guards <see cref="OnEnable"/> and
    /// <see cref="OnDisable"/> so neither ever fires redundantly.
    /// </summary>
    public abstract class CommandsModule
    {
        /// <summary>
        /// Unique module name, compared case-insensitively. Used for
        /// <c>/commands [module]</c>, <c>/toggle_module</c> and storage keys.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>Human-readable module description for <c>/modules</c>.</summary>
        public abstract string Description { get; }

        /// <summary>Storage key prefix for all module flags.</summary>
        public const string StorageKeyPrefix = "CommandsPlusPlus.Module.";

        private string StorageKey => StorageKeyPrefix + Name + ".Enabled";

        /// <summary>
        /// Persisted desired state. Reads enabled when no value is stored.
        /// </summary>
        public bool Autostart
        {
            get => Game.LocalStorage.TryGetItemBool(StorageKey, out bool result) ? result : true;
            set => Game.LocalStorage.SetItem(StorageKey, value);
        }

        private bool _isEnabled;

        /// <summary>
        /// Runtime state. Setting it fires <see cref="OnEnable"/> or
        /// <see cref="OnDisable"/> exactly once per transition — assigning the
        /// current value is a no-op.
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            private set
            {
                if (_isEnabled == value) return;

                _isEnabled = value;

                if (value)
                    OnEnable();
                else
                    OnDisable();
            }
        }

        /// <summary>
        /// Starts dormant. Activation (<see cref="Enable"/>) is driven by
        /// <see cref="ModuleRegistry.RegisterAll"/> from the persisted
        /// <see cref="Autostart"/> value, so construction never fires virtuals
        /// and never ignores stored state.
        /// </summary>
        protected CommandsModule()
        {
            _isEnabled = false;
        }

        /// <summary>
        /// Commands owned by this module. A plain list on purpose: unlike
        /// <see cref="CommandHandler.CommandCollection"/> it has no
        /// auto-subscribe side effects — exposure happens explicitly in
        /// <see cref="Register"/> and nowhere else.
        /// </summary>
        public readonly List<CommandHandler.Command> Commands = [];

        /// <summary>
        /// Called once per enable transition. Subscribe to game events here.
        /// </summary>
        public abstract void OnEnable();

        /// <summary>
        /// Called once per disable transition. Stop game events here.
        /// </summary>
        public abstract void OnDisable();

        /// <summary>
        /// Exposes owned commands to the handler. Skips commands that are
        /// already exposed, so repeated calls never create duplicates.
        /// </summary>
        public void Register()
        {
            foreach (CommandHandler.Command command in Commands)
            {
                if (!CommandHandler.ActiveCommands.Contains(command))
                    CommandHandler.ActiveCommands.Add(command);
            }
        }

        /// <summary>Withdraws owned commands from the handler.</summary>
        public void Unregister()
        {
            foreach (CommandHandler.Command command in Commands)
                CommandHandler.ActiveCommands.Remove(command);
        }

        /// <summary>
        /// Persists the enabled state, exposes commands and fires
        /// <see cref="OnEnable"/> once (guarded by <see cref="IsEnabled"/>).
        /// </summary>
        public void Enable()
        {
            Autostart = true;
            Register();
            IsEnabled = true;
        }

        /// <summary>
        /// Persists the disabled state, fires <see cref="OnDisable"/> once
        /// (guarded by <see cref="IsEnabled"/>) and withdraws commands.
        /// </summary>
        public void Disable()
        {
            Autostart = false;
            IsEnabled = false;
            Unregister();
        }

        /// <summary>Flips the state via <see cref="Enable"/>/<see cref="Disable"/>. Returns the new state.</summary>
        public bool Toggle()
        {
            if (IsEnabled)
                Disable();
            else
                Enable();

            return IsEnabled;
        }

        /// <summary>
        /// Helper to create, configure and track a command owned by this module.
        /// </summary>
        protected CommandHandler.Command AddCommand(string name, Action<UserMessageCallbackArgs> onCommand,
            string description = null, bool hostOnly = false, bool moderatorOnly = false)
        {
            CommandHandler.Command command = new(name, onCommand)
            {
                Description = description,
                HostOnly = hostOnly,
                ModeratorOnly = moderatorOnly
            };

            Commands.Add(command);
            return command;
        }
    }
}
