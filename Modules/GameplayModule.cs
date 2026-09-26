namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Custom gameplay rules.
    /// </summary>
    public sealed partial class GameplayModule : CommandsModule
    {
        public override string Name => "Gameplay";

        public override string Description => "Gameplay module";

        public GameplayModule()
        {
            AddCommand(new Command(
                "rsboard",
                "Resets the stored win ratio statistics. Moderator-only.",
                Rsboard
            ));

            AddCommand(new Command(
                "wpnspawn",
                "[true|false] - Toggles or sets whether weapons spawn on the map. Moderator-only, not persisted.",
                Wpnspawn
            ));

            AddCommand(new Command(
                "refill_all",
                "[true|false] - Toggles or sets whether ammo is constantly refilled for all players.",
                RefillAll, CommandPermision.Host
            ));

            AddCommand(new Command(
                "grab",
                "[true|false] - Toggles or sets whether players are able to grab and throw each other.",
                Grab, CommandPermision.Host
            ));

            AddCommand(new Command(
                "throw",
                "[true|false] - Toggles or sets whether players can throw objects.",
                ThrowCommand, CommandPermision.Host
            ));

            AddCommand(new Command(
                "regen",
                "<hp> - Sets health regenerated per second for all players. Set to `0` or below to disable.",
                Regen, CommandPermision.Host
            ));

            AddCommand(new Command(
                "dmg_numbers",
                "[true|false] [players|objects|all] - Toggles or sets whether damage is displayed.",
                DmgNumbers, CommandPermision.Host
            ));

            AddCommand(new Command(
                "speech",
                "[true|false] - Toggles custom speech bubbles above players. Second parameter plays a sound when speech appears (default to `false`).",
                Speech, CommandPermision.Host
            ));

            AddCommand(new Command(
                "dropin",
                "<delay> - Sets the drop-in spawn delay. Set to `0` or below to disable.",
                Dropin, CommandPermision.Host
            ));

            AddCommand(new Command(
                "respawn",
                "<delay> - Sets the custom respawn delay. Set to `0` or below to disable.",
                Respawn, CommandPermision.Host
            ));

            AddCommand(new Command(
                "friendly_fire",
                "[true|false] - Toggles friendly fire.",
                FriendlyFire, CommandPermision.Host
            ));

            AddCommand(new Command(
                "gravity",
                "<constant> - Sets a constant applied to gravity. Set to `0` to disable.",
                Gravity, CommandPermision.Host
            ));

            AddCommand(new Command(
                "camera",
                "{static|dynamic|individual} [zoom] - Sets camera type and optional zoom level for the current round. Not persisted.",
                Camera, CommandPermision.Host
            ));

            AddCommand(new Command(
                "gmover",
                "{true|false|players} - Controls automatic victory detection, i.e. whether the round may end. Host-only.",
                Gmover, CommandPermision.Host
            ));

            AddCommand(new Command(
                "weather",
                "<none|snow|rain> - Sets the weather. Moderator-only, not persisted.",
                Weather
            ));

            AddCommand(new Command(
                "clear_obj",
                "<id> - Removes all objects with the given ID. Moderator-only.",
                ClearObj
            ));
        }
    }
}