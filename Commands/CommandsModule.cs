namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Base class for a Commands++ module.
    /// Each module contains a <see cref="CommandHandler.CommandCollection"/> list.
    /// <see cref="IsRestricted"/> restricts all of its commands to the host only.
    /// </summary>
    public abstract class CommandsModule
    {
        /// <summary>Unique module name, used for help and storage functions.</summary>
        public abstract string Name { get; }

        /// <summary>Module description, used for help.</summary>
        public abstract string Description { get; }

        /// <summary>Storage prefix .</summary>
        public const string StorageKeyPrefix = "CommandsPlusPlus.Module.";

        private string StorageKey => StorageKeyPrefix + Name + ".Blocked";

        private bool _isRestricted;

        public bool IsRestricted
        {
            get => _isRestricted;
            private set
            {
                if (_isRestricted == value) return;

                _isRestricted = value;
                Game.LocalStorage.SetItem(StorageKey, value);

                if (value)
                    OnEnabled();
                else
                    OnDisabled();
            }
        }

        protected CommandsModule() => IsRestricted = Game.LocalStorage.TryGetItemBool(StorageKey, out bool result) && result;

        public void Reset() => Game.LocalStorage.RemoveItem(StorageKey);

        /// <summary>
        /// Commands owned by this module. 
        /// </summary>
        public readonly List<Command> Commands = [];

        public virtual void OnDisabled() { }

        public virtual void OnEnabled() { }

        public bool Toggle() => IsRestricted = !_isRestricted;

        /// <summary>
        /// Helper to create and track a command owned by this module.
        /// </summary>
        protected void AddCommand(Command command)
        {
            Commands.Add(command);
        }
    }
}
