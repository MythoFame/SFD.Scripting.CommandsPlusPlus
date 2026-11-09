using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private static void Effect(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length < 1 || tokens.Length > 2)
            {
                Game.ShowChatMessage("Usage: /effect <effect> [player]", Color.Red, uid);
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

            List<IPlayer> targets = [];

            if (tokens.Length == 2)
            {
                IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[1], args.User)];

                if (players.Length == 0)
                {
                    Game.ShowChatMessage($"Player '{tokens[1]}' not found.", Color.Red, uid);
                    return;
                }

                targets.AddRange(players);
            }
            else
            {
                IPlayer self = args.User.GetPlayer();

                if (self == null || self.IsRemoved)
                {
                    Game.ShowChatMessage("Your character must be active!", Color.Red, uid);
                    return;
                }

                targets.Add(self);
            }

            int affected = 0;

            foreach (IPlayer player in targets)
            {
                if (player == null || player.IsRemoved) continue;

                Game.PlayEffect(effect, player.GetWorldPosition());
                affected++;
            }

            if (affected == 0) return;

            Game.ShowChatMessage($"Played effect '{EffectHelper.GetDisplayName(effect)}' for {affected} player(s).", Color.Green, uid);
        }
    }
}
