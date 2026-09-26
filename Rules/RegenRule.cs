using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Regenerates health for all players while enabled. The stored regen
    /// amount is the only state: zero or below means disabled.
    /// </summary>
    public static class RegenRule
    {
        private const string AMOUNT_KEY = "CommandsPlusPlus.Rule.Regen.Amount";

        private static Events.UpdateCallback _update = null;

        /// <summary>
        /// Health regenerated per second. Backed directly by storage.
        /// Setting it starts or stops the update hook exactly once per
        /// enabled transition.
        /// </summary>
        public static float Amount
        {
            get => Game.LocalStorage.GetItem(AMOUNT_KEY) as float? ?? 0;
            set
            {
                bool wasEnabled = IsEnabled;
                Game.LocalStorage.SetItem(AMOUNT_KEY, value);

                if (IsEnabled != wasEnabled)
                    OnEnabled(IsEnabled);
            }
        }

        /// <summary>
        /// Whether the rule is active. Equivalent to a positive amount.
        /// </summary>
        public static bool IsEnabled => Amount > 0;

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
                _update = Game.Events.StartUpdateCallback(OnUpdate);
            }
            else
            {
                if (_update == null) return;

                _update.Stop();

                _update = null;
            }
        }

        private static void OnUpdate(float dlt)
        {
            float delta = Amount * (dlt / 1000f);

            foreach (IPlayer player in Game.GetPlayers())
            {
                if (player.IsDead) continue;

                player.SetHealth(player.GetHealth() + delta);
            }
        }
    }
}
