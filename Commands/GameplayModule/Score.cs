using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Score(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /score <amount>", Color.Red, uid);
                return;
            }

            if (!int.TryParse(tokens[0], out int amount))
            {
                Game.ShowChatMessage($"Invalid amount '{tokens[0]}'.", Color.Red, uid);
                return;
            }

            if (Game.GetMapType() != MapType.Survival)
            {
                Game.ShowChatMessage("Score is only available in survival games.", Color.Red, uid);
                return;
            }

            Game.TotalScore = amount;

            Game.ShowChatMessage($"Score set to {amount}.", Color.Green, uid);
        }
    }
}
