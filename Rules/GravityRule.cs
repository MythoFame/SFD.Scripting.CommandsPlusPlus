using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Shifts gravity while enabled. Ticks every update, adding a constant
    /// to falling players' vertical velocity.
    /// </summary>
    public static class GravityRule
    {
        private const string CONSTANT_KEY = "CommandsPlusPlus.Rule.Gravity.Constant";

        private static Events.UpdateCallback _update = null;

        /// <summary>
        /// Constant applied to gravity. Backed directly by storage, defaults
        /// to zero (disabled). Setting it starts or stops the update hook
        /// exactly once per enabled transition.
        /// </summary>
        public static float Constant
        {
            get => Game.LocalStorage.GetItem(CONSTANT_KEY) as float? ?? 0f;
            set
            {
                bool wasEnabled = IsEnabled;
                Game.LocalStorage.SetItem(CONSTANT_KEY, value);

                if (IsEnabled != wasEnabled)
                    OnEnabled(IsEnabled);
            }
        }

        /// <summary>
        /// Whether the rule is active. Equivalent to a non-zero constant.
        /// </summary>
        public static bool IsEnabled => Constant != 0f;

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
            foreach(IPlayer player in Game.GetPlayers())
            {
                if (player.IsLedgeGrabbing || player.IsClimbing ||
                    !player.IsInMidAir) continue;

                Vector2 impulse = new(0, player.GetLinearVelocity().Y + Constant);

                impulse.X += player.KeyPressed(VirtualKey.AIM_RUN_RIGHT) ? 1 :
                (player.KeyPressed(VirtualKey.AIM_RUN_LEFT) ? -1 : 0);

                impulse.X *= player.KeyPressed(VirtualKey.SPRINT) ? 2 :
                (player.KeyPressed(VirtualKey.WALKING) ? 0.5f : 1);

                player.SetLinearVelocity(impulse);
            }
        }
    }
}
