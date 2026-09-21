using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Controls who spectates and who plays. Pure stored data for now;
    /// the enforcement itself is not implemented yet.
    /// </summary>
    public static class SpectationRule
    {
        private const string WHITELIST_KEY = "CommandsPlusPlus.Rule.Spectation.WhitelistOnly";
        private const string SPECTATING_KEY = "CommandsPlusPlus.Rule.Spectation.Spectating";
        private const string WHITELIST_NAMES_KEY = "CommandsPlusPlus.Rule.Spectation.Whitelist";

        /// <summary>
        /// Runs rule startup. The rule itself is always enabled; it only
        /// returns early when the Spectation module is restricted. Call once
        /// when the world is live.
        /// </summary>
        public static void Initialize()
        {
            if (ModuleRegistry.TryGet("Spectation", out CommandsModule module) && module.IsRestricted)
                return;

            if (Game.DeathSequenceEnabled)
            {
                Game.DeathSequenceEnabled = false;

                Game.Events.StartUpdateCallback(_ =>
                {
                    Game.DeathSequenceEnabled = true;
                }, 500, 1);
            }

            foreach(IUser user in Game.GetActiveUsers())
            {
                if (user.IsBot || !Spectating.Contains(user.AccountName)) continue;

                ForceSpectate(user, "You're currently spectating. Type /spectate to stop spectating.");
            }

            if (WhitelistOnly)
            {
                foreach(IUser user in Game.GetActiveUsers())
                {
                    if (user.IsBot || Whitelist.Contains(user.AccountName)) continue;

                    ForceSpectate(user, "Only whitelisted players may play.");
                }
            }
        }

        private static void ForceSpectate(IUser user, string message)
        {
            IPlayer player = user.GetPlayer();

            if (player == null || player.IsRemoved) return;

            player.SetUser(null);

            player.Remove();

            Game.ShowChatMessage(message, Color.Yellow, user.UserIdentifier);
        }

        /// <summary>
        /// Whether only whitelisted players may play while everyone else
        /// spectates. Backed directly by storage, defaults to false.
        /// </summary>
        public static bool WhitelistOnly
        {
            get => Game.LocalStorage.GetItem(WHITELIST_KEY) as bool? ?? false;
            set
            {
                if (WhitelistOnly == value) return;

                Game.LocalStorage.SetItem(WHITELIST_KEY, value);
            }
        }

        /// <summary>
        /// Account names of spectating players. Backed directly by storage,
        /// defaults to empty.
        /// </summary>
        public static string[] Spectating
        {
            get => Game.LocalStorage.TryGetItemStringArr(SPECTATING_KEY, out string[] result) && result != null
                ? result
                : [];
            set => Game.LocalStorage.SetItem(SPECTATING_KEY, value ?? []);
        }

        /// <summary>
        /// Account names of whitelisted players. Backed directly by storage,
        /// defaults to empty.
        /// </summary>
        public static string[] Whitelist
        {
            get => Game.LocalStorage.TryGetItemStringArr(WHITELIST_NAMES_KEY, out string[] result) && result != null
                ? result
                : [];
            set => Game.LocalStorage.SetItem(WHITELIST_NAMES_KEY, value ?? []);
        }
    }
}
