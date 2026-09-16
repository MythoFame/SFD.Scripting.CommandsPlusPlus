using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus.Commands;

public partial class GameScript : GameScriptInterfaceExtended
{
    public enum CommandPermision : byte
    {
        Host,
        Moderator, // includes moderator
        Everyone, // includes host and moderator
    }

    /// <summary>
    /// Represents a single chat command that the <see cref="CommandHandler"/> can dispatch.
    /// </summary>
    public sealed class Command
    {
        private string _name = string.Empty;

        public string Name
        {
            get => _name;
            set => _name = value.ToUpper();
        }

        /// <summary>
        /// Permissions for this command.
        /// </summary>
        public CommandPermision Permissions;

        /// <summary>
        /// The human-readable description to show when the user requests command help.
        /// </summary>
        public string Description = null;

        /// <summary>
        /// The action executed when a user issues this command in chat. Receives the
        /// original <see cref="UserMessageCallbackArgs"/> containing the sender and arguments.
        /// </summary>
        public Action<UserMessageCallbackArgs> OnCommand = null;

        public Command(string name, string description, Action<UserMessageCallbackArgs> onCommand, CommandPermision permissions = CommandPermision.Moderator)
        {
            Name = name;
            Description = description;
            Permissions = permissions;
            OnCommand = onCommand;
        }
    }
}