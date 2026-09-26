using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public class Fly : Ability
    {
        private const float SPEED = 5;

        /// <summary>Every live instance, at most one per player UniqueID. Instances self-register on construction.</summary>
        private static readonly List<Fly> _instances = [];

        public Fly(IPlayer player) : base(player)
        {
            _instances.Add(this);
        }

        /// <summary>Finds the instance bound to a player UniqueID, if any.</summary>
        public static Fly Find(int uniqueID) => _instances.FirstOrDefault(fly => fly.Player != null && fly.Player.UniqueID == uniqueID);

        /// <summary>Drops instances bound to gone players so stale references never linger.</summary>
        public static void PurgeStale() => _instances.RemoveAll(fly => fly.Player == null || fly.Player.IsRemoved || fly.Player.IsDead);

        public override void OnEnabled(bool enabled) { }

        public override void Update(float dlt, float dltSecs)
        {
            Vector2 vel = Player.GetLinearVelocity();

            vel.X /= 2;

            if (Player.KeyPressed(VirtualKey.JUMP))
            {
                vel.Y = SPEED;

                Player.SetLinearVelocity(vel);

                EffectsWrapper.PlayTraceSpawner(Player, EffectName.DustTrail, dltSecs);
            }
            else if (vel.Y < -SPEED)
            {
                vel.Y = -SPEED;

                Player.SetLinearVelocity(vel);
            }
        }
    }
}
