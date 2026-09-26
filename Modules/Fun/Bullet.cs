using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private static void Bullet(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length == 0 || tokens.Length > 2)
            {
                Game.ShowChatMessage("Usage: /bullet <player> [id]", Color.Red, uid);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            // Drop abilities bound to gone players so stale references never linger.
            CustomBullet.PurgeStale();

            bool enable = tokens.Length == 2;
            int affected = 0;
            string lastName = string.Empty;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                lastName = player.Name;

                if (enable)
                {
                    CustomBullet existing = CustomBullet.Find(player.UniqueID);

                    existing ??= new CustomBullet(player);

                    existing.ObjectID = tokens[1];
                    existing.Enabled = true;
                    affected++;
                }
                else
                {
                    CustomBullet existing = CustomBullet.Find(player.UniqueID);

                    if (existing != null)
                    {
                        existing.Enabled = false;
                        affected++;
                    }
                }
            }

            if (affected == 0)
            {
                if (!enable)
                {
                    if (players.Length == 1 && !string.IsNullOrEmpty(lastName))
                        Game.ShowChatMessage($"{lastName} has no custom bullets.", Color.Yellow, uid);
                    else
                        Game.ShowChatMessage("No players had custom bullets.", Color.Yellow, uid);
                }

                return;
            }

            if (enable)
            {
                if (affected == 1)
                {
                    Game.ShowChatMessage($"Custom bullets set to '{tokens[1]}' for {lastName}.",
                        Color.Green, uid);
                }
                else
                {
                    Game.ShowChatMessage($"Custom bullets set to '{tokens[1]}' for {affected} player(s).",
                        Color.Green, uid);
                }
            }
            else
            {
                if (affected == 1)
                {
                    Game.ShowChatMessage($"Custom bullets disabled for {lastName}.",
                        Color.Green, uid);
                }
                else
                {
                    Game.ShowChatMessage($"Disabled custom bullets for {affected} player(s).",
                        Color.Green, uid);
                }
            }
        }
    }
}
