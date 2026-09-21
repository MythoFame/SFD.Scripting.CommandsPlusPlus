namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// The control center. Browse commands and manage other modules. Always
    /// available and cannot be restricted.
    /// Command implementations live in <c>Commands/ManagementModule/</c> as
    /// partial declarations of this class.
    /// </summary>
    public sealed partial class ManagementModule : CommandsModule
    {
        public override string Name => "Management";

        public override string Description => "The control center. Browse commands and manage other modules.";

        public ManagementModule()
        {
            AddCommand("modules", Modules,
                "- Display all modules along with whether they are allowed or restricted.");
            AddCommand("commands", ShowCommands,
                "[module] - Display all commands along with their help. If a module is provided, then display only that module's commands and help.");
            AddCommand("toggle_module", ToggleModule,
                "<module> - Toggles whether a module is restricted. Restricted modules have all their commands limited to the host.",
                hostOnly: true);
            AddCommand("reset_modules", ResetModules,
                "- Resets all modules to their default allowed state. Host-only, persisted.",
                hostOnly: true);
        }
    }
}
