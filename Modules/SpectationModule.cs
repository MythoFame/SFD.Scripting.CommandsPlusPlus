namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Choose who plays and who watches. Ideal for lobbies and tournaments.
    /// Restricted by default. Command implementations live in
    /// <c>Commands/SpectationModule/</c> as partial declarations of this class.
    /// </summary>
    public sealed partial class SpectationModule : CommandsModule
    {
        public override string Name => "Spectation";

        public override string Description => "Choose who plays and who watches.";

        public override bool DefaultRestricted => true;

        public SpectationModule()
        {
            AddCommand(new Command(
                "spec",
                "[user] - Toggles spectation for next round. Moderators can force-spectate a user.",
                Spectate
            ));

            AddCommand(new Command(
                "spec_wl",
                "- Toggles whitelist-only mode. Moderator-only.",
                SpectateWhitelist
            ));

            AddCommand(new Command(
                "spec_add",
                "<user> - Adds user to whitelist. Moderator-only.",
                SpectateAddWhitelist
            ));

            AddCommand(new Command(
                "spec_rm",
                "<account> - Removes account from whitelist (`*` clears all). Moderator-only.",
                SpectateRmWhitelist
            ));
        }
    }
}
