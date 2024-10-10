using System.Reflection;
using Studweb.Infrastructure.Persistance;

namespace Studweb.Infrastructure.UnitTests.Persistance.TestUtils;

public static class UnitOfWorkUtils
{
    public static void SetPrivateFieldTransactionStartedOnTrue(UnitOfWork unitOfWork)
    {
        var field = typeof(UnitOfWork)
            .GetField("_transactionStarted", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(unitOfWork, true);
    }

    public static void SetPrivateFieldDisposedOnFalse(UnitOfWork unitOfWork)
    {
        var field = typeof(UnitOfWork)
            .GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(unitOfWork, false);
    }
}