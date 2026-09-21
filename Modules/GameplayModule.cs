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
        }
    }
}
