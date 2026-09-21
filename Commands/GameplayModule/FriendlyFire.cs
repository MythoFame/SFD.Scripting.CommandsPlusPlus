using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void FriendlyFire(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 0)
            {
                Game.ShowChatMessage("Usage: /friendly_fire", Color.Red, uid);
                return;
            }

            FriendlyFireRule.IsEnabled = !FriendlyFireRule.IsEnabled;

            Game.ShowChatMessage($"Friendly fire {(FriendlyFireRule.IsEnabled ? "disabled" : "enabled")}.", Color.Green, uid);
        }
    }
}
