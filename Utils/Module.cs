using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Base class for a Commands++ module. Each module owns its
    /// <see cref="CommandHandler.Command"/> instances and registers them with
    /// <see cref="CommandHandler.ActiveCommands"/> only while enabled.
    /// Enabled state persists in <see cref="IGame.LocalStorage"/> and defaults
    /// to enabled for all modules.
    /// </summary>
    public abstract class Module
    {
        /// <summary>
        /// Unique module name, compared case-insensitively. Used for
        /// <c>/commands [module]</c>, <c>/toggle_module</c> and storage keys.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>Human-readable module description for <c>/modules</c>.</summary>
        public abstract string Description { get; }

        /// <summary>
        /// Default enabled state when nothing is persisted. Always true.
        /// </summary>
        public virtual bool EnabledByDefault => true;

        /// <summary>
        /// Whether this module may be disabled. Management overrides this to false.
        /// </summary>
        public virtual bool CanBeDisabled => true;

        /// <summary>Storage key prefix for all module flags.</summary>
        public const string StorageKeyPrefix = "CommandsPlusPlus.Module.";

        private string StorageKey => StorageKeyPrefix + Name + ".Enabled";

        /// <summary>
        /// Persisted enabled flag. Reads default when no value is stored.
        /// </summary>
        public bool IsEnabled
        {
            get => Game.LocalStorage.GetItem(StorageKey) as bool? ?? EnabledByDefault;
            set => Game.LocalStorage.SetItem(StorageKey, value);
        }

        /// <summary>Whether commands are currently registered with the handler.</summary>
        public bool IsRegistered { get; private set; }

        /// <summary>
        /// Commands owned by this module. Added to
        /// <see cref="CommandHandler.ActiveCommands"/> on <see cref="Register"/>
        /// and removed on <see cref="Unregister"/>.
        /// </summary>
        public readonly List<CommandHandler.Command> Commands = [];

        /// <summary>
        /// Called after commands are registered. Subscribe to game events here.
        /// </summary>
        public virtual void OnEnable() { }

        /// <summary>
        /// Called before commands are unregistered. Stop game events here.
        /// </summary>
        public virtual void OnDisable() { }

        /// <summary>Adds owned commands to the handler. No-op if already registered.</summary>
        public void Register()
        {
            if (IsRegistered) return;

            foreach (CommandHandler.Command command in Commands)
                CommandHandler.ActiveCommands.Add(command);

            IsRegistered = true;
        }

        /// <summary>Removes owned commands from the handler. No-op if not registered.</summary>
        public void Unregister()
        {
            if (!IsRegistered) return;

            foreach (CommandHandler.Command command in Commands)
                CommandHandler.ActiveCommands.Remove(command);

            IsRegistered = false;
        }

        /// <summary>Persists enabled state and registers commands plus events.</summary>
        public void Enable()
        {
            IsEnabled = true;

            if (!IsRegistered)
            {
                Register();
                OnEnable();
            }
        }

        /// <summary>Persists disabled state and stops events plus unregisters commands.</summary>
        public void Disable()
        {
            IsEnabled = false;

            if (IsRegistered)
            {
                OnDisable();
                Unregister();
            }
        }

        /// <summary>Flips the persisted state. Returns the new state.</summary>
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
