using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Represents a base ability that can be activated and updated over time.
    /// </summary>
    public abstract class Ability
    {
        // Interval for the main update callback event
        private const uint COOLDOWN = 0;

        // Main update callback event
        private Events.UpdateCallback _updateCallback = null;

        // The player associated with this ability
        public IPlayer Player;

        /// <summary>
        /// Gets or sets whether the ability is enabled.
        /// </summary>
        public bool Enabled
        {
            get => _updateCallback != null;
            set
            {
                if (value == Enabled) return;

                if (value)
                {
                    _updateCallback = Game.Events.StartUpdateCallback(Update, COOLDOWN);
                    OnEnabled();
                }
                else
                {
                    _updateCallback.Stop();
                    _updateCallback = null;
                    OnDisabled();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ability"/> class.
        /// </summary>
        /// <param name="player">The player associated with this ability.</param>
        public Ability(IPlayer player)
        {
            Player = player;
            Enabled = true;
        }

        /// <summary>
        /// Updates the power-up with the specified time delta.
        /// </summary>
        /// <param name="dlt">The time delta since the last update.</param>
        private void Update(float dlt)
        {
            // Check if the player is still valid
            if (Player == null || Player.IsRemoved || Player.IsDead)
            {
                Enabled = false;
                return;
            }

            // Invoke the virtual Update method
            Update(dlt, dlt / 1000);
        }

        /// <summary>
        /// Abstract method for updating the power-up.
        /// </summary>
        /// <param name="dlt">The time delta since the last update.</param>
        /// <param name="dltSecs">The time delta in seconds since the last
        /// update.</param>
        public abstract void Update(float dlt, float dltSecs);

        /// <summary>
        /// Abstract method called when the power-up is enabled or disabled. Called by
        /// the constructor.
        /// </summary>
        public abstract void OnEnabled();

        public abstract void OnDisabled();
    }
}
