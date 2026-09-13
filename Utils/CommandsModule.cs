using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Base class for a Commands++ module. Each module owns its
    /// <see cref="CommandHandler.Command"/> instances and registers them with
    /// <see cref="CommandHandler.GlobalCommands"/> only while enabled.
    /// Enabled state persists in <see cref="IGame.LocalStorage"/> and defaults
    /// to enabled for all modules.
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
        /// Persisted enabled flag. Reads default when no value is stored.
        /// </summary>
        public bool Autostart
        {
            get => Game.LocalStorage.TryGetItemBool(StorageKey, out bool result) || result;
            set => Game.LocalStorage.SetItem(StorageKey, value);
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value)
                    OnEnable();
                else
                    OnDisable();

                _isEnabled = value;
            }
        }

        public CommandsModule()
        {
            IsEnabled = true; //hardcoded for debugging
        }

        /// <summary>
        /// Commands owned by this module. Added to
        /// <see cref="CommandHandler.GlobalCommands"/> on <see cref="Register"/>
        /// and removed on <see cref="Unregister"/>.
        /// </summary>
        public readonly CommandHandler.CommandCollection Commands = [];

        /// <summary>
        /// Called after commands are registered. Subscribe to game events here.
        /// </summary>
        public abstract void OnEnable();

        /// <summary>
        /// Called before commands are unregistered. Stop game events here.
        /// </summary>
        public abstract void OnDisable();

        /// <summary>Flips the persisted state. Returns the new state.</summary>
        public bool Toggle() => IsEnabled = !IsEnabled;

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
