using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void ShowModules(UserMessageCallbackArgs args)
        {
            int uid = args.User?.UserIdentifier ?? -1;
            Game.ShowChatMessage("Modules:", Color.Green, uid);

            foreach (CommandsModule module in ModuleRegistry.Modules.OrderBy(module => module.Name))
            {
                string state = module.IsRestricted ? "restricted" : "allowed";
                Color color = module.IsRestricted ? Color.Red : Color.Green;
                Game.ShowChatMessage($"{module.Name}: {state} - {module.Description}", color, uid);
            }
        }
    }
}
