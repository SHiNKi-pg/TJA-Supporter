using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TJA_Supporter.Lib;

namespace TJA_Supporter.Test
{
    public class MeasureUnitTest
    {
        public MeasureUnitTest()
        {

        }

        [Theory(DisplayName = "パーステスト")]
        [InlineData("1111", 4, 4, 120, 4, 2)]
        [InlineData("10201020", 4, 4, 120, 8, 2)]
        [InlineData("10201020", 3, 4, 120, 8, 1.5)]
        [InlineData("10203040", 8, 4, 120, 8, 4)]
        public void ParseTest(string input, long bn, long bd, double bpm, int notesCount, double length)
        {
            var measure = Measure.Parse(input, bpm, new Lib.Math.Fraction(bn, bd));
            Assert.Equal(measure.NotesCount, notesCount);
            Assert.Equal(measure.Length, length);
        }

        [Theory(DisplayName = "ノーツパディング")]
        [InlineData("1010", 1, "10001000")]
        [InlineData("11221", 2, "100100200200100")]
        public void PaddingTest(string input, int appendSize, string act)
        {
            var measure = Measure.Parse(input, 120, new Lib.Math.Fraction(4, 4));
            var padded = measure.Padding(appendSize);
            Assert.Equal(padded.ToNotesString(), act);
        }
    }
}
