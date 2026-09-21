using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Regen(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /regen <hp>", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (!float.TryParse(tokens[0], out float amount))
            {
                Game.ShowChatMessage($"Invalid amount '{tokens[0]}'.", Color.Red, args.User.UserIdentifier);
                return;
            }

            RegenRule.Amount = amount;

            if (amount > 0)
                Game.ShowChatMessage($"Health regen set to {amount} per second.", Color.Green, args.User.UserIdentifier);
            else
                Game.ShowChatMessage("Health regen disabled.", Color.Green, args.User.UserIdentifier);
        }
    }
}
