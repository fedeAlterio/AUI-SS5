using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T a, T b)> ToPairs<T>(this IEnumerable<T> @this)
        {            
            var enumerator = @this.GetEnumerator();
            if (!enumerator.MoveNext())
                yield break;

            var first = enumerator.Current;
            if (!enumerator.MoveNext())
                yield break;

            var second = enumerator.Current;
            yield return (first, second);
            while(enumerator.MoveNext())
            {
                first = second;
                second = enumerator.Current;
                yield return (first, second);
            }
        }
    }
}
