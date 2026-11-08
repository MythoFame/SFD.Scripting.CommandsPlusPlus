using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Prevents grabbing by dragging grabbers down. While grabbing is
    /// suppressed, every update offsets the vertical position of grabbing
    /// players by -1, which breaks their hold.
    /// </summary>
    public static class GrabRule
    {
        private const string SUPPRESSED_KEY = "CommandsPlusPlus.Rule.Grab.Suppressed";

        private static Events.UpdateCallback _update = null;

        /// <summary>
        /// Gets or sets whether grabbing is allowed. Backed directly by
        /// storage, so there is no shadow state to drift. Setting it starts
        /// or stops the update hook exactly once per transition.
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
