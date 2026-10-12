using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Bottom(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /bottom <height>", Color.Red, uid);
                return;
            }

            if (!int.TryParse(tokens[0], out int height))
            {
                Game.ShowChatMessage($"Invalid height '{tokens[0]}'.", Color.Red, uid);
                return;
            }

            Game.SetWorldBottom(height);

            Game.ShowChatMessage($"World bottom set to {height}.", Color.Green, uid);
        }
    }
}
