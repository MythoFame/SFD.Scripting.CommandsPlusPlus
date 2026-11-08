using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Revive(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /revive <user>", Color.Red, uid);
                return;
            }

            IUser[] users = [.. ParseHelper.ParseUsers(tokens[0], args.User)];

            if (users.Length == 0)
            {
                Game.ShowChatMessage($"User '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            int revived = 0;
            int spawned = 0;
            int skipped = 0;

            foreach (IUser user in users)
            {
                if (user == null) continue;

                IPlayer player = user.GetPlayer();

                if (player == null)
                {
                    PlayerHelper.Spawn(user, PathGridHelper.GetRandomPathGridPosition(Random.Shared));
                    spawned++;
                }
                else if (player.IsDead)
                {
                    PlayerHelper.Revive(player);
                    revived++;
                }
                else
                {
                    skipped++;
                }
            }

            if (revived > 0)
            {
                Game.ShowChatMessage($"Revived {revived} player(s).", Color.Green, uid);
            }

            if (spawned > 0)
            {
                Game.ShowChatMessage($"Spawned {spawned} player(s).", Color.Green, uid);
            }

            if (skipped > 0)
            {
                Game.ShowChatMessage($"{skipped} player(s) were not dead.", Color.Yellow, uid);
            }
        }
    }
}
