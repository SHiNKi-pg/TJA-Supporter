using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJA_Supporter.Lib
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/>関連拡張クラス
    /// </summary>
    public static class EnumerableEx
    {
        /// <summary>
        /// このオブジェクトだけを含む<see cref="IEnumerable{T}"/>オブジェクトを返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<T> Just<T>(this T obj)
        {
            yield return obj;
        }
    }
}
