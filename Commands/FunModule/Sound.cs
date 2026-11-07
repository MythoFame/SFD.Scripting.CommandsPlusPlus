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

            string soundID = tokens[0];

            Game.PlaySound(soundID, Vector2.Zero);

            Game.ShowChatMessage($"Played sound '{soundID}'.", Color.Green, uid);
        }
    }
}
