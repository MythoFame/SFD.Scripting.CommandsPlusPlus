using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Modules.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Management;

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
<<<<<<< HEAD:Modules/Management/Modules.cs
                string state = module.IsEnabled ? "blocked" : "active";
                Color color = module.IsEnabled ? Color.Red : Color.Green;
                Game.ShowChatMessage($"{module.Name}: {state} - {module.Description}", color, args.User.UserIdentifier);
=======
                string state = module.IsRestricted ? "restricted" : "allowed";
                Color color = module.IsRestricted ? Color.Red : Color.Green;

                Game.ShowChatMessage($"{module.Name}: {state} - {module.Description}", color, uid);
>>>>>>> cf508c181bd9486ea397da7250feb8128dd8b058:Commands/ManagementModule/Modules.cs
            }
        }
    }
}
