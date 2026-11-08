using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Prevents throwing by untracking missiles. While throwing is
    /// suppressed, every created object gets TrackAsMissile(false), so
    /// thrown objects never become live missiles.
    /// </summary>
    public static class ThrowRule
    {
        private const string SUPPRESSED_KEY = "CommandsPlusPlus.Rule.Throw.Suppressed";

        private static Events.ObjectCreatedCallback _objectCreated = null;

        /// <summary>
        /// Gets or sets whether throwing is allowed. Backed directly by
        /// storage, so there is no shadow state to drift. Setting it starts
        /// or stops the object hook exactly once per transition.
        /// </summary>
        public static bool IsAllowed
        {
            get => !(Game.LocalStorage.GetItem(SUPPRESSED_KEY) as bool? ?? false);
            set
            {
                bool suppressed = !value;

                if ((Game.LocalStorage.GetItem(SUPPRESSED_KEY) as bool? ?? false) == suppressed) return;

                Game.LocalStorage.SetItem(SUPPRESSED_KEY, suppressed);
                OnSuppressed(suppressed);
            }
        }

        /// <summary>
        /// Restores the persisted state. Call once when the world is live.
        /// </summary>
        public static void Initialize()
        {
            if (Game.LocalStorage.GetItem(SUPPRESSED_KEY) as bool? ?? false)
                OnSuppressed(true);
        }

        private static void OnSuppressed(bool suppressed)
        {
            if (suppressed)
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
