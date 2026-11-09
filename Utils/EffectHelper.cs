using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Helpers for resolving effect names against the game's
    /// <c>EffectName</c> catalog, with fuzzy matching. Users type the
    /// human-readable field names (e.g. <c>DustTrail</c>); the game
    /// itself wants the raw values (e.g. <c>TR_D</c>).
    /// </summary>
    public static class EffectHelper
    {
        private const int MAX_SUGGESTIONS = 5;

        /// <summary>Human-readable field name to raw effect value.</summary>
        private static readonly Dictionary<string, string> _effects = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Raw effect value to human-readable field name.</summary>
        private static readonly Dictionary<string, string> _names = [];

        static EffectHelper()
        {
            foreach (var field in typeof(EffectName)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (!field.IsLiteral || field.IsInitOnly || field.FieldType != typeof(string)) continue;

                string value = (string)field.GetRawConstantValue();

                if (string.IsNullOrEmpty(value)) continue;

                _effects[field.Name] = value;
                _names.TryAdd(value, field.Name);
            }
        }

        /// <summary>
        /// All known human-readable effect names from the game's catalog.
        /// </summary>
        public static string[] GetEffectNames() => [.. _effects.Keys];

        /// <summary>
        /// Fuzzy-matches a human-readable effect name. Returns the raw
        /// effect value, or null when nothing matches unambiguously;
        /// suggestions holds close candidate names then.
        /// </summary>
        public static string ResolveEffectName(string input, out string[] suggestions) =>
            Resolve([.. _effects.Keys], input, out suggestions) is string name ? _effects[name] : null;

        /// <summary>
        /// Maps a raw effect value back to its human-readable name,
        /// or the value itself when unknown.
        /// </summary>
        public static string GetDisplayName(string value) =>
            value != null && _names.TryGetValue(value, out string name) ? name : value;

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
