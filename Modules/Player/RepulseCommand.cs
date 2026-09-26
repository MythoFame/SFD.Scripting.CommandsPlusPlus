using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void RepulseCommand(UserMessageCallbackArgs args) => MagnetControl(args, attract: false);
    }
}
