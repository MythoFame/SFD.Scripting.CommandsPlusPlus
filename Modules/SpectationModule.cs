namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Spectation control commands (spectate, whitelist, ...). Restricted by
    /// default. Command implementations live in <c>Commands/SpectationModule/</c>
    /// as partial declarations of this class.
    /// </summary>
    public sealed partial class SpectationModule : CommandsModule
    {
        public override string Name => "Spectation";

        public override string Description => "Spectation module";

        public override bool DefaultRestricted => true;

        public SpectationModule()
        {
        }
    }
}
