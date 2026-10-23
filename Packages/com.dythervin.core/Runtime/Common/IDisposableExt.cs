using System;

namespace Dythervin.Core
{
    public interface IDisposableExt : IDisposable
    {
        bool IsDisposed { get; }
    }

    public static class DisposableExtExtensions
    {
        public static bool TryDispose<T>(this T disposableExt)
            where T : IDisposableExt
        {
            if (disposableExt.IsDisposed)
                return false;

            disposableExt.Dispose();
            return true;
        }

        public static void AssertNotDisposed<T>(this T disposableExt)
            where T : IDisposableExt
        {
            if (disposableExt.IsDisposed)
                throw new Exception("Already disposed");
        }
    }
}