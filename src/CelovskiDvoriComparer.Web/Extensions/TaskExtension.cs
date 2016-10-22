using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CelovskiDvoriComparer.Web.Extensions
{
    public static class TaskExtension
    {
        public static Task<TResult[]> ForEachAsync<T, TResult>(this IEnumerable<T> sequence, Func<T, Task<TResult>> action)
        {
            return Task.WhenAll(sequence.Select(action));
        }
    }
}
