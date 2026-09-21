using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Gravity(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /gravity <constant>", Color.Red, uid);
                return;
            }

            float constant;

            if (!float.TryParse(tokens[0], out constant))
            {
                Game.ShowChatMessage($"Invalid amount '{tokens[0]}'.", Color.Red, uid);
                return;
            }

            GravityRule.Constant = constant;

            if (constant != 0f)
                Game.ShowChatMessage($"Gravity constant set to {constant}.", Color.Green, uid);
            else
                Game.ShowChatMessage("Default gravity restored.", Color.Green, uid);
        }
    }
}
