using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Modules.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Management;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void ResetModules(UserMessageCallbackArgs args)
        {
            ModuleRegistry.ResetAll();
            Game.ShowChatMessage("All modules reset to their default enabled state.", Color.Green, args.User.UserIdentifier);
        }
    }
}
