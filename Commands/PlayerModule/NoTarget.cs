using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void NoTarget(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /notarget <player>", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            int affected = 0;
            string lastName = string.Empty;
            bool lastState = false;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                player.SetValidBotEliminateTarget(!player.IsValidBotEliminateTarget);
                lastName = player.Name;
                lastState = player.IsValidBotEliminateTarget;
                affected++;
            }

            if (affected == 0) return;

            if (affected == 1)
            {
                Game.ShowChatMessage($"{lastName} is {(lastState ? "now" : "no longer")} targeted by bots.",
                    Color.Green, args.User.UserIdentifier);
            }
            else
            {
                Game.ShowChatMessage($"Toggled bot targeting for {affected} player(s).",
                    Color.Green, args.User.UserIdentifier);
            }
        }
    }
}
