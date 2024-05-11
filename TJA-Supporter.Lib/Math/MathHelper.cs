using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TJA_Supporter.Lib.Text;

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
            if(x < y)
            {
                // yの方が大きければ変数の値を入れ替える
                long tmp = x;
                x = y;
                y = tmp;
            }
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

        #region ParseToComplex
        /// <summary>
        /// 複素数文字列を<see cref="Complex"/>オブジェクトに変換します。
        /// </summary>
        /// <param name="complexText"></param>
        /// <returns></returns>
        public static Complex ParseToComplex(string complexText)
        {
            bool result = complexText.PullOut(@"((?<real>-?\d+\.?\d*))?\+?((?<imaginary>-?\d+\.?\d*)i)?",
                gc =>
                {
                    double real = 0;
                    double imaginary = 0;
                    if (gc.ContainsKey("real") && !string.IsNullOrEmpty(gc["real"].Value))
                        real = double.Parse(gc["real"].Value);
                    if (gc.ContainsKey("imaginary") && !string.IsNullOrEmpty(gc["imaginary"].Value))
                        imaginary = double.Parse(gc["imaginary"].Value);
                    return new(real, imaginary);
                },
                out Complex c);
            if (result)
                return c;
            else
                throw new ArgumentException($"Text '{complexText} cannot parse.'");
        }
        #endregion
    }
}
