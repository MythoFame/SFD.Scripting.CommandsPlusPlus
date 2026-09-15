using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public class Clip : Ability
    {
        private const float SPEED = 8;

        /// <summary>Every live instance, at most one per player UniqueID. Instances self-register on construction.</summary>
        private static readonly List<Clip> _instances = [];

        public Clip(IPlayer player) : base(player)
        {
            _instances.Add(this);
        }

        /// <summary>Finds the instance bound to a player UniqueID, if any.</summary>
        public static Clip Find(int uniqueID) => _instances.FirstOrDefault(clp => clp.Player != null && clp.Player.UniqueID == uniqueID);

        /// <summary>Drops instances bound to gone players so stale references never linger.</summary>
        public static void PurgeStale() => _instances.RemoveAll(clp => clp.Player == null || clp.Player.IsRemoved || clp.Player.IsDead);

        public override void OnDisabled()
        {
        }

        public override void OnEnabled()
        {
        }

        public override void Update(float dlt, float dltSecs)
        {
            // Noclip is kinematic: cancel physics velocity every tick, then move manually.
            Player.SetLinearVelocity(Vector2.Zero);

            float x = 0f;

            if (Player.KeyPressed(VirtualKey.AIM_RUN_RIGHT))
                x += 1f;

            if (Player.KeyPressed(VirtualKey.AIM_RUN_LEFT))
                x -= 1f;

            float y = 0f;

            if (Player.KeyPressed(VirtualKey.AIM_CLIMB_UP) || Player.KeyPressed(VirtualKey.JUMP))
                y += 1f;

            if (Player.KeyPressed(VirtualKey.AIM_CLIMB_DOWN))
                y -= 1f;

            if (x == 0f && y == 0f)
                return;

            Vector2 direction = new(x, y);
            direction.Normalize();

            Player.SetWorldPosition(Player.GetWorldPosition() + direction * SPEED);
        }
    }
}
