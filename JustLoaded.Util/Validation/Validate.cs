using System.Diagnostics.CodeAnalysis;

namespace JustLoaded.Util.Validation;

public static class Validate {

    public static T Matches<T>(this T value, Func<T, bool> predicate, Func<Exception>? lazyException = null) {
        if (!predicate(value)) {
            Fail(lazyException);
        }
        return value;
    }
    
    public static T IsNotNull<T>(this T? value, Func<Exception>? lazyException = null) {
        if (value == null) {
            Fail(lazyException);
        }

        return value;
    }
    
    public static void IsEqualTo<T>(this T? thisValue, T? predictedValue, Func<Exception>? lazyException = null) {
        if (thisValue == null) {
            if (predictedValue != null) {
                Fail();
            }
            return;
        }

        thisValue.Equals(predictedValue).IsTrue(lazyException);
    }
    
    public static void IsTrue([DoesNotReturnIf(false)] this bool value, Func<Exception>? lazyException = null) {
        if (!value) {
            Fail(lazyException);
        }
    }

    [DoesNotReturn]
    public static void Fail(Func<Exception>? lazyException = null) {
        throw lazyException?.Invoke() ?? new ValidationException("Validation Failed");
    }
    
}