using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Displays damage numbers while enabled. Hooks object damage and shows
    /// the damage scaled and colored by severity.
    /// </summary>
    public static class DmgNumbersRule
    {
        [Flags]
        public enum DamageTarget
        {
            Players = 1,
            Objects = 2,
            All = Players | Objects
        }

        private const string ENABLED_KEY = "CommandsPlusPlus.Rule.DmgNumbers.Enabled";
        private const string TARGET_KEY = "CommandsPlusPlus.Rule.DmgNumbers.Target";

        private static Events.ObjectDamageCallback _objDmg = null;
        private static Events.PlayerDamageCallback _playerDmg = null;

        /// <summary>
        /// Gets or sets whether the rule is active. Backed directly by storage,
        /// so there is no shadow state to drift. Setting it starts or stops
        /// the damage hook exactly once per transition.
        /// </summary>
        public static bool IsEnabled
        {
            get => Game.LocalStorage.GetItem(ENABLED_KEY) as bool? ?? false;
            set
            {
                if (IsEnabled == value) return;

                Game.LocalStorage.SetItem(ENABLED_KEY, value);
                OnEnabled(value);
            }
        }

        /// <summary>
        /// Restores the persisted state. Call once when the world is live.
        /// </summary>
        public static void Initialize()
        {
            if (IsEnabled)
                OnEnabled(true);
        }

        /// <summary>
        /// Which targets show damage numbers. Backed directly by storage.
        /// Defaults to all targets.
        /// </summary>
        public static DamageTarget Target
        {
            get => (DamageTarget)(Game.LocalStorage.GetItem(TARGET_KEY) as int? ?? (int)DamageTarget.All);
            set
            {
                if (Target == value) return;

                Game.LocalStorage.SetItem(TARGET_KEY, (int)value);
            }
        }

        private static void OnEnabled(bool enabled)
        {
            if (enabled)
            {
                _objDmg = Game.Events.StartObjectDamageCallback(OnObjectDamage);
                _playerDmg = Game.Events.StartPlayerDamageCallback(OnPlayerDamage);
            }
            else
            {
                _objDmg?.Stop();

                _objDmg = null;

                _playerDmg?.Stop();

                _playerDmg = null;
            }
        }

        private static void OnObjectDamage(IObject obj, ObjectDamageArgs args)
        {
            if (!Target.HasFlag(DamageTarget.Objects)) return;

            ShowDamage(obj, args.Damage);
        }

        private static void OnPlayerDamage(IPlayer player, PlayerDamageArgs args)
        {
            if (!Target.HasFlag(DamageTarget.Players)) return;

            ShowDamage(player, args.Damage);
        }

        private static void ShowDamage(IObject target, float damage)
        {
            if (damage < 1) return;

            float upper = target.GetMaxHealth();
            float t = upper > 0 ? MathHelper.Clamp(damage / upper, 0f, 1f) : 0f;

            Color color = ColorHelper.Lerp(Color.White, Color.Red, t);
            float scale = MathHelper.Lerp(1f, 2f, t);
            float duration = MathHelper.Lerp(500f, 2000f, t);

            PointShape.Random(v =>
            {
                EffectsWrapper.PlayCustomFloatText(v, $"-{(int)damage}", color, duration, scale);
            }, target.GetAABB(), Random.Shared);
        }
    }
}
