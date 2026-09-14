using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void Modules(UserMessageCallbackArgs args)
        {
            Game.ShowChatMessage("Modules:", Color.Green, args.User.UserIdentifier);

            foreach (CommandsModule module in ModuleRegistry.All.OrderBy(module => module.Name))
            {
                string state = module.IsEnabled ? "enabled" : "disabled";
                Color color = module.IsEnabled ? Color.Green : Color.Red;

                Game.ShowChatMessage($"{module.Name}: {state} - {module.Description}", color, args.User.UserIdentifier);
            }
        }
    }
}
