using static SFD.Scripting.CommandsPlusPlus.Commands.GameScript;
using static SFD.Scripting.CommandsPlusPlus.Modules.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Gameplay;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Custom gameplay rules.
    /// </summary>
    public sealed partial class GameplayModule : CommandsModule
    {
        public override string Name => "Gameplay";

        public override string Description => "Gameplay module";

        public GameplayModule()
        {
            AddCommand(new Command(
                "rsboard",
                "Resets the stored win ratio statistics",
                ResetWinratio
            ));
        }
    }
}
