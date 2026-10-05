using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Helpers for resolving clothing item names and color packages against
    /// the game's catalogs, with fuzzy matching.
    /// </summary>
    public static class ClothingHelper
    {
        private const int MAX_SUGGESTIONS = 5;

        /// <summary>
        /// Valid item names for a profile slot and gender.
        /// </summary>
        public static string[] GetSlotItemNames(string slot, Gender gender) => slot switch
        {
            "Skin" => Game.GetClothingItemNamesSkin(gender),
            "ChestUnder" => Game.GetClothingItemNamesChestUnder(gender),
            "Legs" => Game.GetClothingItemNamesLegs(gender),
            "Waist" => Game.GetClothingItemNamesWaist(gender),
            "Feet" => Game.GetClothingItemNamesFeet(gender),
            "ChestOver" => Game.GetClothingItemNamesChestOver(gender),
            "Accessory" => Game.GetClothingItemNamesAccessory(gender),
            "Hands" => Game.GetClothingItemNamesHands(gender),
            "Head" => Game.GetClothingItemNamesHead(gender),
            _ => [],
        };

        /// <summary>
        /// Fuzzy-matches an item name for a slot and gender. Returns null when
        /// nothing matches unambiguously; suggestions holds close candidates then.
        /// </summary>
        public static string ResolveItemName(string slot, Gender gender, string input, out string[] suggestions) =>
            Resolve(GetSlotItemNames(slot, gender), input, out suggestions);

        /// <summary>
        /// Color packages for an item's palette channel. Secondary selects the
        /// secondary list, otherwise the primary list.
        /// </summary>
        public static string[] GetColorPackages(string itemName, bool secondary)
        {
            string paletteName = Game.GetClothingItemColorPaletteName(itemName);

            if (string.IsNullOrEmpty(paletteName)) return [];

            ColorPalette palette = Game.GetColorPalette(paletteName);

            if (palette == null) return [];

            string[] packages = secondary ? palette.SecondaryColorPackages : palette.PrimaryColorPackages;

            return packages ?? [];
        }

        /// <summary>
        /// Fuzzy-matches a color package name. Returns null when nothing matches
        /// unambiguously; suggestions holds close candidates then.
        /// </summary>
        public static string ResolveColorPackage(string[] packages, string input, out string[] suggestions) =>
            Resolve(packages, input, out suggestions);

        /// <summary>
        /// Formats suggestions as a "Did you mean ...?" message.
        /// </summary>
        public static string DidYouMean(string[] suggestions) =>
            suggestions.Length <= MAX_SUGGESTIONS
                ? $"Did you mean {string.Join(", ", suggestions)}?"
                : $"Did you mean {string.Join(", ", suggestions.Take(MAX_SUGGESTIONS))} and {suggestions.Length - MAX_SUGGESTIONS} more?";

        private static string Resolve(string[] candidates, string input, out string[] suggestions)
        {
            suggestions = [];

            if (candidates == null || candidates.Length == 0 || string.IsNullOrEmpty(input))
                return null;

            string exact = candidates.FirstOrDefault(c => string.Equals(c, input, StringComparison.OrdinalIgnoreCase));

            if (exact != null) return exact;

            string[] starts = [.. candidates.Where(c => c.StartsWith(input, StringComparison.OrdinalIgnoreCase))];

            if (starts.Length == 1) return starts[0];

            string[] matches = starts.Length > 1 ? starts
                : [.. candidates.Where(c => c.Contains(input, StringComparison.OrdinalIgnoreCase))];

            if (matches.Length == 1) return matches[0];

            suggestions = matches;
            return null;
        }
    }
}
