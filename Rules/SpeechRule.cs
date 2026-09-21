using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Shows speech bubbles above players while enabled. Hooks user messages;
    /// the bubble display itself is not implemented yet.
    /// </summary>
    public static class SpeechRule
    {
        private const string ENABLED_KEY = "CommandsPlusPlus.Rule.Speech.Enabled";
        private const string SOUND_KEY = "CommandsPlusPlus.Rule.Speech.PlaySound";

        private static Events.UserMessageCallback _userMessage = null;

        /// <summary>
        /// Gets or sets whether the rule is active. Backed directly by storage,
        /// so there is no shadow state to drift. Setting it starts or stops
        /// the message hook exactly once per transition.
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
        /// Whether a sound plays when speech appears. Backed directly by
        /// storage, defaults to false.
        /// </summary>
        public static bool PlaySound
        {
            get => Game.LocalStorage.GetItem(SOUND_KEY) as bool? ?? false;
            set
            {
                if (PlaySound == value) return;

                Game.LocalStorage.SetItem(SOUND_KEY, value);
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
                _userMessage = Game.Events.StartUserMessageCallback(OnUserMessage);
            }
            else
            {
                if (_userMessage == null) return;

                Game.Events.Stop(_userMessage);

                _userMessage = null;
            }
        }

        private static void OnUserMessage(UserMessageCallbackArgs args)
        {
            if (args.IsCommand) return;

            IPlayer player = args.User.GetPlayer();

            if (player == null || player.IsRemoved) return;

            Game.CreateDialogue(args.Message, ColorHelper.GetTeamColor(player.GetTeam()),
                player, string.Empty, -1, false);

            if (PlaySound)
            {
                Game.PlaySound(SoundsDatabase.C4Arm, Vector2.Zero, 0.1f);
            }
        }
    }
}
