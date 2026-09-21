using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Tag(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length == 0 || tokens.Length > 2)
            {
                Game.ShowChatMessage("Usage: /tag <player> [name|status]", Color.Red, uid);
                return;
            }

            bool toggleName = true;
            bool toggleStatus = true;

            if (tokens.Length == 2)
            {
                if (string.Equals(tokens[1], "name", StringComparison.OrdinalIgnoreCase))
                    toggleStatus = false;
                else if (string.Equals(tokens[1], "status", StringComparison.OrdinalIgnoreCase))
                    toggleName = false;
                else
                {
                    Game.ShowChatMessage("Mode must be 'name' or 'status'.", Color.Red, uid);
                    return;
                }
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            int affected = 0;
            string lastName = string.Empty;
            bool lastNameVisible = false;
            bool lastStatusVisible = false;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                if (toggleName)
                    player.SetNametagVisible(!player.GetNametagVisible());

                if (toggleStatus)
                    player.SetStatusBarsVisible(!player.GetStatusBarsVisible());

                lastName = player.Name;
                lastNameVisible = player.GetNametagVisible();
                lastStatusVisible = player.GetStatusBarsVisible();
                affected++;
            }

            if (affected == 0) return;

            if (affected == 1)
            {
                List<string> parts = [];

                if (toggleName)
                    parts.Add($"nametag {(lastNameVisible ? "visible" : "hidden")}");
                if (toggleStatus)
                    parts.Add($"status bars {(lastStatusVisible ? "visible" : "hidden")}");

                Game.ShowChatMessage($"{lastName}: {string.Join(" and ", parts)}.", Color.Green, uid);
            }
            else
            {
                Game.ShowChatMessage($"Toggled tags for {affected} player(s).", Color.Green, uid);
            }
        }
    }
}
