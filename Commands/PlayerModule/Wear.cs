using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Wear(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length < 3 || tokens.Length > 5)
            {
                ShowWearUsage(args);
                return;
            }

            var slots = typeof(IProfile).GetFields(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(f => f.FieldType == typeof(IProfileClothingItem)).ToArray();

            var slot = slots.FirstOrDefault(f => string.Equals(f.Name, tokens[1], StringComparison.OrdinalIgnoreCase));

            if (slot == null)
            {
                Game.ShowChatMessage($"Unknown slot '{tokens[1]}'.", Color.Red, args.User.UserIdentifier);
                ShowWearUsage(args);
                return;
            }

            string color1 = tokens.Length > 3 ? ToColorPackage(tokens[3]) : "";
            string color2 = tokens.Length > 4 ? ToColorPackage(tokens[4]) : "";

            IProfileClothingItem item = new(Capitalize(tokens[2]), color1, color2);

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

                IProfile profile = player.GetProfile();
                slot.SetValue(profile, item);
                player.SetProfile(profile);
                affected++;
            }

            if (affected == 0) return;

            Game.ShowChatMessage($"Set {slot.Name} to '{tokens[2]}' for {affected} player(s).", Color.Green, args.User.UserIdentifier);
        }

        /// <summary>
        /// Forces the first letter to upper case, e.g. balaclava to Balaclava.
        /// </summary>
        private static string Capitalize(string value) =>
            string.IsNullOrEmpty(value) ? value : char.ToUpper(value[0]) + value[1..];

        /// <summary>
        /// Maps a color name to its package name, e.g. red to ClothingRed
        /// and lightgrey to ClothingLightGrey. Empty stays empty.
        /// </summary>
        private static string ToColorPackage(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";

            if (value.StartsWith("light", StringComparison.OrdinalIgnoreCase) && value.Length > 5)
                return "ClothingLight" + Capitalize(value[5..]);

            if (value.StartsWith("dark", StringComparison.OrdinalIgnoreCase) && value.Length > 4)
                return "ClothingDark" + Capitalize(value[4..]);

            return "Clothing" + Capitalize(value);
        }

        /// <summary>
        /// Shows usage plus every clothing slot in a single message.
        /// </summary>
        private static void ShowWearUsage(UserMessageCallbackArgs args)
        {
            Game.ShowChatMessage("Usage: /wear <player> <slot> <name> [color1] [color2].", Color.Red, args.User.UserIdentifier);

            var slots = typeof(IProfile).GetFields(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(f => f.FieldType == typeof(IProfileClothingItem));

            Game.ShowChatMessage($"Slots: {string.Join(", ", slots.Select(f => f.Name))}.", Color.Red, args.User.UserIdentifier);
        }
    }
}
