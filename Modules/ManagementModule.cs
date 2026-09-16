namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Required core module for managing all other modules. Always allowed
    /// and cannot be restricted.
    /// Command implementations live in <c>Commands/ManagementModule/</c> as
    /// partial declarations of this class.
    /// </summary>
    public sealed partial class ManagementModule : CommandsModule
    {
        public override string Name => "Management";

        public override string Description => "Management module";

        public ManagementModule()
        {
            AddCommand("modules", Modules,
                "- Display all modules along with whether they are allowed or restricted");
            AddCommand("commands", ShowCommands,
                "[module] - Display all commands along with their help");
            AddCommand("toggle_module", ToggleModule,
                "<module> - Toggles whether a module is restricted",
                hostOnly: true);
            AddCommand("reset_modules", ResetModules,
                "- Resets all modules to their default allowed state",
                hostOnly: true);
        }
    }
}
