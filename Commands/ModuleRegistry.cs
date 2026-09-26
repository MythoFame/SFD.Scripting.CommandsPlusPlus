namespace SFD.Scripting.CommandsPlusPlus;

public partial class GameScript : GameScriptInterfaceExtended
{
    public static class ModuleRegistry
    {
        public static CommandsModule[] Modules = [];

        public static IEnumerable<Command> Commands
        {
            get
            {
                foreach (CommandsModule module in Modules)
                {
                    foreach (Command command in module.Commands)
                    {
                        yield return command;
                    }
                }
            }
        }

        public static bool TryGet(string name, out CommandsModule module)
        {
            foreach (CommandsModule mod in Modules)
            {
                if (mod.Name == name)
                {
                    module = mod;
                    return true;
                }
            }

            module = null;
            return false;
        }

        public static void ResetAll()
        {
            foreach (CommandsModule module in Modules)
                module.Reset();
        }

        /// <summary>
        /// Creates, registers and exposes every module, then applies its persisted
        /// <see cref="CommandsModule.Blocked"/> value. Modules without a
        /// stored value default to unblocked; unblocked ones stay active without
        /// any spurious <c>OnBlocked</c> call.
        /// </summary>
        public static void RegisterAll()
        {
            Modules = [
                new ManagementModule(),
                new PlayerModule(),
                new GameplayModule(),
                new FunModule()
            ];

            CommandHandler.Init();
        }
    }
}
