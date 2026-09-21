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
            AddCommand("spectate", Spectate,
                "[user] - Toggles your own spectation for the next round");
            AddCommand("spectate_whitelist", SpectateWhitelist,
                "- Toggles whether only whitelisted players are allowed to play",
                moderatorOnly: true);
            AddCommand("spectate_add_whitelist", SpectateAddWhitelist,
                "<user> - Adds a user to the spectate whitelist",
                moderatorOnly: true);
            AddCommand("spectate_rm_whitelist", SpectateRmWhitelist,
                "<account> - Removes an account from the spectate whitelist, * clears all",
                moderatorOnly: true);
        }
    }
}
