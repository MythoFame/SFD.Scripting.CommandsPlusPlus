using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus.Utils;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Helpers for striking lightning upon the map.
    /// </summary>
    public static class LightningHelper
    {
        private const float STRIKE_RADIUS = 50;
        private const float STRIKE_DAMAGE = 9;
        private const float STRIKE_FORCE = 9;
        private const int STRIKE_FIRE_NODE_COUNT = 15;
        private const float STRIKE_FIRE_EXPAND_VELOCITY = 3;
        private const float BOLT_OFFSET = 20;
        private const float BOLT_POINT_DISTANCE = 15;
        private const float EXPLOSION_SHAKE_INTENSITY = 10;
        private const float EXPLOSION_SHAKE_DURATION = 1000; // ms

        /// <summary>
        /// Strikes lightning from a random point in the sky down onto
        /// <paramref name="end"/>, igniting it and damaging, knocking down and scattering
        /// every destructable object around it.
        /// </summary>
        /// <param name="end">The point the lightning strikes.</param>
        public static void Strike(Vector2 end, Random random)
        {
            StrikeEffect(end, random);

            Game.SpawnFireNodes(end, STRIKE_FIRE_NODE_COUNT, STRIKE_FIRE_EXPAND_VELOCITY);

            Vector2 radius = new(STRIKE_RADIUS);

            Area effectArea = new(end - radius, end + radius);

            foreach (IObject obj in Game.GetObjectsByArea(effectArea))
            {
                if (!obj.Destructable)
                    continue;

                obj.DealDamage(STRIKE_DAMAGE);

                if (obj is IPlayer player)
                    player.Fall();

                obj.SetLinearVelocity(new(
                    (random.NextSingle() * 2 - 1) * STRIKE_FORCE,
                    random.NextSingle() * STRIKE_FORCE));
            }
        }

        /// <summary>
        /// Displays the lightning strike visual and audio effects, from a random point in
        /// the sky down onto <paramref name="end"/>.
        /// </summary>
        /// <param name="end">The point the lightning strikes.</param>
        public static void StrikeEffect(Vector2 end, Random random) => StrikeEffect(PathGridHelper.GetSkyPosition(random), end, random);

        /// <summary>
        /// Displays the lightning strike visual and audio effects, from
        /// <paramref name="top"/> down onto <paramref name="end"/>.
        /// </summary>
        /// <param name="top">The point the bolt originates from.</param>
        /// <param name="end">The point the lightning strikes.</param>
        public static void StrikeEffect(Vector2 top, Vector2 end, Random random)
        {
            Lightning(v => Game.PlayEffect(EffectName.ItemGleam, v), top, end, random, BOLT_OFFSET, BOLT_POINT_DISTANCE);

            Game.PlayEffect(EffectName.Explosion, end);

            Game.PlayEffect(EffectName.CameraShaker, Vector2.Zero, EXPLOSION_SHAKE_INTENSITY, EXPLOSION_SHAKE_DURATION, true);

            Game.PlaySound(SoundsDatabase.Explosion, Vector2.Zero);
            Game.PlaySound(SoundsDatabase.BarrelExplode, Vector2.Zero);
        }

        /// <summary>
        /// Traces a jagged lightning bolt between two points, displacing every point of
        /// the trail randomly along the direction perpendicular to the bolt.
        /// </summary>
        /// <param name="func">The action to perform on each point of the bolt.</param>
        /// <param name="origin">The starting point of the bolt.</param>
        /// <param name="end">The ending point of the bolt.</param>
        /// <param name="offset">The maximum displacement of each point of the bolt.</param>
        /// <param name="pointDistance">The distance between each point of the bolt.</param>
        public static void Lightning(Action<Vector2> func, Vector2 origin, Vector2 end, Random random,
            float offset = 5, float pointDistance = 0.1f)
        {
            Vector2 direction = end - origin;

            if (direction.LengthSquared() < float.Epsilon)
            {
                func(origin);
                return;
            }

            Vector2 displacement = Vector2Helper.Orthogonal(Vector2.Normalize(direction));

            PointShape.Trail(point =>
            {
                func(point + displacement * (random.NextSingle() * 2 - 1) * offset);
            }, origin, end, pointDistance);
        }
    }
}
