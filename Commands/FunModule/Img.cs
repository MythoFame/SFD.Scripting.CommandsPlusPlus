using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private const string IMG_STORAGE = "imgdump";
        private const string IMG_KEY = "Image";

        private static void Img(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length > 1)
            {
                Game.ShowChatMessage("Usage: /img [bg|fg]", Color.Red, uid);
                return;
            }

            string objectId;

            if (tokens.Length == 0 || string.Equals(tokens[0], "bg", StringComparison.OrdinalIgnoreCase))
                objectId = "BgText";
            else if (string.Equals(tokens[0], "fg", StringComparison.OrdinalIgnoreCase))
                objectId = "Text";
            else
            {
                Game.ShowChatMessage("Usage: /img [bg|fg]", Color.Red, uid);
                return;
            }

            IScriptStorage storage = Game.GetSharedStorage(IMG_STORAGE);

            if (storage == null || !storage.TryGetItemString(IMG_KEY, out string data) || string.IsNullOrEmpty(data))
            {
                Game.ShowChatMessage("No image data found.", Color.Red, uid);
                return;
            }

            IPlayer self = args.User?.GetPlayer();

            if (self == null || self.IsRemoved)
            {
                Game.ShowChatMessage("Your character must be active!", Color.Red, uid);
                return;
            }

            ImageHelper.DecodedImage[] frames = ImageHelper.GetFrames(data);

            if (frames == null)
            {
                Game.ShowChatMessage("Image data is invalid.", Color.Red, uid);
                return;
            }

            int total = ImageHelper.Render(frames, self.GetWorldPosition(), objectId, uid);

            Game.ShowChatMessage($"Rendering image ({total} pixels).", Color.Green, uid);
        }
    }
}
