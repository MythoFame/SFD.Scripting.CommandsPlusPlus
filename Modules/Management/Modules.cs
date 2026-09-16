using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Modules.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Management;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void ShowModules(UserMessageCallbackArgs args)
        {
            Game.ShowChatMessage("Modules:", Color.Green, args.User.UserIdentifier);

            foreach (CommandsModule module in ModuleRegistry.Modules.OrderBy(module => module.Name))
            {
                string state = module.IsEnabled ? "blocked" : "active";
                Color color = module.IsEnabled ? Color.Red : Color.Green;
                Game.ShowChatMessage($"{module.Name}: {state} - {module.Description}", color, args.User.UserIdentifier);
            }
        }
    }
}
