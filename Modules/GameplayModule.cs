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
                "- Resets the stored win ratio statistics. Moderator-only.",
                moderatorOnly: true);
            AddCommand("wpnspawn", Wpnspawn,
                "[true|false] - Toggles or sets whether weapons spawn on the map. Moderator-only, not persisted.",
                moderatorOnly: true);
            AddCommand("refill_all", RefillAll,
                "[true|false] - Toggles or sets whether ammo is constantly refilled for all players.",
                hostOnly: true);
            AddCommand("grab", Grab,
                "[true|false] - Toggles or sets whether players are able to grab and throw each other.",
                hostOnly: true);
            AddCommand("throw", ThrowCommand,
                "[true|false] - Toggles or sets whether players can throw objects.",
                hostOnly: true);
            AddCommand("regen", Regen,
                "<hp> - Sets health regenerated per second for all players. Set to `0` or below to disable.",
                hostOnly: true);
            AddCommand("dmg_numbers", DmgNumbers,
                "[true|false] [players|objects|all] - Toggles or sets whether damage is displayed.",
                hostOnly: true);
            AddCommand("speech", Speech,
                "[true|false] - Toggles custom speech bubbles above players. Second parameter plays a sound when speech appears (default to `false`).",
                hostOnly: true);
            AddCommand("dropin", Dropin,
                "<delay> - Sets the drop-in spawn delay. Set to `0` or below to disable.",
                hostOnly: true);
            AddCommand("respawn", Respawn,
                "<delay> - Sets the custom respawn delay. Set to `0` or below to disable.",
                hostOnly: true);
            AddCommand("friendly_fire", FriendlyFire,
                "- Toggles friendly fire.",
                hostOnly: true);
            AddCommand("gravity", Gravity,
                "<constant> - Sets a constant applied to gravity. Set to `0` to disable.",
                hostOnly: true);
            AddCommand("camera", Camera,
                "{static|dynamic|individual} [zoom] - Sets camera type and optional zoom level for the current round. Not persisted.",
                hostOnly: true);
            AddCommand("gmover", Gmover,
                "{true|false|players} - Controls automatic victory detection, i.e. whether the round may end. Host-only.",
                hostOnly: true);
            AddCommand("weather", Weather,
                "<none|snow|rain> - Sets the weather. Moderator-only, not persisted.",
                moderatorOnly: true);
            AddCommand("clear", Clear,
                "<id> - Removes all objects with the given ID. Moderator-only.",
                moderatorOnly: true);
        }
    }
}
