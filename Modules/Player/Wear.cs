using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Wear(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];
            int uid = args.User?.UserIdentifier ?? -1;

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
                Game.ShowChatMessage($"Unknown slot '{tokens[1]}'.", Color.Red, uid);
                ShowWearUsage(args);
                return;
            }

            string color1 = tokens.Length > 3 ? ColorHelper.ToColorPackage(tokens[3]) : "";
            string color2 = tokens.Length > 4 ? ColorHelper.ToColorPackage(tokens[4]) : "";

            IProfileClothingItem item = new(ColorHelper.Capitalize(tokens[2]), color1, color2);

            IPlayer[] players = [.. ParseHelper.ParsePlayers(tokens[0], args.User)];

            if (players.Length == 0)
            {
                Game.ShowChatMessage($"Player '{tokens[0]}' not found.", Color.Red, uid);
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

            Game.ShowChatMessage($"Set {slot.Name} to '{tokens[2]}' for {affected} player(s).", Color.Green, uid);
        }

        /// <summary>
        /// Shows usage plus every clothing slot in a single message.
        /// </summary>
        private static void ShowWearUsage(UserMessageCallbackArgs args)
        {
            int uid = args.User?.UserIdentifier ?? -1;
            Game.ShowChatMessage("Usage: /wear <player> <slot> <name> [color1] [color2].", Color.Red, uid);

            var slots = typeof(IProfile).GetFields(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(f => f.FieldType == typeof(IProfileClothingItem));

            Game.ShowChatMessage($"Slots: {string.Join(", ", slots.Select(f => f.Name))}.", Color.Red, uid);
        }
    }
}
