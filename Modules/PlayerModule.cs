using SFDGameScriptInterface;

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
                "<player> [gib|remove] - Kills a player, optionally gibbing or removing them",
                moderatorOnly: true);
            AddCommand("tp", Tp,
                "<from> [to] - Teleports a player to another player, or to you if no target is given",
                moderatorOnly: true);
            AddCommand("trip", Trip,
                "<player> - Trips a player, knocking them down",
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
            AddCommand("skin", Skin,
                "<from> <to> - Copies one player's profile onto another player",
                moderatorOnly: true);
        }

        public override void OnEnable()
        {
        }

        public override void OnDisable()
        {
        }
    }
}
