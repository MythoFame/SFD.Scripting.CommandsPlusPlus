using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public class CustomBullet : Ability
    {
        private const float BOUNCE_SPIN = 500;
        private const float DEFAULT_SPIN = 30;
        private const float VELOCITY_SCALE = 0.03f;
        private const uint DESTROY_DELAY = 10000;

        /// <summary>Every live instance, at most one per player UniqueID. Instances self-register on construction.</summary>
        private static readonly List<CustomBullet> _instances = [];

        private Events.ProjectileCreatedCallback _projectileCreated = null;

        public string ObjectID = string.Empty;

        public CustomBullet(IPlayer player) : base(player)
        {
            _instances.Add(this);
        }

        /// <summary>Finds the instance bound to a player UniqueID, if any.</summary>
        public static CustomBullet Find(int uniqueID) => _instances.FirstOrDefault(cb => cb.Player != null && cb.Player.UniqueID == uniqueID);

        /// <summary>Drops instances bound to gone players so stale references never linger.</summary>
        public static void PurgeStale() => _instances.RemoveAll(cb => cb.Player == null || cb.Player.IsRemoved || cb.Player.IsDead);

        public override void OnEnabled(bool enabled)
        {
            if (enabled)
            {
                _projectileCreated = Game.Events.StartProjectileCreatedCallback(OnProjectileCreated);
            }
            else
            {
                _projectileCreated.Stop();

                _projectileCreated = null;
            }
        }

        private void OnProjectileCreated(IProjectile[] projs)
        {
            foreach(IProjectile proj in projs)
            {
                if (proj.InitialOwnerPlayerID != Player.UniqueID) continue;

                float angularVel = proj.PowerupBounceActive ? BOUNCE_SPIN : DEFAULT_SPIN;

                IObject obj = Game.CreateObject(ObjectID, Vector2.Zero,
                    Vector2Helper.Angle(proj.Direction), proj.Velocity * VELOCITY_SCALE, angularVel);

                // Push the spawn point forward by the object's half-extent
                // along the shot direction so big objects don't clip into
                // the shooter.
                Area aabb = obj.GetAABB();

                Vector2 direction = proj.Direction;

                if (direction.LengthSquared() > 0)
                    direction = Vector2.Normalize(direction);

                float clearance = Math.Abs(direction.X) * aabb.Width / 2f
                    + Math.Abs(direction.Y) * aabb.Height / 2f + 8f;

                obj.SetWorldPosition(proj.Position + direction * clearance);

                obj.SetBodyType(BodyType.Dynamic);

                obj.TrackAsMissile(true);

                if (proj.PowerupFireActive)
                {
                    obj.SetMaxFire();
                }

                if (proj.PowerupBounceActive)
                {
                    EffectsWrapper.PlayTraceSpawner(obj, EffectName.Electric, 1);

                    obj.SetMass(obj.GetMass() / 2);
                }

                proj.FlagForRemoval();

                Game.Events.StartUpdateCallback(_ =>
                {
                    obj?.Destroy();
                }, DESTROY_DELAY, 1);
            }
        }

        public override void Update(float dlt, float dltSecs) { }
    }
}
