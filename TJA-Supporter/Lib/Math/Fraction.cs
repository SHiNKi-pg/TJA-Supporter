using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJA_Supporter.Lib.Math
{
    /// <summary>
    /// 分数構造体
    /// </summary>
    public struct Fraction
    {
        /// <summary>
        /// 分子
        /// </summary>
        public long Numerator { get; set; }
        
        /// <summary>
        /// 分母
        /// </summary>
        public long Denominator { get; set; }

        /// <summary>
        /// 分子を分母で割った数値を取得します。
        /// </summary>
        public double Value { get => (double)Numerator / Denominator; }

        #region Constructor
        /// <summary>
        /// 分数を表します
        /// </summary>
        /// <param name="numerator">分子</param>
        /// <param name="denominator">分母</param>
        public Fraction(long numerator, long denominator)
        {
            this.Numerator = numerator;
            this.Denominator = denominator;
        }
        #endregion

        #region Method
        /// <summary>
        /// 約分し、約分後の分数を返します。
        /// </summary>
        /// <returns></returns>
        public Fraction Reduce()
        {
            Fraction frac = new(this.Numerator, this.Denominator);
            // 分子と分母を最大公約数で割る。、
            long quotient = MathHelper.GCD(frac.Numerator, frac.Denominator);
            frac.Numerator /= quotient;
            frac.Denominator /= quotient;

            return frac;
        }

        /// <summary>
        /// 分数を文字列で表示します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
        #endregion
    }
}
