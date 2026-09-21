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
            AddCommand("spec", Spectate,
                "[user] - Toggles spectation for next round");
            AddCommand("spec_wl", SpectateWhitelist,
                "- Toggles whitelist-only mode",
                moderatorOnly: true);
            AddCommand("spec_add", SpectateAddWhitelist,
                "<user> - Adds user to whitelist",
                moderatorOnly: true);
            AddCommand("spec_rm", SpectateRmWhitelist,
                "<account> - Removes account from whitelist, * clears all",
                moderatorOnly: true);
        }
    }
}
