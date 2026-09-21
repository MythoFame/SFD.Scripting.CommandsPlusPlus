using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Prevents grabbing by dragging grabbers down. While enabled, every
    /// update offsets the vertical position of grabbing players by -1,
    /// which breaks their hold.
    /// </summary>
    public static class GrabRule
    {
        private const string ENABLED_KEY = "CommandsPlusPlus.Rule.Grab.Enabled";

        private static Events.UpdateCallback _update = null;

        /// <summary>
        /// Gets or sets whether the rule is active. Backed directly by storage,
        /// so there is no shadow state to drift. Setting it starts or stops
        /// the update hook exactly once per transition.
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
            foreach (IPlayer player in Game.GetPlayers())
            {
                if (!player.IsGrabbing) continue;

                PlayerHelper.Unstick(player);
            }
        }
    }
}
