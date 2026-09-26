namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Fun commands.
    /// </summary>
    public sealed partial class FunModule : CommandsModule
    {
        public override string Name => "Fun";

        public override string Description => "Fun module";

        public FunModule()
        {
            AddCommand(new Command(
                "lightning",
                "<player> - Summons a lightning strike upon a player. Moderator-only.",
                Lightning
            ));

            AddCommand(new Command(
                "graffiti",
                "<text> - Creates floating graffiti text at your position.",
                Graffiti, CommandPermision.Everyone
            ));

            AddCommand(new Command(
                "fart",
                "- Makes you fart.",
                Fart, CommandPermision.Everyone
            ));

            AddCommand(new Command(
                "suicide",
                "- Die dramatically.",
                Suicide, CommandPermision.Everyone
            ));

            AddCommand(new Command(
                "clone",
                "<player> [team] [ai] - Spawns a clone of a player, optionally on a given team with the given AI. Moderator-only.",
                Clone
            ));

            AddCommand(new Command(
                "anvil",
                "<player> - Drops a heavy object onto a player. Moderator-only.",
                Anvil
            ));

            AddCommand(new Command(
                "bot",
                "[team] [ai] [name] - Spawns a robot, optionally on a given team with the given AI and name. Moderator-only.",
                Bot
            ));

            AddCommand(new Command(
                "color",
                "<player> <color> - Recolors all of a player's clothing. Moderator-only.",
                ColorCommand
            ));

            AddCommand(new Command(
                "bullet",
                "<player> [id] - Sets custom bullets for a player, or disables them if no ID is given. Moderator-only.",
                Bullet
            ));
        }
    }
}
