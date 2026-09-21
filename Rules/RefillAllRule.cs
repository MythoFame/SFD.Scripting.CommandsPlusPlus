using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Constantly refills ammo for all players while enabled. Hooks projectile
    /// creation: every shot tops up everyone's primary, secondary and thrown
    /// ammo, which amounts to infinite ammo.
    /// </summary>
    public static class RefillAllRule
    {
        private const string ENABLED_KEY = "CommandsPlusPlus.Rule.RefillAll.Enabled";

        private static Events.ProjectileCreatedCallback _projectileCreated = null;

        /// <summary>
        /// Gets or sets whether the rule is active. Backed directly by storage,
        /// so there is no shadow state to drift. Setting it starts or stops
        /// the projectile hook exactly once per transition.
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
                _projectileCreated = Game.Events.StartProjectileCreatedCallback(OnProjectileCreated);
            }
            else
            {
                if (_projectileCreated == null) return;

                _projectileCreated.Stop();

                _projectileCreated = null;
            }
        }

        private static void OnProjectileCreated(IProjectile[] projs)
        {
            foreach (IPlayer player in Game.GetPlayers())
            {
                player.SetCurrentPrimaryWeaponAmmo(int.MaxValue);
                player.SetCurrentSecondaryWeaponAmmo(int.MaxValue);
                player.SetCurrentThrownItemAmmo(int.MaxValue);
            }
        }
    }
}
