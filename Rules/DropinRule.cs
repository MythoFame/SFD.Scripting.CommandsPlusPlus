using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Spawns joining players after a delay while enabled, instead of making
    /// them wait for the next round.
    /// </summary>
    public static class DropinRule
    {
        private const string DELAY_KEY = "CommandsPlusPlus.Rule.Dropin.Delay";

        private static Events.UserJoinCallback _userJoin = null;

        /// <summary>
        /// Spawn delay for joining players. Backed directly by storage,
        /// defaults to zero (disabled). Setting it starts or stops the join
        /// hook exactly once per enabled transition.
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
                _userJoin = Game.Events.StartUserJoinCallback(OnUserJoin);
            }
            else
            {
                if (_userJoin == null) return;

                _userJoin.Stop();

                _userJoin = null;
            }
        }

        private static void OnUserJoin(IUser[] users)
        {
            foreach (IUser user in users)
            {
                if (user == null) continue;

                PlayerHelper.Respawn(user, PathGridHelper.GetRandomSpawnPosition(Random.Shared), Delay);
            }
        }
    }
}
