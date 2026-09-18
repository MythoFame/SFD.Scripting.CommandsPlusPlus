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
                "[bool] - Toggles or explicitly sets whether weapons spawn on the map",
                moderatorOnly: true);
        }
    }
}
