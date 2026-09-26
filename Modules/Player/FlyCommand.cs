using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void FlyCommand(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /fly <player>", Color.Red, uid);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            // Drop abilities bound to gone players so stale references never linger.
            Fly.PurgeStale();

            int affected = 0;
            string lastName = string.Empty;
            bool lastState = false;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                Fly existing = Fly.Find(player.UniqueID);

                if (existing == null)
                    existing = new Fly(player);
                else
                    existing.Enabled = !existing.Enabled;

                lastName = player.Name;
                lastState = existing.Enabled;
                affected++;
            }

            if (affected == 0) return;

            if (affected == 1)
            {
                Game.ShowChatMessage($"Flying {(lastState ? "enabled" : "disabled")} for {lastName}.",
                    Color.Green, uid);
            }
            else
            {
                Game.ShowChatMessage($"Toggled flying for {affected} player(s).",
                    Color.Green, uid);
            }
        }
    }
}
