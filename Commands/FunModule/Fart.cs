using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private const float FART_STEAM_SIZE = 0.5f;

        private static void Fart(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 0)
            {
                Game.ShowChatMessage("Usage: /fart", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer self = args.User.GetPlayer();

            if (self == null || self.IsRemoved)
            {
                Game.ShowChatMessage("Your character must be active!", Color.Red, args.User.UserIdentifier);
                return;
            }

            Vector2 pos = self.GetWorldPosition();

            EffectsWrapper.PlaySteam(pos, Color.Green, FART_STEAM_SIZE);
            EffectsWrapper.PlaySteam(pos, Color.Green, FART_STEAM_SIZE);

            Game.PlaySound(SoundsDatabase.BalloonPop, pos);

            Game.ShowChatMessage("You farted.", Color.Green, args.User.UserIdentifier);
        }
    }
}
