using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Clear(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /clear <id>", Color.Red, uid);
                return;
            }

            int removed = 0;

            foreach (IObject obj in Game.GetObjectsByName(tokens[0]))
            {
                if (obj == null || obj.IsRemoved) continue;

                obj.Remove();
                removed++;
            }

            Game.ShowChatMessage($"Removed {removed} object(s) named '{tokens[0]}'.", Color.Green, uid);
        }
    }
}
