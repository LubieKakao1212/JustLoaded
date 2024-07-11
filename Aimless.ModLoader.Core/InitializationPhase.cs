namespace Aimless.ModLoader.Core;

public static class InitializationPhaseExtensions {
    
    public static bool IsBefore(this InitializationPhase phase, InitializationPhase other) {
        return (int)phase < (int)other;
    }
    
    public static bool IsAtMost(this InitializationPhase phase, InitializationPhase other) {
        return (int)phase <= (int)other;
    }
    
    public static bool IsAfter(this InitializationPhase phase, InitializationPhase other) {
        return (int)phase > (int)other;
    }

    public static bool IsAtLeast(this InitializationPhase phase, InitializationPhase other) {
        return (int)phase >= (int)other;
    }
    public static bool IsAt(this InitializationPhase phase, InitializationPhase other) {
        return (int)phase == (int)other;
    }

    public static bool IsErrored(this InitializationPhase phase) {
        return phase.IsBefore(InitializationPhase.Created);
    }

    public static void AssertBefore(this InitializationPhase phase, InitializationPhase other) {
        Assert(phase, other, IsBefore, $"Mismatched Initialization order, expected to be before {other} but was at {phase}");
    }
    
    public static void AssertAtMost(this InitializationPhase phase, InitializationPhase other) {
        Assert(phase, other, IsAtMost, $"Mismatched Initialization order, expected to be at most at {other} but was at {phase}");
    }

    public static void AssertAfter(this InitializationPhase phase, InitializationPhase other) {
        Assert(phase, other, IsAfter, $"Mismatched Initialization order, expected to be after {other} but was at {phase}");
    }
    
    public static void AssertAtLeast(this InitializationPhase phase, InitializationPhase other) {
        Assert(phase, other, IsAtLeast, $"Mismatched Initialization order, expected to be at least at {other} but was at {phase}");
    }
    
    public static void AssertAt(this InitializationPhase phase, InitializationPhase other) {
        Assert(phase, other, IsAt, $"Mismatched Initialization order, expected to be at least at {other} but was at {phase}");
    }

    
    public static void AssertNotErrored(this InitializationPhase phase) {
        if (IsErrored(phase)) {
            //TODO Exception
            throw new ApplicationException("There was an error during mod loading: " + phase);
        }
    }

    private static void Assert(InitializationPhase phase, InitializationPhase other, Func<InitializationPhase, InitializationPhase, bool> comparison, string message) {
        AssertNotErrored(phase);
        if (!comparison(phase, other)) {
            //TODO Exception
            throw new ApplicationException(message);
        }
    }
    
}

public enum InitializationPhase : int {
    Created = 0,
    DiscoveringMods,
    ModsSet,
    DependenciesResolved,
    ModSystemInitializing,
    ModSystemInitialized,
    Loading,
    Ready,
    
    ErroredGeneric = -1,
    ErroredInvalidState = -2,
    ErroredDuplicateMods = -3,
    ErroredMissingModDependencies = -4,
}