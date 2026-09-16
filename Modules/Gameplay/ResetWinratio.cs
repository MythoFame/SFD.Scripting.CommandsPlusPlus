using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Gameplay;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void ResetWinratio(UserMessageCallbackArgs args)
        {
            Game.ResetScore();

            Game.ShowChatMessage("Win ratio statistics reset.", Color.Green, args.User.UserIdentifier);
        }
    }
}
