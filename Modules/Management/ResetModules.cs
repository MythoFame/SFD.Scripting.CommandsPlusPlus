using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class ManagementModule
    {
        private static void ResetModules(UserMessageCallbackArgs args)
        {
            int uid = args.User?.UserIdentifier ?? -1;
            ModuleRegistry.ResetAll();

            Game.ShowChatMessage("All modules reset to their default state.", Color.Green, uid);
        }
    }
}
