using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class GameplayModule
    {
        private static void Camera(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length == 0 || tokens.Length > 2)
            {
                Game.ShowChatMessage("Usage: /camera {static|dynamic|individual} [zoom]", Color.Red, uid);
                return;
            }

            if (!Enum.TryParse(tokens[0], true, out CameraMode mode) || !Enum.IsDefined(mode))
            {
                Game.ShowChatMessage("Usage: /camera {static|dynamic|individual} [zoom]", Color.Red, uid);
                return;
            }

            float zoom = 0f;

            if (tokens.Length > 1)
            {
                if (mode != CameraMode.Individual)
                {
                    Game.ShowChatMessage("Zoom only applies to the individual camera.", Color.Red, uid);
                    return;
                }

                if (!float.TryParse(tokens[1], out zoom) || zoom <= 0)
                {
                    Game.ShowChatMessage($"Invalid zoom '{tokens[1]}'.", Color.Red, uid);
                    return;
                }
            }

            Game.SetCurrentCameraMode(mode);

            if (tokens.Length > 1)
            {
                Game.SetCameraFixedIndividualZoom(zoom);
                Game.ShowChatMessage($"Camera set to individual with zoom {zoom}.", Color.Green, uid);
            }
            else
            {
                Game.ShowChatMessage($"Camera set to {mode.ToString().ToLowerInvariant()}.", Color.Green, uid);
            }
        }
    }
}
