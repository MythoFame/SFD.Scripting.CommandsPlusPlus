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
                "<player> - Summons a lightning strike upon a player. Moderator-only.",
                moderatorOnly: true);
            AddCommand("graffiti", Graffiti,
                "<text> - Creates floating graffiti text at your position.");
            AddCommand("fart", Fart,
                "- Makes you fart.");
            AddCommand("suicide", Suicide,
                "- Die dramatically.");
            AddCommand("clone", Clone,
                "<player> [team] [ai] - Spawns a clone of a player, optionally on a given team with the given AI. Moderator-only.",
                moderatorOnly: true);
            AddCommand("anvil", Anvil,
                "<player> - Drops a heavy object onto a player. Moderator-only.",
                moderatorOnly: true);
            AddCommand("bot", Bot,
                "[team] [ai] [name] - Spawns a robot, optionally on a given team with the given AI and name. Moderator-only.",
                moderatorOnly: true);
            AddCommand("color", ColorCommand,
                "<player> <color> - Recolors all of a player's clothing. Moderator-only.",
                moderatorOnly: true);
            AddCommand("bullet", Bullet,
                "<player> [id] - Sets custom bullets for a player, or disables them if no ID is given. Moderator-only.",
                moderatorOnly: true);
        }
    }
}
