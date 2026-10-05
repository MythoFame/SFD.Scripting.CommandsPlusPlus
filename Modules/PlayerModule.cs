namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Moderator tools to directly control players, their gear and movement.
    /// Command implementations live in <c>Commands/PlayerModule/</c> as
    /// partial declarations of this class.
    /// </summary>
    public sealed partial class PlayerModule : CommandsModule
    {
        public override string Name => "Player";

        public override string Description => "Moderator tools to directly control players.";

        public PlayerModule()
        {
            AddCommand("kill", Kill,
                "<player> - Kills a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("gib", Gib,
                "<player> - Gibs a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("remove", Remove,
                "<player> - Removes a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("dmg", Dmg,
                "<player> <amount> - Deals damage to a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("notarget", NoTarget,
                "<player> - Toggles whether bots target a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("tp", Tp,
                "<to> - Teleports you to a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("tphere", Tphere,
                "<player> - Teleports a player to you.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("tppos", Tppos,
                "<player> <x> <y> - Teleports a player to a world position.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("trip", Trip,
                "<player> - Trips a player, knocking them down.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("pos", Pos,
                "<player> - Displays the world position of a player. Moderator-only.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("modifier", Modifier,
                "<player> <modifier> <value> - Sets a player modifier to the given value.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("tag", Tag,
                "<player> [name|status] - Toggles nametag and status bar visibility for a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("input", Input,
                "<player> - Toggles whether a player can provide input, effectively freezing or unfreezing them.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("team", Team,
                "<player> <team> - Sets the team of a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("burn", Burn,
                "<player> - Toggles whether a player is burning.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("spawn", Spawn,
                "<id> - Spawns an object with the given ID at your position.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("revive", Revive,
                "<player> - Revives a dead player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("refill", Refill,
                "<player> - Refills a player's ammo as if they used an ammo stash.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("fly", FlyCommand,
                "<player> - Toggles flying for a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("noclip", Noclip,
                "<player> - Toggles noclip for a player, allowing them to pass through walls.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("copy", Copy,
                "<from> <to> - Copies one player's profile onto another player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("swap", Swap,
                "<from> <to> - Swaps the profiles of two players.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("user", User,
                "<from> <to> - Swaps the users of two players.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("wear", Wear,
                "<player> <slot> <name> [color1] [color2] - Gives a player a cosmetic item in the given slot with the given colors.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("action", ActionCommand,
                "<player> <action> - Queues an action for a player whose input is disabled.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("magnet", MagnetCommand,
                "<player> [area_size] [players|objects|all] - Toggles attraction of nearby players and/or objects toward a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("repulse", RepulseCommand,
                "<player> [area_size] [players|objects|all] - Toggles repulsion of nearby players and/or objects away from a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("boost", Boost,
                "<player> <left|down|up|right> [speed] - Boosts a player in a direction.",
                permission: CommandHandler.Permission.ModeratorOnly);
        }
    }
}
