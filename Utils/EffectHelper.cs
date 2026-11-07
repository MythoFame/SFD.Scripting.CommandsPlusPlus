using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Helpers for resolving effect names against the game's
    /// <c>EffectName</c> catalog, with fuzzy matching.
    /// </summary>
    public static class EffectHelper
    {
        private const int MAX_SUGGESTIONS = 5;

        private static readonly string[] _effectNames = [.. typeof(EffectName)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
            .Select(f => (string)f.GetRawConstantValue())
            .Where(v => !string.IsNullOrEmpty(v))];

        /// <summary>
        /// All known effect names from the game's catalog.
        /// </summary>
        public static string[] GetEffectNames() => _effectNames;

        /// <summary>
        /// Fuzzy-matches an effect name. Returns null when nothing matches
        /// unambiguously; suggestions holds close candidates then.
        /// </summary>
        public static string ResolveEffectName(string input, out string[] suggestions) =>
            Resolve(_effectNames, input, out suggestions);

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
