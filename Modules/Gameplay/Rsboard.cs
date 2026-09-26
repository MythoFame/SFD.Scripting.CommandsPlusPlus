using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Gameplay;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Rsboard(UserMessageCallbackArgs args)
        {
            int uid = args.User?.UserIdentifier ?? -1;
            Game.ResetScore();

            Game.ShowChatMessage("Win ratio statistics reset.", Color.Green, uid);
        }
    }
}
