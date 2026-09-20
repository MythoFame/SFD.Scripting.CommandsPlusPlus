using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public sealed partial class PlayerModule
    {
        private static void Modifier(UserMessageCallbackArgs args)
        {
            string[] tokens = [.. ParseHelper.SplitArguments(args.CommandArguments)];

            if (tokens.Length != 3)
            {
                ShowModifierUsage(args);
                return;
            }

            var fields = typeof(PlayerModifiers).GetFields(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            var field = fields.FirstOrDefault(f => string.Equals(f.Name, tokens[1], StringComparison.OrdinalIgnoreCase));

            if (field == null)
            {
                var starts = fields.Where(f => f.Name.StartsWith(tokens[1], StringComparison.OrdinalIgnoreCase)).ToArray();

                if (starts.Length == 1)
                    field = starts[0];
                else
                {
                    var matches = starts.Length > 1 ? starts
                        : [.. fields.Where(f => f.Name.Contains(tokens[1], StringComparison.OrdinalIgnoreCase))];

                    if (matches.Length == 1)
                        field = matches[0];
                    else if (matches.Length > 1)
                    {
                        string candidates = matches.Length <= 5
                            ? string.Join(", ", matches.Select(f => f.Name))
                            : string.Join(", ", matches.Take(5).Select(f => f.Name)) + $" and {matches.Length - 5} more";

                        Game.ShowChatMessage($"Ambiguous modifier '{tokens[1]}'. Candidates: {candidates}.", Color.Red, args.User.UserIdentifier);
                        return;
                    }
                }
            }

            if (field == null)
            {
                Game.ShowChatMessage($"Unknown modifier '{tokens[1]}'.", Color.Red, args.User.UserIdentifier);
                ShowModifierUsage(args);
                return;
            }

            object value;

            if (field.FieldType == typeof(int))
            {
                if (!int.TryParse(tokens[2], out int intValue))
                {
                    Game.ShowChatMessage($"Invalid value '{tokens[2]}'.", Color.Red, args.User.UserIdentifier);
                    return;
                }

                value = intValue;
            }
            else if (field.FieldType == typeof(float))
            {
                if (!float.TryParse(tokens[2], out float floatValue))
                {
                    Game.ShowChatMessage($"Invalid value '{tokens[2]}'.", Color.Red, args.User.UserIdentifier);
                    return;
                }

                value = floatValue;
            }
            else
            {
                Game.ShowChatMessage($"Modifier '{field.Name}' has unsupported type '{field.FieldType.Name}'.", Color.Red, args.User.UserIdentifier);
                return;
            }

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

                PlayerModifiers modifiers = player.GetModifiers();
                field.SetValue(modifiers, value);
                player.SetModifiers(modifiers);
                affected++;
            }

            if (affected == 0) return;

            Game.ShowChatMessage($"Set {field.Name} to {tokens[2]} for {affected} player(s).", Color.Green, args.User.UserIdentifier);
        }

        /// <summary>
        /// Shows usage plus every <see cref="PlayerModifiers"/> field, five
        /// per message so no single chat message hits the length limit.
        /// </summary>
        private static void ShowModifierUsage(UserMessageCallbackArgs args)
        {
            Game.ShowChatMessage("Usage: /modifier <player> <modifier> <value>. Modifiers:", Color.Red, args.User.UserIdentifier);

            List<string> chunk = [];

            foreach (var field in typeof(PlayerModifiers).GetFields(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                chunk.Add(field.Name);

                if (chunk.Count == 15)
                {
                    Game.ShowChatMessage(string.Join(", ", chunk), Color.Red, args.User.UserIdentifier);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
                Game.ShowChatMessage(string.Join(", ", chunk), Color.Red, args.User.UserIdentifier);
        }
    }
}
