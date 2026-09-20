using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private static void ColorCommand(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 2)
            {
                Game.ShowChatMessage("Usage: /color <player> <color>", Color.Red, args.User.UserIdentifier);
                return;
            }

            string package = ColorHelper.ToColorPackage(tokens[1]);

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, args.User.UserIdentifier);
                return;
            }

            int affected = 0;

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                player.SetProfile(ColorProfile(player.GetProfile(), package));
                affected++;
            }

            if (affected == 0) return;

            Game.ShowChatMessage($"Applied {package} to {affected} player(s).", Color.Green, args.User.UserIdentifier);
        }

        /// <summary>
        /// Recolors every clothing item on a profile, keeping item names.
        /// Empty slots stay empty and Skin is left untouched.
        /// </summary>
        private static IProfile ColorProfile(IProfile profile, string package)
        {
            if (profile.Accessory != null)
                profile.Accessory = new IProfileClothingItem(profile.Accessory.Name, package, package);

            if (profile.ChestOver != null)
                profile.ChestOver = new IProfileClothingItem(profile.ChestOver.Name, package, package);

            if (profile.ChestUnder != null)
                profile.ChestUnder = new IProfileClothingItem(profile.ChestUnder.Name, package, package);

            if (profile.Feet != null)
                profile.Feet = new IProfileClothingItem(profile.Feet.Name, package, package);

            if (profile.Hands != null)
                profile.Hands = new IProfileClothingItem(profile.Hands.Name, package, package);

            if (profile.Head != null)
                profile.Head = new IProfileClothingItem(profile.Head.Name, package, package);

            if (profile.Legs != null)
                profile.Legs = new IProfileClothingItem(profile.Legs.Name, package, package);

            if (profile.Waist != null)
                profile.Waist = new IProfileClothingItem(profile.Waist.Name, package, package);

            return profile;
        }
    }
}
