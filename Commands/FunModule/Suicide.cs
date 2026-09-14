using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private const float SUICIDE_EFFECT_TIME = 2;
        private const float SUICIDE_Y_VEL = 8;
        private const float SUICIDE_ANG_VEL = 60;

        private static void Suicide(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 0)
            {
                Game.ShowChatMessage("Usage: /suicide", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer self = args.User.GetPlayer();

            if (self == null || self.IsRemoved)
            {
                Game.ShowChatMessage("Your character must be active!", Color.Red, args.User.UserIdentifier);
                return;
            }

            if (!self.IsInputEnabled)
            {
                Game.ShowChatMessage("Your input must be enabled!", Color.Red, args.User.UserIdentifier);
                return;
            }

            self.Kill();

            EffectsWrapper.PlayTraceSpawner(self, EffectName.DustTrail, SUICIDE_EFFECT_TIME);

            self.Fall();

            Vector2 vel = self.GetLinearVelocity();

            vel.Y = SUICIDE_Y_VEL;

            self.SetLinearVelocity(vel);
            self.SetAngularVelocity(SUICIDE_ANG_VEL);

            IProfile profile = self.GetProfile();

            Game.PlaySound(
                profile.Gender == Gender.Male ? SoundsDatabase.Wilhelm : SoundsDatabase.CartoonScream,
                Vector2.Zero);
        }
    }
}
