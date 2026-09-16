using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Base class for a Commands++ module. Each module owns its
    /// <see cref="CommandHandler.Command"/> instances in a plain list and
    /// exposes them via <see cref="CommandHandler.ActiveCommands"/>.
    /// <see cref="Restricted"/> persists the restricted state in
    /// <see cref="IGame.LocalStorage"/> and defaults to allowed; the runtime
    /// <see cref="IsRestricted"/> flag guards <see cref="OnRestricted"/> so it
    /// never fires redundantly. Restricting a module limits all of its commands
    /// to the host instead of withdrawing them.
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

        private string StorageKey => StorageKeyPrefix + Name + ".Restricted";

        /// <summary>
        /// Persisted restricted state. Reads allowed when no value is stored.
        /// </summary>
        public bool Restricted
        {
            get => Game.LocalStorage.TryGetItemBool(StorageKey, out bool result) && result;
            set => Game.LocalStorage.SetItem(StorageKey, value);
        }

        private bool _isRestricted;

        /// <summary>
        /// Runtime state. Setting it fires <see cref="OnRestricted"/> exactly
        /// once per transition — assigning the current value is a no-op.
        /// </summary>
        public bool IsRestricted
        {
            get => _isRestricted;
            private set
            {
                if (_isRestricted == value) return;

                _isRestricted = value;
                OnRestricted(value);
            }
        }

        /// <summary>
        /// Starts allowed. Restriction (<see cref="Restrict"/>) is driven by
        /// <see cref="ModuleRegistry.RegisterAll"/> from the persisted
        /// <see cref="Restricted"/> value, so construction never fires virtuals
        /// and never ignores stored state.
        /// </summary>
        protected CommandsModule() => _isRestricted = false;

        /// <summary>
        /// Commands owned by this module. A plain list on purpose: unlike
        /// <see cref="CommandHandler.CommandCollection"/> it has no
        /// auto-subscribe side effects — exposure happens explicitly in
        /// <see cref="Register"/> and nowhere else.
        /// </summary>
        public readonly List<CommandHandler.Command> Commands = [];

        /// <summary>
        /// Each owned command's permissions before <see cref="Restrict"/>
        /// overrode them. Captured on first restriction, restored on
        /// <see cref="Allow"/>.
        /// </summary>
        private readonly Dictionary<CommandHandler.Command, (bool hostOnly, bool moderatorOnly)> _originalPermissions = [];

        /// <summary>
        /// Called once per allow/restrict transition. React to the restriction
        /// change here.
        /// </summary>
        public virtual void OnRestricted(bool restricted) { }

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

        /// <summary>
        /// Persists the restricted state and limits every owned command to the
        /// host, remembering original permissions for <see cref="Allow"/>.
        /// Fires <see cref="OnRestricted"/> once (guarded by <see cref="IsRestricted"/>).
        /// </summary>
        public void Restrict()
        {
            Restricted = true;

            foreach (CommandHandler.Command command in Commands)
            {
                if (!_originalPermissions.ContainsKey(command))
                    _originalPermissions[command] = (command.HostOnly, command.ModeratorOnly);

                command.HostOnly = true;
            }

            IsRestricted = true;
        }

        /// <summary>
        /// Persists the allowed state and restores every owned command's
        /// original permissions. Fires <see cref="OnRestricted"/> once (guarded
        /// by <see cref="IsRestricted"/>).
        /// </summary>
        public void Allow()
        {
            Restricted = false;

            foreach (var entry in _originalPermissions)
            {
                entry.Key.HostOnly = entry.Value.hostOnly;
                entry.Key.ModeratorOnly = entry.Value.moderatorOnly;
            }

            _originalPermissions.Clear();
            IsRestricted = false;
        }

        /// <summary>Flips the state via <see cref="Restrict"/>/<see cref="Allow"/>. Returns the new restricted state.</summary>
        public bool Toggle()
        {
            if (IsRestricted)
                Allow();
            else
                Restrict();

            return IsRestricted;
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