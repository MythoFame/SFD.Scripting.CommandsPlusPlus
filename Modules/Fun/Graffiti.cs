using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Utils.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Fun;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private const int GRAFFITI_TEXT_LIMIT = 40;
        private const int GRAFFITI_MAX_AMOUNT = 5;
        private const float GRAFFITI_MAX_SCALE = 2f;
        private const float GRAFFITI_MIN_SCALE = 0.25f;
        private const float GRAFFITI_MAX_TILT = 0.2f;
        private const float GRAFFITI_STEAM_SCALE = 2;
        private const string GRAFFITI_ID = "CPP.GRAFFITI";

        private static readonly Vector2 _offset = new(0, 13);

        private static void Graffiti(UserMessageCallbackArgs args)
        {
            int uid = args.User?.UserIdentifier ?? -1;
            string text = args.CommandArguments.Trim();

            if (text.Length == 0)
            {
                Game.ShowChatMessage("Usage: /graffiti <text>", Color.Red, uid);
                return;
            }

            if (text.Length > GRAFFITI_TEXT_LIMIT)
            {
                Game.ShowChatMessage($"Text cannot have more than {GRAFFITI_TEXT_LIMIT} characters!", Color.Red, uid);
                return;
            }

            IPlayer self = args.User.GetPlayer();

            if (self == null || self.IsRemoved)
            {
                Game.ShowChatMessage("Your character must be active!", Color.Red, uid);
                return;
            }

            Vector2 graffitiPos = self.GetWorldPosition() + _offset;

            IObjectText graffiti = (IObjectText)Game.CreateObject("BgText", graffitiPos);

            graffiti.SetText(text);
            graffiti.SetTextAlignment(TextAlignment.Middle);

            Color color = ColorHelper.GetTeamColor(self.GetTeam());

            graffiti.SetTextColor(color);
            graffiti.SetAngle((Random.Shared.NextSingle() * 2 - 1) * GRAFFITI_MAX_TILT);
            graffiti.SetTextScale(GRAFFITI_MAX_SCALE - text.Length / (float)GRAFFITI_TEXT_LIMIT * (GRAFFITI_MAX_SCALE - GRAFFITI_MIN_SCALE));
            graffiti.CustomID = GRAFFITI_ID;

            EffectsWrapper.PlaySteam(graffitiPos, color, GRAFFITI_STEAM_SCALE);
            EffectsWrapper.PlaySteam(graffitiPos, color, GRAFFITI_STEAM_SCALE);
            EffectsWrapper.PlaySteam(graffitiPos, color, GRAFFITI_STEAM_SCALE);

            Game.ShowChatMessage($"Created graffiti '{text}'.", Color.Green, uid);

            IObject[] graffitis = Game.GetObjectsByCustomID<IObjectText>(GRAFFITI_ID);

            if (graffitis.Length > GRAFFITI_MAX_AMOUNT)
            {
                graffitis.MinBy(graffiti => graffiti.UniqueID).Remove();
            }
        }
    }
}
