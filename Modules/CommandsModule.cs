using static SFD.Scripting.CommandsPlusPlus.Commands.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Base class for a Commands++ module.
    /// Each module contains a <see cref="CommandHandler.CommandCollection"/> list.
    /// <see cref="IsEnabled"/> restricts all of its commands to the host only.
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

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            private set
            {
                if (_isEnabled == value) return;

                _isEnabled = value;
                Game.LocalStorage.SetItem(StorageKey, value);

                if (value)
                    OnEnabled();
                else
                    OnDisabled();
            }
        }

        protected CommandsModule() => IsEnabled = Game.LocalStorage.TryGetItemBool(StorageKey, out bool result) && result;

        public void Reset() => Game.LocalStorage.RemoveItem(StorageKey);

        /// <summary>
        /// Commands owned by this module. 
        /// </summary>
        public readonly List<Command> Commands = [];

        public virtual void OnDisabled() { }

        public virtual void OnEnabled() { }

        public bool Toggle() => IsEnabled = !_isEnabled;

        /// <summary>
        /// Helper to create and track a command owned by this module.
        /// </summary>
        protected void AddCommand(Command command)
        {
            Commands.Add(command);
        }
    }
}
