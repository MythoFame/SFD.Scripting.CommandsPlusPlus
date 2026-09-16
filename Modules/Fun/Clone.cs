using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Utils.GameScript;

namespace SFD.Scripting.CommandsPlusPlus.Modules.Fun;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private static void Clone(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length < 1 || tokens.Length > 3)
            {
                Game.ShowChatMessage("Usage: /clone <player> [team] [ai]", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer[] sources = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (sources.Length != 1)
            {
                Game.ShowChatMessage("Target a single player.", Color.Red, args.User.UserIdentifier);
                return;
            }

            IPlayer source = sources[0];

            if (source == null || source.IsRemoved)
            {
                Game.ShowChatMessage("Target a single live player.", Color.Red, args.User.UserIdentifier);
                return;
            }

            PlayerTeam team = source.GetTeam();

            if (tokens.Length >= 2)
            {
                if (!Enum.TryParse(tokens[1], true, out team) || !Enum.IsDefined(team))
                {
                    Game.ShowChatMessage("Invalid team. Use independent (or 0) or team 1-8.", Color.Red, args.User.UserIdentifier);
                    return;
                }
            }

            PredefinedAIType ai = PredefinedAIType.BotA;

            if (source.IsBot && source.GetBotBehavior() is BotBehavior behavior)
                ai = behavior.PredefinedAI;

            if (tokens.Length >= 3)
            {
                if (!Enum.TryParse(tokens[2], true, out ai) || !Enum.IsDefined(ai))
                {
                    Game.ShowChatMessage("Invalid AI type.", Color.Red, args.User.UserIdentifier);
                    return;
                }
            }

            IPlayer clone = Game.CreatePlayer(source.GetWorldPosition());

            clone.SetBotBehavior(new(true, ai));
            clone.SetBotName(source.Name);
            clone.SetProfile(source.GetProfile());
            clone.SetTeam(team);

            if (source.IsFalling)
                clone.Fall();

            clone.SetLinearVelocity(source.GetLinearVelocity());
            clone.SetAngularVelocity(source.GetAngularVelocity());

            clone.GiveWeaponItem(source.CurrentMeleeWeapon.WeaponItem);
            clone.SetCurrentMeleeDurability(source.CurrentMeleeWeapon.Durability);

            clone.GiveWeaponItem(source.CurrentSecondaryWeapon.WeaponItem);
            clone.SetCurrentSecondaryWeaponAmmo(source.CurrentSecondaryWeapon.TotalAmmo);

            clone.GiveWeaponItem(source.CurrentPrimaryWeapon.WeaponItem);
            clone.SetCurrentPrimaryWeaponAmmo(source.CurrentPrimaryWeapon.TotalAmmo);

            clone.GiveWeaponItem(source.CurrentThrownItem.WeaponItem);
            clone.SetCurrentThrownItemAmmo(source.CurrentThrownItem.CurrentAmmo);

            clone.GiveWeaponItem(source.CurrentPowerupItem.WeaponItem);

            PlayerHelper.Unstick(clone);

            Game.ShowChatMessage($"Cloned {source.Name} (team {team}, AI {ai}).", Color.Green, args.User.UserIdentifier);
        }
    }
}
