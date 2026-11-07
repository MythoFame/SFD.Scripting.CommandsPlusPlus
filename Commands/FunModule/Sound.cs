using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private static void Sound(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 1)
            {
                Game.ShowChatMessage("Usage: /sound <sound_id>", Color.Red, uid);
                return;
            }

            string soundID = SoundHelper.ResolveSoundName(tokens[0], out string[] suggestions);

            if (soundID == null)
            {
                Game.ShowChatMessage($"Unknown sound '{tokens[0]}'.", Color.Red, uid);

                if (suggestions.Length > 0)
                    Game.ShowChatMessage(SoundHelper.DidYouMean(suggestions), Color.Red, uid);

                return;
            }

            Game.PlaySound(soundID, Vector2.Zero);

            Game.ShowChatMessage($"Played sound '{soundID}'.", Color.Green, uid);
        }
    }
}
