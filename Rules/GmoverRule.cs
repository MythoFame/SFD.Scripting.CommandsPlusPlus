using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Restricts game over to players while enabled, ignoring bots. The
    /// detection itself is not implemented yet.
    /// </summary>
    public static class GmoverRule
    {
        private const string ENABLED_KEY = "CommandsPlusPlus.Rule.Gmover.Enabled";
        private const uint COOLDOWN = 500;

        private static Events.UpdateCallback _update = null;

        /// <summary>
        /// Gets or sets whether the rule is active. Backed directly by storage,
        /// so there is no shadow state to drift.
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
            if (IsEnabled && Game.AutoVictoryConditionEnabled)
                OnEnabled(true);
        }

        private static void OnEnabled(bool enabled)
        {
            if (enabled)
            {
                _update = Game.Events.StartUpdateCallback(OnUpdate, COOLDOWN);
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
            foreach (IPlayer player in Game.GetPlayers())
            {
                if (!player.IsBot && !player.IsDead && !player.IsRemoved)
                    return;
            }

            Game.SetGameOver();
        }
    }
}
