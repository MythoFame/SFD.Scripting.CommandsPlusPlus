namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Player interaction commands (kill, revive, teleport, ...).
    /// Command implementations live in <c>Commands/PlayerModule/</c> as
    /// partial declarations of this class.
    /// </summary>
    public sealed partial class PlayerModule : CommandsModule
    {
        public override string Name => "Player";

        public override string Description => "Player module";

        public PlayerModule()
        {
            AddCommand("kill", Kill,
                "<player> - Kills a player",
                moderatorOnly: true);
            AddCommand("gib", Gib,
                "<player> - Gibs a player",
                moderatorOnly: true);
            AddCommand("remove", Remove,
                "<player> - Removes a player",
                moderatorOnly: true);
            AddCommand("dmg", Dmg,
                "<player> <amount> - Deals damage to a player",
                moderatorOnly: true);
            AddCommand("notarget", NoTarget,
                "<player> - Toggles whether bots target a player",
                moderatorOnly: true);
            AddCommand("tp", Tp,
                "<to> - Teleports you to a player",
                moderatorOnly: true);
            AddCommand("tphere", Tphere,
                "<player> - Teleports a player to you",
                moderatorOnly: true);
            AddCommand("tppos", Tppos,
                "<player> <x> <y> - Teleports a player to a world position",
                moderatorOnly: true);
            AddCommand("trip", Trip,
                "<player> - Trips a player, knocking them down",
                moderatorOnly: true);
            AddCommand("pos", Pos,
                "<player> - Displays the world position of a player",
                moderatorOnly: true);
            AddCommand("input", Input,
                "<player> - Toggles whether a player can provide input",
                moderatorOnly: true);
            AddCommand("team", Team,
                "<player> <team> - Sets the team of a player",
                moderatorOnly: true);
            AddCommand("burn", Burn,
                "<player> - Toggles whether a player is burning",
                moderatorOnly: true);
            AddCommand("spawn", Spawn,
                "<id> - Spawns an object with the given ID at your position",
                moderatorOnly: true);
            AddCommand("revive", Revive,
                "<player> - Revives a dead player",
                moderatorOnly: true);
            AddCommand("refill", Refill,
                "<player> - Refills a player's ammo as if they used an ammo stash",
                moderatorOnly: true);
            AddCommand("fly", FlyCommand,
                "<player> - Toggles flying for a player",
                moderatorOnly: true);
            AddCommand("noclip", Noclip,
                "<player> - Toggles noclip for a player, allowing them to pass through walls",
                moderatorOnly: true);
            AddCommand("copy", Copy,
                "<from> <to> - Copies one player's profile onto another player",
                moderatorOnly: true);
            AddCommand("swap", Swap,
                "<from> <to> - Swaps the profiles of two players",
                moderatorOnly: true);
            AddCommand("user", User,
                "<from> <to> - Swaps the users of two players",
                moderatorOnly: true);
            AddCommand("action", ActionCommand,
                "<player> <action> - Queues an action for a player whose input is disabled",
                moderatorOnly: true);
        }
    }
}
