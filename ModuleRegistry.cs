using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Holds all known modules by case-insensitive name. Owns lookup plus
    /// reset-to-defaults (removing persisted flags so every module reads
    /// enabled again). Per-module enable/disable lives on
    /// <see cref="CommandsModule"/>; the guarded <see cref="CommandsModule.IsEnabled"/>
    /// setter guarantees <c>OnEnable</c>/<c>OnDisable</c> fire exactly once per
    /// transition, so this class never needs its own state checks.
    /// </summary>
    public static class ModuleRegistry
    {
        private static readonly Dictionary<string, CommandsModule> _modules = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Snapshot of registered modules for safe enumeration. Private: only
        /// the registry may walk it, and it refreshes on every
        /// <see cref="Register"/> so it never goes stale.
        /// </summary>
        private static CommandsModule[] _cachedModules = [];

        /// <summary>All registered modules.</summary>
        public static IEnumerable<CommandsModule> All => _cachedModules;

        /// <summary>Stores a module instance. Replaces any module with the same name.</summary>
        public static void Register(CommandsModule module)
        {
            _modules[module.Name] = module;
            _cachedModules = [.. _modules.Values];
        }

        /// <summary>Looks up a module without throwing.</summary>
        public static bool TryGet(string name, out CommandsModule module)
        {
            if (string.IsNullOrEmpty(name))
            {
                module = null;
                return false;
            }

            return _modules.TryGetValue(name, out module);
        }

        /// <summary>
        /// Resets all modules to their default (enabled) state by removing every
        /// persisted flag under <see cref="CommandsModule.StorageKeyPrefix"/>
        /// and enabling every module. Redundant transitions are no-ops thanks to
        /// the guarded setter, so already-enabled modules are untouched.
        /// </summary>
        public static void ResetAll()
        {
            foreach (string key in Game.LocalStorage.GetKeys())
            {
                if (key.StartsWith(CommandsModule.StorageKeyPrefix, StringComparison.Ordinal))
                    Game.LocalStorage.RemoveItem(key);
            }

            foreach (CommandsModule module in _cachedModules)
                module.Enable();
        }

        /// <summary>
        /// Creates, registers and activates every module from its persisted
        /// <see cref="CommandsModule.Autostart"/> value. Modules without a
        /// stored value default to enabled; disabled ones stay dormant without
        /// any spurious <c>OnDisable</c> call.
        /// </summary>
        public static void RegisterAll()
        {
            Register(new PlayerModule());
            Register(new GameplayModule());

            foreach (CommandsModule module in _cachedModules)
            {
                if (module.Autostart)
                    module.Enable();
            }
        }
    }
}
