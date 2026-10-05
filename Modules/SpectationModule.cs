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
            AddCommand("spec", Spectate,
                "[user] - Toggles spectation for next round. Moderators can force-spectate a user.");
            AddCommand("spec_wl", SpectateWhitelist,
                "- Toggles whitelist-only mode.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("spec_add", SpectateAddWhitelist,
                "<user> - Adds user to whitelist.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("spec_rm", SpectateRmWhitelist,
                "<account> - Removes account from whitelist (`*` clears all).",
                permission: CommandHandler.Permission.ModeratorOnly);
        }
    }
}
