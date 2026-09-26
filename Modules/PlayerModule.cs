using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Player interaction commands (kill, revive, teleport, etc).
    /// </summary>
    public sealed partial class PlayerModule : CommandsModule
    {
        public override string Name => "Player";

        public override string Description => "Player module";

        public PlayerModule()
        {
            AddCommand(new Command(
                "kill",
                "<player> - Kills a player",
                Kill
            ));

            AddCommand(new Command(
                "gib",
                "<player> - Gibs a player",
                Gib
            ));

            AddCommand(new Command(
                "remove",
                "<player> - Removes a player",
                Remove
            ));

            AddCommand(new Command(
                "dmg",
                "<player> <amount> - Deals damage to a player",
                Dmg
            ));

            AddCommand(new Command(
                "notarget",
                "<player> - Toggles whether bots target a player",
                NoTarget
            ));

            AddCommand(new Command(
                "tp",
                "<from> <to> - Teleports a player to another player, or to you if no target is given",
                Tp
            ));

            AddCommand(new Command(
                "trip",
                "<player> - Trips a player, knocking them down",
                Trip
            ));

            AddCommand(new Command(
                "input",
                "<player> - Toggles whether a player can provide input",
                Input
            ));

            AddCommand(new Command(
                "team",
                "<player> <team> - Sets the team of a player",
                Team
            ));

            AddCommand(new Command(
                "burn",
                "<player> - Toggles whether a player is burning",
                Burn
            ));

            AddCommand(new Command(
                "spawn",
                "<id> - Spawns an object with the given ID at your position",
                Spawn
            ));

            AddCommand(new Command(
                "revive",
                "<player> - Revives a dead player",
                Revive
            ));

            AddCommand(new Command(
                "refill",
                "<player> - Refills a player's ammo as if they used an ammo stash",
                Refill
            ));

            AddCommand(new Command(
                "fly",
                "<player> - Toggles flying for a player",
                FlyCommand
            ));

            AddCommand(new Command(
                "noclip",
                "<player> - Toggles noclip for a player, allowing them to pass through walls",
                Noclip
            ));

            AddCommand(new Command(
                "copy",
                "<from> <to> - Copies one player's profile onto another player",
                Copy
            ));

            AddCommand(new Command(
                "swap",
                "<from> <to> - Swaps the profiles of two players",
                Swap
            ));

            AddCommand(new Command(
                "user",
                "<from> <to> - Swaps the users of two players",
                User
            ));

            AddCommand(new Command(
                "action",
                "<player> <action> - Queues an action for a player whose input is disabled",
                ActionCommand
            ));
        }
    }
}
