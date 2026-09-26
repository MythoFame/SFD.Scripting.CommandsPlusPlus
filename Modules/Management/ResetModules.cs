using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Modules.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Management;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void ResetModules(UserMessageCallbackArgs args)
        {
            int uid = args.User?.UserIdentifier ?? -1;
            ModuleRegistry.ResetAll();
<<<<<<< HEAD:Modules/Management/ResetModules.cs
            Game.ShowChatMessage("All modules reset to their default enabled state.", Color.Green, args.User.UserIdentifier);
=======

            Game.ShowChatMessage("All modules reset to their default state.", Color.Green, uid);
>>>>>>> cf508c181bd9486ea397da7250feb8128dd8b058:Commands/ManagementModule/ResetModules.cs
        }
    }
}
