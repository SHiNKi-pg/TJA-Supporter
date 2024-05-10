using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TJA_Supporter.Lib.Math
{
    /// <summary>
    /// 数学計算ヘルパークラス
    /// </summary>
    public static class MathHelper
    {
        #region GCD
        /// <summary>
        /// 2つの整数の最大公約数を求めて返します。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static long GCD(long x, long y)
        {
            // NOTE: ユークリッドの互除法を用いて最大公約数を求める。
            while (true)
            {
                long remainder = x % y;
                if(remainder == 0) {
                    return y;
                }
                x = y;
                y = remainder;
            }
        }
        #endregion

        #region LCM
        /// <summary>
        /// 2つの整数の最小公倍数を求めて返します。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static long LCM(long x, long y)
        {
            // NOTE: 最小公倍数と最大公約数の積と2つの整数の積が等しいことを利用する。
            return (x * y) / GCD(x, y);
        }
        #endregion

        #region ComplexString
        public static string ToComplexString(this Complex complex)
        {
            if (complex.Real == 0 && complex.Imaginary == 0)
                return "0";
            else if (complex.Imaginary == 0)
                return complex.Real.ToString();
            else if (complex.Real == 0)
                return complex.Imaginary.ToString() + "i";
            else
            {
                StringBuilder compStr = new();
                compStr.Append(complex.Real.ToString());
                compStr.Append(complex.Imaginary > 0 ? "+" : "");
                compStr.Append(complex.Imaginary.ToString());
                compStr.Append("i");
                return compStr.ToString();
            }
        }
        #endregion
    }
}
