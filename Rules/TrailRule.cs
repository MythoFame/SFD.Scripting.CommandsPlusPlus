using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Leaves effect trails behind players. The rule itself is always
    /// enabled; it only returns early when the Fun module is restricted.
    /// </summary>
    public static class TrailRule
    {
        private const uint DELAY = 50;

        /// <summary>Below this linear speed no trail effect is played.</summary>
        private const float MIN_SPEED = 1;

        /// <summary>At or above this linear speed the trail effect plays every tick.</summary>
        private const float FULL_SPEED = 15;

        private sealed record TrailEntry(IUser User, string Effect);

        private static TrailEntry[] _trails = [];
        /// <summary>
        /// Saves a user's trail effect.
        /// </summary>

        public static void SetTrail(string accountName, string effect) =>
            Game.LocalStorage.SetItem(GetKey(accountName), effect);

        /// <summary>
        /// Reads a user's saved trail effect, or null when they have none.
        /// </summary>
        public static string GetTrail(string accountName) =>
            Game.LocalStorage.GetItem(GetKey(accountName)) as string;

        /// <summary>
        /// Starts the update hook. Call once when the world is live.
        /// </summary>
        public static void Initialize()
        {
            if (ModuleRegistry.TryGet("Fun", out CommandsModule module) && module.IsRestricted)
                return;

            List<TrailEntry> trails = [];

            foreach (IUser user in Game.GetActiveUsers())
            {
                if (user.IsBot) continue;

                string effect = GetTrail(user.AccountName);

                if (string.IsNullOrEmpty(effect)) continue;

                trails.Add(new(user, effect));
            }

            _trails = [.. trails];

            Game.Events.StartUpdateCallback(OnUpdate, DELAY);
        }

        private static void OnUpdate(float dlt)
        {
            foreach (TrailEntry trail in _trails)
            {
                IPlayer player = trail.User.GetPlayer();

                if (player == null || player.IsRemoved) continue;

                float speed = player.GetLinearVelocity().Length();

                if (speed < MIN_SPEED) continue;

                float chance = Math.Min((speed - MIN_SPEED) / (FULL_SPEED - MIN_SPEED), 1f);

                if (_random.NextSingle() >= chance) continue;

                Game.PlayEffect(trail.Effect, player.GetWorldPosition());
            }
        }

        /// <summary>
        /// Builds the storage key holding a user's trail effect.
        /// </summary>
        private static string GetKey(string accountName) => $"CommandsPlusPlus.Player.{accountName}.Trail";
    }
}
