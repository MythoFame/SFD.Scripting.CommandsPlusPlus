using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Base class for a Commands++ module. Each module owns its
    /// <see cref="CommandHandler.Command"/> instances in a plain list and
    /// exposes them via <see cref="CommandHandler.ActiveCommands"/>.
    /// <see cref="Blocked"/> persists the blocked state in
    /// <see cref="IGame.LocalStorage"/> and defaults to unblocked; the runtime
    /// <see cref="IsBlocked"/> flag guards <see cref="OnBlocked"/> so it never
    /// fires redundantly. Blocking a module restricts all of its commands to
    /// the host instead of withdrawing them.
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

        private string StorageKey => StorageKeyPrefix + Name + ".Blocked";

        /// <summary>
        /// Persisted blocked state. Reads unblocked when no value is stored.
        /// </summary>
        public bool Blocked
        {
            get => Game.LocalStorage.TryGetItemBool(StorageKey, out bool result) && result;
            set => Game.LocalStorage.SetItem(StorageKey, value);
        }

        private bool _isBlocked;

        /// <summary>
        /// Runtime state. Setting it fires <see cref="OnBlocked"/> exactly once
        /// per transition — assigning the current value is a no-op.
        /// </summary>
        public bool IsBlocked
        {
            get => _isBlocked;
            private set
            {
                if (_isBlocked == value) return;

                _isBlocked = value;
                OnBlocked(value);
            }
        }

        /// <summary>
        /// Starts unblocked. Activation (<see cref="Block"/>) is driven by
        /// <see cref="ModuleRegistry.RegisterAll"/> from the persisted
        /// <see cref="Blocked"/> value, so construction never fires virtuals
        /// and never ignores stored state.
        /// </summary>
        protected CommandsModule() => _isBlocked = false;

        /// <summary>
        /// Commands owned by this module. A plain list on purpose: unlike
        /// <see cref="CommandHandler.CommandCollection"/> it has no
        /// auto-subscribe side effects — exposure happens explicitly in
        /// <see cref="Register"/> and nowhere else.
        /// </summary>
        public readonly List<CommandHandler.Command> Commands = [];

        /// <summary>
        /// Each owned command's permissions before <see cref="Block"/> overrode
        /// them. Captured on first block, restored on <see cref="Unblock"/>.
        /// </summary>
        private readonly Dictionary<CommandHandler.Command, (bool hostOnly, bool moderatorOnly)> _originalPermissions = [];

        /// <summary>
        /// Called once per block/unblock transition. React to the restriction
        /// change here.
        /// </summary>
        public virtual void OnBlocked(bool blocked) { }

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
        /// Persists the blocked state and restricts every owned command to the
        /// host, remembering original permissions for <see cref="Unblock"/>.
        /// Fires <see cref="OnBlocked"/> once (guarded by <see cref="IsBlocked"/>).
        /// </summary>
        public void Block()
        {
            Blocked = true;

            foreach (CommandHandler.Command command in Commands)
            {
                if (!_originalPermissions.ContainsKey(command))
                    _originalPermissions[command] = (command.HostOnly, command.ModeratorOnly);

                command.HostOnly = true;
            }

            IsBlocked = true;
        }

        /// <summary>
        /// Persists the unblocked state and restores every owned command's
        /// original permissions. Fires <see cref="OnBlocked"/> once (guarded by
        /// <see cref="IsBlocked"/>).
        /// </summary>
        public void Unblock()
        {
            Blocked = false;

            foreach (var entry in _originalPermissions)
            {
                entry.Key.HostOnly = entry.Value.hostOnly;
                entry.Key.ModeratorOnly = entry.Value.moderatorOnly;
            }

            _originalPermissions.Clear();
            IsBlocked = false;
        }

        /// <summary>Flips the state via <see cref="Block"/>/<see cref="Unblock"/>. Returns the new blocked state.</summary>
        public bool Toggle()
        {
            if (IsBlocked)
                Unblock();
            else
                Block();

            return IsBlocked;
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
