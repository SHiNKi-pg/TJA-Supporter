using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TJA_Supporter.Lib.Math;

namespace TJA_Supporter.Test.Math
{
    public class ComplexTest
    {
        [Theory(DisplayName = "複素数文字列")]
        [InlineData(0, 0, "0")]
        [InlineData(1, 0, "1")]
        [InlineData(0, 1, "1i")]
        [InlineData(1, 1, "1+1i")]
        [InlineData(-1, 0, "-1")]
        [InlineData(0, -1, "-1i")]
        [InlineData(-2, -2, "-2-2i")]
        [InlineData(3.5, 4.1, "3.5+4.1i")]
        public void ComplexStringTest(double real, double imaginary, string ans)
        {
            var c = new Complex(real, imaginary);
            Assert.Equal(c.ToComplexString(), ans);
        }

        [Theory(DisplayName = "複素数パース")]
        [InlineData("0", 0, 0)]
        [InlineData("1", 1, 0)]
        [InlineData("0+1i", 0, 1)]
        [InlineData("1+1i", 1, 1)]
        [InlineData("-1", -1, 0)]
        [InlineData("0-1i", 0, -1)]
        [InlineData("-1-1i", -1, -1)]
        [InlineData("0.5+0.8i", 0.5, 0.8)]
        [InlineData("-1.414+1.414i", -1.414, 1.414)]
        public void ParseComplexTest(string input, double real, double imaginary)
        {
            Complex complex = MathHelper.ParseToComplex(input);
            Assert.Equal(complex, new Complex(real, imaginary));
        }
    }
}
