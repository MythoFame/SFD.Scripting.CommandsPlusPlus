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
                "Resets the stored win ratio statistics",
                Rsboard
            ));
        }
    }
}