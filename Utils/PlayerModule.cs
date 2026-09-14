
using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Player interaction commands (kill, revive, teleport, ...).
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
            IPlayer[] players = [.. ParseHelper.ParsePlayers(args.CommandArguments)];

            foreach (IPlayer player in players)
                player.Kill();
        }
    }
}
