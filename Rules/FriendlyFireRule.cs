using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Allows friendly fire while enabled. Hooks player damage; the
    /// team check itself is not implemented yet.
    /// </summary>
    public static class FriendlyFireRule
    {
        private const string ENABLED_KEY = "CommandsPlusPlus.Rule.FriendlyFire.Enabled";

        private static Events.PlayerDamageCallback _playerDamage = null;

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

        private static void OnEnabled(bool enabled)
        {
            if (enabled)
            {
                _playerDamage = Game.Events.StartPlayerDamageCallback(OnPlayerDamage);
            }
            else
            {
                if (_playerDamage == null) return;

                _playerDamage.Stop();

                _playerDamage = null;
            }
        }

        private static void OnPlayerDamage(IPlayer player, PlayerDamageArgs args)
        {
            if (args.OverkillDamage || args.DamageType != PlayerDamageEventType.Projectile) return;

            IProjectile projectile = Game.GetProjectile(args.SourceID);

            IPlayer owner = Game.GetPlayer(projectile.InitialOwnerPlayerID);

            if (owner == null || PlayerHelper.IsEnemy(owner, player)) return;

            player.SetHealth(player.GetHealth() + args.Damage);
        }
    }
}
