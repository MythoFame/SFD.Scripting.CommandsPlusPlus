namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Change how the match itself behaves. Persistent rules like respawns
    /// and physics that stay on until you turn them off. Host-only, persisted.
    /// Command implementations live in <c>Commands/GameplayModule/</c> as
    /// partial declarations of this class.
    /// </summary>
    public sealed partial class GameplayModule : CommandsModule
    {
        public override string Name => "Gameplay";

        public override string Description => "Change how the match itself behaves.";

        public GameplayModule()
        {
            AddCommand("rsboard", Rsboard,
                "- Resets the stored win ratio statistics.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("wpnspawn", Wpnspawn,
                "[true|false] - Toggles or sets whether weapons spawn on the map, not persisted.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("refill_all", RefillAll,
                "[true|false] - Toggles or sets whether ammo is constantly refilled for all players.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("grab", Grab,
                "[true|false] - Toggles or sets whether players are able to grab and throw each other.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("throw", ThrowCommand,
                "[true|false] - Toggles or sets whether players can throw objects.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("regen", Regen,
                "<hp> - Sets health regenerated per second for all players. Set to `0` or below to disable.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("dmg_numbers", DmgNumbers,
                "[true|false] [players|objects|all] - Toggles or sets whether damage is displayed.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("speech", Speech,
                "[true|false] - Toggles custom speech bubbles above players. Second parameter plays a sound when speech appears (default to `false`).",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("dropin", Dropin,
                "<delay> - Sets the drop-in spawn delay. Set to `0` or below to disable.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("respawn", Respawn,
                "<delay> - Sets the custom respawn delay. Set to `0` or below to disable.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("friendly_fire", FriendlyFire,
                "[true|false] - Toggles friendly fire.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("gravity", Gravity,
                "<constant> - Sets a constant applied to gravity. Set to `0` to disable.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("camera", Camera,
                "{static|dynamic|individual} [zoom] - Sets camera type and optional zoom level for the current round. Not persisted.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("gmover", Gmover,
                "{true|false|players} - Controls automatic victory detection, i.e. whether the round may end.",
                permission: CommandHandler.Permission.HostOnly);
            AddCommand("weather", Weather,
                "<none|snow|rain> - Sets the weather, not persisted.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("clear_obj", ClearObj,
                "<id> - Removes all objects with the given ID.",
                permission: CommandHandler.Permission.ModeratorOnly);
        }
    }
}
