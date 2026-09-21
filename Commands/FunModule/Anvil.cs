using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private static readonly string[] _anvilObjectIDs =
        [
            "CarnivalCart00",
            "ChurchBell00",
            "ConcretePipe01",
            "Forklift00",
            "HangingCrate00",
            "Car00A",
            "Car01A",
            "Van00",
        ];

        private static void Anvil(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /anvil <player>", Color.Red, uid);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            int affected = 0;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                string objectID = _anvilObjectIDs[_random.Next(_anvilObjectIDs.Length)];

                IObject obj = Game.CreateObject(objectID, Vector2.Zero);

                Area aabb = obj.GetAABB();
                Vector2 playerPos = player.GetWorldPosition();

                obj.SetWorldPosition(new(playerPos.X, playerPos.Y + aabb.Height / 2f + 48f));

                obj.SetLinearVelocity(Vector2Helper.Up);
                EffectsWrapper.PlayTraceSpawner(obj, EffectName.DustTrail, 2f);

                Game.Events.StartUpdateCallback(_ =>
                {
                    obj?.Destroy();
                }, 5000, 1);

                affected++;
            }

            if (affected == 0) return;

            Game.ShowChatMessage($"Dropped an anvil on {affected} player(s).", Color.Green, uid);
        }
    }
}
