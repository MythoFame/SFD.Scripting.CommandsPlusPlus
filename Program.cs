using SFDGameScriptInterface;
using static SFD.Scripting.CommandsPlusPlus.Modules.GameScript;

namespace SFD.Scripting.CommandsPlusPlus;

public abstract class GameScriptInterfaceExtended : GameScriptInterface
{
    protected static readonly IGame Game;
}

public partial class GameScript : GameScriptInterfaceExtended
{
<<<<<<< HEAD
    public static void OnStartup()
=======
    private static readonly Random _random = Random.Shared;

    public static void OnStartup() => ModuleRegistry.RegisterAll();

    public static void OnShutdown()
>>>>>>> cf508c181bd9486ea397da7250feb8128dd8b058
    {
        JobsRule.Shutdown();
    }

    public static void AfterStartup()
    {
        RefillAllRule.Initialize();
        GrabRule.Initialize();
        RegenRule.Initialize();
        ThrowRule.Initialize();
        DmgNumbersRule.Initialize();
        SpeechRule.Initialize();
        DropinRule.Initialize();
        GmoverRule.Initialize();
        RespawnRule.Initialize();
        FriendlyFireRule.Initialize();
        GravityRule.Initialize();
        SpectationRule.Initialize();
        JobsRule.Initialize();
    }
}
