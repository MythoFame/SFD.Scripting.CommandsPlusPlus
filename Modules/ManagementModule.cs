namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Required core module for managing all other modules.
    /// Always enabled and cannot be disabled.
    /// </summary>
    public sealed partial class ManagementModule : CommandsModule
    {
        public override string Name => "Management";

        public override string Description => "Management module";

        public ManagementModule()
        {
            AddCommand(new Command(
                "modules",
                "Display all modules along with whether they are enabled or disabled",
                ShowModules, CommandPermision.Everyone
            ));

            AddCommand(new Command(
                "commands",
                "[module] - Display all commands along with their help",
                ShowCommands, CommandPermision.Everyone
            ));

            AddCommand(new Command(
                "toggle_module",
                "<module> - Toggles whether a module is blocked",
                ToggleModule, CommandPermision.Host
            ));

            AddCommand(new Command(
                "reset_module",
                "Resets all modules to their default unblocked state",
                ResetModules, CommandPermision.Host
            ));
        }
    }
}