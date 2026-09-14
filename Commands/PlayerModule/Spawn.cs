using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Spawn(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /spawn <id>", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer self = args.User.GetPlayer();

            if (self == null || self.IsRemoved)
            {
                Game.ShowChatMessage("You have no live player to spawn at.", Color.Red, args.User.UserIdentifier);
                return;
            }

            IObject obj = Game.CreateObject(tokens[0], Vector2.Zero);

            Area aabb = obj.GetAABB();
            Vector2 playerPos = self.GetWorldPosition();
            obj.SetWorldPosition(new(playerPos.X + (aabb.Width / 2f + 16f) * self.FacingDirection, playerPos.Y));

            Game.ShowChatMessage($"Spawned '{tokens[0]}'.", Color.Green, args.User.UserIdentifier);
        }
    }
}
