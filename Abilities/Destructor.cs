using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public class Destructor : Ability
    {
        /// <summary>Every live instance, at most one per player UniqueID. Instances self-register on construction.</summary>
        private static readonly List<Destructor> _instances = [];

        private Events.ProjectileHitCallback _projectileHit = null;

        public Destructor(IPlayer player) : base(player)
        {
            _instances.Add(this);
        }

        /// <summary>Finds the instance bound to a player UniqueID, if any.</summary>
        public static Destructor Find(int uniqueID) => _instances.FirstOrDefault(d => d.Player != null && d.Player.UniqueID == uniqueID);

        /// <summary>Drops instances bound to gone players so stale references never linger.</summary>
        public static void PurgeStale() => _instances.RemoveAll(d => d.Player == null || d.Player.IsRemoved || d.Player.IsDead);

        public override void OnEnabled(bool enabled)
        {
            if (enabled)
            {
                _projectileHit = Game.Events.StartProjectileHitCallback(OnProjectileHit);
            }
            else
            {
                _projectileHit.Stop();

                _projectileHit = null;
            }
        }

        private void OnProjectileHit(IProjectile projectile, ProjectileHitArgs args)
        {
            if (projectile.InitialOwnerPlayerID != Player.UniqueID) return;

            IObject hit = Game.GetObject(args.HitObjectID);

            if (hit == null || hit.IsRemoved || hit.DestructionInitiated) return;

            EffectsWrapper.PlayCustomFloatText(args.HitPosition, "DESTROYED!", Color.Red);

            hit.Destroy();
        }

        public override void Update(float dlt, float dltSecs) { }
    }
}
