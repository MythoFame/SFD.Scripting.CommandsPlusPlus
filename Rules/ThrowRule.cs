using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Prevents throwing by untracking missiles. While enabled, every created
    /// object gets TrackAsMissile(false), so thrown objects never become
    /// live missiles.
    /// </summary>
    public static class ThrowRule
    {
        private const string ENABLED_KEY = "CommandsPlusPlus.Rule.Throw.Enabled";

        private static Events.ObjectCreatedCallback _objectCreated = null;

        /// <summary>
        /// Gets or sets whether the rule is active. Backed directly by storage,
        /// so there is no shadow state to drift. Setting it starts or stops
        /// the object hook exactly once per transition.
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
                _objectCreated = Game.Events.StartObjectCreatedCallback(OnObjectCreated);
            }
            else
            {
                if (_objectCreated == null) return;

                _objectCreated.Stop();

                _objectCreated = null;
            }
        }

        private static void OnObjectCreated(IObject[] objs)
        {
            foreach (IObject obj in objs)
            {
                if (obj == null || obj.IsRemoved) continue;

                obj.TrackAsMissile(false);
            }
        }
    }
}
