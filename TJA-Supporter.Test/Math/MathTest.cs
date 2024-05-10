using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TJA_Supporter.Lib.Math;

namespace TJA_Supporter.Test.Math
{
    public class MathTest
    {
        [Theory(DisplayName = "最大公約数")]
        [InlineData(2, 3, 1)]
        [InlineData(5, 4, 1)]
        [InlineData(4, 6, 2)]
        [InlineData(145, 120, 5)]
        [InlineData(608, 532, 76)]
        public void GCDTest(long x, long y, long act)
        {
            long gcd = MathHelper.GCD(x, y);
            Assert.Equal(gcd, act);
        }

        [Theory(DisplayName = "最小公倍数")]
        [InlineData(2, 3, 6)]
        [InlineData(12, 8, 24)]
        [InlineData(16, 12, 48)]
        [InlineData(13, 17, 221)]
        public void LCMTest(long x, long y, long act)
        {
            long lcm = MathHelper.LCM(x, y);
            Assert.Equal(lcm, act);
        }
    }
}
