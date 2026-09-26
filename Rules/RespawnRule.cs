using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Respawns dead players after a delay while enabled, instead of making
    /// them wait for the next round.
    /// </summary>
    public static class RespawnRule
    {
        private const string DELAY_KEY = "CommandsPlusPlus.Rule.Respawn.Delay";

        private static Events.PlayerDeathCallback _playerDeath = null;

        /// <summary>
        /// Spawn delay in milliseconds. Backed directly by storage, defaults
        /// to zero (disabled). Setting it starts or stops the death hook
        /// exactly once per enabled transition.
        /// </summary>
        public static uint Delay
        {
            get => (uint)(Game.LocalStorage.GetItem(DELAY_KEY) as int? ?? 0);
            set
            {
                bool wasEnabled = IsEnabled;
                Game.LocalStorage.SetItem(DELAY_KEY, (int)value);

                if (IsEnabled != wasEnabled)
                    OnEnabled(IsEnabled);
            }
        }

        /// <summary>
        /// Whether the rule is active. Equivalent to a positive delay.
        /// </summary>
        public static bool IsEnabled => Delay > 0;

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
                _playerDeath = Game.Events.StartPlayerDeathCallback(OnPlayerDeath);
            }
            else
            {
                if (_playerDeath == null) return;

                _playerDeath.Stop();

                _playerDeath = null;
            }
        }

        private static void OnPlayerDeath(IPlayer player, PlayerDeathArgs args)
        {
            if (player == null || player.IsRemoved) return;

            IUser user = player.GetUser();

            if (user == null) return;

            PlayerHelper.Respawn(user, PathGridHelper.GetRandomSpawnPosition(Random.Shared), Delay);
        }
    }
}
