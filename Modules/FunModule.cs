using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Lightweight fun commands.
    /// Command implementations live in <c>Commands/FunModule/</c> as
    /// partial declarations of this class.
    /// </summary>
    public sealed partial class FunModule : CommandsModule
    {
        public override string Name => "Fun";

        public override string Description => "Fun module";

        public FunModule()
        {
            AddCommand("lightning", Lightning,
                "<player> - Summons a lightning strike upon a player",
                moderatorOnly: true);
            AddCommand("graffiti", Graffiti,
                "<text> - Creates floating graffiti text at your position");
            AddCommand("fart", Fart,
                "- Makes you fart");
            AddCommand("suicide", Suicide,
                "- Die dramatically");
            AddCommand("clone", Clone,
                "<player> [team] [ai] - Spawns a clone of a player, optionally on a given team with the given AI",
                moderatorOnly: true);
            AddCommand("anvil", Anvil,
                "<player> - Drops a heavy object onto a player",
                moderatorOnly: true);
            AddCommand("bot", Bot,
                "[team] [ai] [name] - Spawns a robot, optionally on a given team with the given AI and name",
                moderatorOnly: true);
        }

        public override void OnEnable()
        {
        }

        public override void OnDisable()
        {
        }
    }
}
