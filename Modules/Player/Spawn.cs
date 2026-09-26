using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Utils.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Player;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Spawn(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /spawn <id>", Color.Red, uid);
                return;
            }

            IPlayer self = args.User.GetPlayer();

            if (self == null || self.IsRemoved)
            {
                Game.ShowChatMessage("You have no live player to spawn at.", Color.Red, uid);
                return;
            }

            IObject obj = Game.CreateObject(tokens[0], Vector2.Zero);

            Area aabb = obj.GetAABB();
            Vector2 playerPos = self.GetWorldPosition();
            obj.SetWorldPosition(new(playerPos.X + (aabb.Width / 2f + 16f) * self.FacingDirection, playerPos.Y));

            Game.ShowChatMessage($"Spawned '{tokens[0]}'.", Color.Green, uid);
        }
    }
}
