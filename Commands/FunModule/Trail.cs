using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private static void Trail(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /trail <effect>", Color.Red, uid);
                return;
            }

            string effect = EffectHelper.ResolveEffectName(tokens[0], out string[] suggestions);

            if (effect == null)
            {
                Game.ShowChatMessage($"Unknown effect '{tokens[0]}'.", Color.Red, uid);

                if (suggestions.Length > 0)
                    Game.ShowChatMessage(EffectHelper.DidYouMean(suggestions), Color.Red, uid);

                return;
            }

            if (args.User == null) return;

            TrailRule.SetTrail(args.User.AccountName, effect);

            Game.ShowChatMessage($"Trail effect set to '{EffectHelper.GetDisplayName(effect)}'. Takes effect next round.", Color.Green, uid);
        }
    }
}
