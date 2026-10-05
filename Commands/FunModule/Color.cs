using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class FunModule
    {
        private static readonly string[] _colorSlots =
        [
            "Accessory", "ChestOver", "ChestUnder", "Feet",
            "Hands", "Head", "Legs", "Waist"
        ];

        private static void ColorCommand(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

            if (tokens.Length != 2)
            {
                Game.ShowChatMessage("Usage: /color <player> <color>", Color.Red, uid);
                return;
            }

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
                return;
            }

            int affected = 0;
            string[] suggestions = [];

            foreach (IPlayer player in players)
            {
                if (player == null || player.IsRemoved) continue;

                IProfile profile = player.GetProfile();

                if (!RecolorProfile(profile, tokens[1], ref suggestions))
                    continue;

                player.SetProfile(profile);
                affected++;
            }

            if (affected == 0)
            {
                Game.ShowChatMessage($"Unknown color '{tokens[1]}'.", Color.Red, uid);

                if (suggestions.Length > 0)
                    Game.ShowChatMessage(ClothingHelper.DidYouMean(suggestions), Color.Red, uid);

                return;
            }

            Game.ShowChatMessage($"Applied '{tokens[1]}' to {affected} player(s).", Color.Green, uid);
        }

        /// <summary>
        /// Recolors every clothing item on a profile, keeping item names.
        /// Each item resolves the color against its own palette, falling back
        /// to the secondary list when the primary list has no match. Items
        /// with no match keep their colors. Empty slots stay empty and Skin
        /// is left untouched. Returns whether any item changed.
        /// </summary>
        private static bool RecolorProfile(IProfile profile, string input, ref string[] suggestions)
        {
            bool changed = false;

            foreach (string slot in _colorSlots)
            {
                var field = typeof(IProfile).GetField(slot,
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (field == null || field.FieldType != typeof(IProfileClothingItem)) continue;

                if (field.GetValue(profile) is not IProfileClothingItem item) continue;

                string package = ClothingHelper.ResolveColorPackage(
                    ClothingHelper.GetColorPackages(item.Name, false), input, out string[] primarySuggestions);

                if (suggestions.Length == 0)
                    suggestions = primarySuggestions;

                package ??= ClothingHelper.ResolveColorPackage(
                    ClothingHelper.GetColorPackages(item.Name, true), input, out _);

                if (package == null) continue;

                field.SetValue(profile, new IProfileClothingItem(item.Name, package, package));
                changed = true;
            }

            return changed;
        }
    }
}
