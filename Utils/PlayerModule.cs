
using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Tracks registered chat commands and dispatches incoming user messages to
    /// their associated callbacks. The handler auto-subscribes to user message
    /// events the moment a command is added to <see cref="ActiveCommands"/>, and
    /// auto-unsubscribes once the list is emptied — no manual Initialize/Destroy
    /// calls required.
    /// </summary>
    public sealed class PlayerModule : CommandsModule
    {
        public override string Name => "Player";

        public override string Description => "Player module";

        public PlayerModule()
        {
            Commands.Add(new CommandHandler.Command("kill", Kill));
        }

        public override void OnEnable()
        {
        }

        public override void OnDisable()
        {
        }

        private static void Kill(UserMessageCallbackArgs args)
        {
            IPlayer[] players = ParseHelper.ParsePlayers(args.CommandArguments).ToArray();

            foreach (IPlayer player in players)
                player.Kill();
        }
    }
}