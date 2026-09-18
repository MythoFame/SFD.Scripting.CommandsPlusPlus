using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Rsboard(UserMessageCallbackArgs args)
        {
            Game.ResetScore();

            Game.ShowChatMessage("Win ratio statistics reset.", Color.Green, args.User.UserIdentifier);
        }
    }
}
