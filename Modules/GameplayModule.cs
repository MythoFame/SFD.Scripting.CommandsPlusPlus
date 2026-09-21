namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Custom gameplay rules.
    /// Command implementations live in <c>Commands/GameplayModule/</c> as
    /// partial declarations of this class.
    /// </summary>
    public sealed partial class GameplayModule : CommandsModule
    {
        public override string Name => "Gameplay";

        public override string Description => "Gameplay module";

        public GameplayModule()
        {
            AddCommand("rsboard", Rsboard,
                "- Resets the stored win ratio statistics",
                moderatorOnly: true);
            AddCommand("wpnspawn", Wpnspawn,
                "[bool] - Toggles or sets whether weapons spawn on the map",
                moderatorOnly: true);
            AddCommand("refill_all", RefillAll,
                "[bool] - Toggles or sets whether ammo is constantly refilled for all players",
                hostOnly: true);
            AddCommand("grab", Grab,
                "[bool] - Toggles or sets whether players are able to grab and throw each other",
                hostOnly: true);
            AddCommand("throw", ThrowCommand,
                "[bool] - Toggles or sets whether players can throw objects",
                hostOnly: true);
            AddCommand("regen", Regen,
                "<hp> - Sets health regenerated per second for all players",
                hostOnly: true);
            AddCommand("dmg_numbers", DmgNumbers,
                "[bool] [players|objects|all] - Toggles or sets whether damage is displayed",
                hostOnly: true);
            AddCommand("speech", Speech,
                "[bool] - Toggles custom speech bubbles above players",
                hostOnly: true);
            AddCommand("dropin", Dropin,
                "<delay> - Sets the drop-in spawn delay, 0 or below disables it",
                hostOnly: true);
            AddCommand("respawn", Respawn,
                "<delay> - Sets the custom respawn delay, 0 or below disables it",
                hostOnly: true);
            AddCommand("friendly_fire", FriendlyFire,
                "- Toggles friendly fire",
                hostOnly: true);
            AddCommand("gravity", Gravity,
                "<constant> - Sets a constant applied to gravity",
                hostOnly: true);
            AddCommand("camera", Camera,
                "{reset|static|dynamic|individual} [zoom] - Sets camera type and optional zoom level for the current round",
                hostOnly: true);
            AddCommand("gmover", Gmover,
                "{true|false|players} - Controls automatic victory detection",
                hostOnly: true);
        }
    }
}
