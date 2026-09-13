using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    /// <summary>
    /// Holds all known modules by case-insensitive name. Owns lookup plus
    /// reset-to-defaults (removing persisted flags so every module reads
    /// enabled again). Per-module enable/disable lives on <see cref="Module"/>.
    /// </summary>
    public static class ModuleRegistry
    {
        private static readonly Dictionary<string, Module> _modules = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>All registered modules.</summary>
        public static IEnumerable<Module> All => _modules.Values;

        /// <summary>Stores a module instance. Replaces any module with the same name.</summary>
        public static void Register(Module module)
        {
            _modules[module.Name] = module;
        }

        /// <summary>Looks up a module without throwing.</summary>
        public static bool TryGet(string name, out Module module)
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
        /// persisted flag under <see cref="Module.StorageKeyPrefix"/> and
        /// re-registering any module that is not currently registered. Does not
        /// write anything back to storage — removed keys simply read as default.
        /// </summary>
        public static void ResetAll()
        {
            foreach (string key in Game.LocalStorage.GetKeys())
            {
                if (key.StartsWith(Module.StorageKeyPrefix, StringComparison.Ordinal))
                    Game.LocalStorage.RemoveItem(key);
            }

            foreach (Module module in _modules.Values)
            {
                if (!module.IsRegistered)
                {
                    module.Register();
                    module.OnEnable();
                }
            }
        }
    }
}
