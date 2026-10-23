using System.Collections.Generic;

namespace Utils.Extensions
{
    public static class MiscExtensions
    {
        public static bool IsSame<T>(this IList<T> a, IList<T> b) where T : class
        {
            int count = a.Count;
            if (count != b.Count) 
                return false;
            
            for (int i = 0; i < count; i++)
            {
                if (!ReferenceEquals(a[i], b[i]))
                    return false;
            }

            return true;
        }
    }
}