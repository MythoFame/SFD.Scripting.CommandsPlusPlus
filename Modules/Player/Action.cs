using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void ActionCommand(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 2)
            {
                ShowActionUsage(args);
                return;
            }

            if (!Enum.TryParse(tokens[1], true, out PlayerCommandType action) || !Enum.IsDefined(action))
            {
                Game.ShowChatMessage($"Invalid action '{tokens[1]}'.", Color.Red, uid);
                ShowActionUsage(args);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            int affected = 0;
            int skipped = 0;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                if (player.IsInputEnabled)
                {
                    skipped++;
                    continue;
                }

                player.AddCommand(new PlayerCommand(action));
                affected++;
            }

            if (affected > 0)
            {
                Game.ShowChatMessage($"Queued {action} for {affected} player(s).", Color.Green, uid);
            }

            if (skipped > 0)
            {
                Game.ShowChatMessage($"{skipped} player(s) must have their input disabled.", Color.Yellow, uid);
            }
        }

        /// <summary>
        /// Shows usage plus every <see cref="PlayerCommandType"/> value, five
        /// per message so no single chat message hits the length limit.
        /// </summary>
        private static void ShowActionUsage(UserMessageCallbackArgs args)
        {
            int uid = args.User?.UserIdentifier ?? -1;
            Game.ShowChatMessage("Usage: /action <player> <action>. Actions:", Color.Red, uid);

            List<string> chunk = [];

            foreach (string name in Enum.GetNames<PlayerCommandType>())
            {
                chunk.Add(name);

                if (chunk.Count == 15)
                {
                    Game.ShowChatMessage(string.Join(", ", chunk), Color.Red, uid);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
                Game.ShowChatMessage(string.Join(", ", chunk), Color.Red, uid);
        }
    }
}
