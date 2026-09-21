using SFDGameScriptInterface;

namespace SFD.Scripting.CommandsPlusPlus;

public abstract class GameScriptInterfaceExtended : GameScriptInterface
{
    protected static readonly IGame Game;
}

public partial class GameScript : GameScriptInterfaceExtended
{
    private static readonly Random _random = Random.Shared;

    public static void OnStartup() => ModuleRegistry.RegisterAll();

    public static void OnShutdown()
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
