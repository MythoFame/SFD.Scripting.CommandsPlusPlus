using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Holds all known modules by case-insensitive name. Owns lookup plus
    /// reset-to-defaults (removing persisted flags so every module reads
    /// enabled again). Per-module enable/disable lives on <see cref="CommandsModule"/>.
    /// </summary>
    public static class ModuleRegistry
    {
        private static readonly Dictionary<string, CommandsModule> _modules = new(StringComparer.OrdinalIgnoreCase);

        // Cached version
        public static CommandsModule[] CachedModules = [];

        /// <summary>All registered modules.</summary>
        public static IEnumerable<CommandsModule> All => _modules.Values;

        /// <summary>Stores a module instance. Replaces any module with the same name.</summary>
        public static void Register(CommandsModule module)
        {
            _modules[module.Name] = module;
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
        /// persisted flag under <see cref="CommandsModule.StorageKeyPrefix"/> and
        /// re-registering any module that is not currently registered. Does not
        /// write anything back to storage — removed keys simply read as default.
        /// </summary>
        public static void ResetAll()
        {
            foreach (string key in Game.LocalStorage.GetKeys())
            {
                if (key.StartsWith(CommandsModule.StorageKeyPrefix, StringComparison.Ordinal))
                    Game.LocalStorage.RemoveItem(key);
            }

            foreach (CommandsModule module in _modules.Values)
            {
                if (!module.IsEnabled)
                    module.IsEnabled = true;
            }
        }

        public static void RegisterAll()
        {
            CommandsModule[] modules = [new PlayerModule()];

            foreach (CommandsModule module in modules)
                Register(module);

            // create a flat array version for faster access when looking up commands
            CachedModules = new CommandsModule[_modules.Count];
            for (int i = 0; i < _modules.Count; i++)
                CachedModules[i] = _modules.ElementAt(i).Value;
        }
    }
}
