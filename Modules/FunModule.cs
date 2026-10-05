namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Light-hearted extras and visual gags. Permission varies per command.
    /// Command implementations live in <c>Commands/FunModule/</c> as
    /// partial declarations of this class.
    /// </summary>
    public sealed partial class FunModule : CommandsModule
    {
        public override string Name => "Fun";

        public override string Description => "Light-hearted extras and visual gags.";

        public FunModule()
        {
            AddCommand("lightning", Lightning,
                "<player> - Summons a lightning strike upon a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("graffiti", Graffiti,
                "<text> - Creates floating graffiti text at your position.");
            AddCommand("fart", Fart,
                "- Makes you fart.");
            AddCommand("suicide", Suicide,
                "- Die dramatically.");
            AddCommand("clone", Clone,
                "<player> [team] [ai] - Spawns a clone of a player, optionally on a given team with the given AI.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("anvil", Anvil,
                "<player> - Drops a heavy object onto a player.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("bot", Bot,
                "[team] [ai] [name] - Spawns a robot, optionally on a given team with the given AI and name.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("color", ColorCommand,
                "<player> <color> - Recolors all of a player's clothing.",
                permission: CommandHandler.Permission.ModeratorOnly);
            AddCommand("bullet", Bullet,
                "<player> [id] - Sets custom bullets for a player, or disables them if no ID is given.",
                permission: CommandHandler.Permission.ModeratorOnly);
        }
    }
}
