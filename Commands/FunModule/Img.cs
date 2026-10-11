using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private const string IMG_STORAGE = "imgdump";
        private const string IMG_KEY_PREFIX = "Image.";

        private static void Img(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            IScriptStorage storage = Game.GetSharedStorage(IMG_STORAGE);

            if (storage == null)
            {
                Game.ShowChatMessage("No image data found.", Color.Red, uid);
                return;
            }

            int count = CountImages(storage);

            if (tokens.Length < 1 || tokens.Length > 2)
            {
                ShowImgUsage(uid, count);
                return;
            }

            if (!int.TryParse(tokens[0], out int index) || index < 0 || index >= count)
            {
                ShowImgUsage(uid, count);
                return;
            }

            string objectId = "BgText";

            if (tokens.Length > 1)
            {
                if (string.Equals(tokens[1], "bg", StringComparison.OrdinalIgnoreCase))
                    objectId = "BgText";
                else if (string.Equals(tokens[1], "fg", StringComparison.OrdinalIgnoreCase))
                    objectId = "Text";
                else
                {
                    ShowImgUsage(uid, count);
                    return;
                }
            }

            if (!storage.TryGetItemString(IMG_KEY_PREFIX + index, out string data) || string.IsNullOrEmpty(data))
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

        private static int CountImages(IScriptStorage storage)
        {
            int count = 0;

            while (storage.TryGetItemString(IMG_KEY_PREFIX + count, out string data) && !string.IsNullOrEmpty(data))
                count++;

            return count;
        }

        private static void ShowImgUsage(int uid, int count)
        {
            string range = count > 0 ? $" (0-{count - 1})" : string.Empty;

            Game.ShowChatMessage($"Usage: /img <index> [bg|fg]{range}", Color.Red, uid);
        }
    }
}
