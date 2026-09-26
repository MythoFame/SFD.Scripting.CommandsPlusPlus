using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Pulls nearby players and objects toward the player (magnet) or
    /// pushes them away (repulse). The sign of <see cref="Force"/> selects
    /// the direction: positive attracts, negative repels.
    /// </summary>
    public class Magnet : Ability
    {
        [Flags]
        public enum Target
        {
            Players = 1,
            Objects = 2,
            All = Players | Objects
        }

        private const float EFFECT_INTERVAL = 250;
        private const float MIN_DISTANCE = 20;
        private const float DEF_FORCE = 2;
        private const float DEF_AREA_SIZE = 250;

        /// <summary>Pull strength. Positive attracts, negative repels.</summary>
        public float Force = DEF_FORCE;

        /// <summary>Side length of the square effect area.</summary>
        public float AreaSize = DEF_AREA_SIZE;

        /// <summary>Which targets are affected.</summary>
        public Target Targets = Target.All;


        private float _effectTimer;

        /// <summary>Every live instance, at most one per player UniqueID. Instances self-register on construction.</summary>
        private static readonly List<Magnet> _instances = [];

        public Magnet(IPlayer player) : base(player)
        {
            _instances.Add(this);

            // Start dormant: the base constructor enables, but the toggle
            // logic assumes fresh instances begin disabled.
            Enabled = false;
        }

        /// <summary>Finds the instance bound to a player UniqueID, if any.</summary>
        public static Magnet Find(int uniqueID) => _instances.FirstOrDefault(m => m.Player != null && m.Player.UniqueID == uniqueID);

        /// <summary>Drops instances bound to gone players so stale references never linger.</summary>
        public static void PurgeStale() => _instances.RemoveAll(m => m.Player == null || m.Player.IsRemoved || m.Player.IsDead);

        public override void OnEnabled(bool enabled)
        {
            if (!enabled)
                Reset();
        }

        private void Reset()
        {
            Force = DEF_FORCE;
            AreaSize = DEF_AREA_SIZE;
            Targets = Target.All;
            _effectTimer = 0;
        }

        public override void Update(float dlt, float dltSecs)
        {
            Area area = Player.GetAABB();
            area.SetDimensions(AreaSize, AreaSize);

            Vector2 center = Player.GetWorldPosition();

            _effectTimer += dlt * Game.SlowmotionModifier;

            bool effect = _effectTimer >= EFFECT_INTERVAL;

            if (effect)
            {
                _effectTimer = 0;
            }

            string hint = Force > 0 ? EffectName.ItemGleam : EffectName.Electric;

            foreach (IObject target in Game.GetObjectsByArea(area))
            {
                if (target == null || target.IsRemoved) continue;

                Vector2 pos = target.GetWorldPosition();

                if (Vector2.Distance(center, pos) <= MIN_DISTANCE && Force > 0) continue;

                if (target is IPlayer player)
                {
                    if (!Targets.HasFlag(Target.Players) || player.IsDead || player.UniqueID == Player.UniqueID) continue;

                    PlayerHelper.Unstick(player);
                }
                else if (!Targets.HasFlag(Target.Objects) ||
                    target.GetBodyType() != BodyType.Dynamic) continue;

                target.SetLinearVelocity(Vector2Helper.DirectionTo(pos, center) * Force);

                target.SetAngularVelocity(Force);

                if (effect)
                {
                    Game.PlayEffect(hint, pos);
                }
            }
        }
    }
}
