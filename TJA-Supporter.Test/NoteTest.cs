using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TJA_Supporter.Lib;

namespace TJA_Supporter.Test
{
    public class NoteTest
    {
        [Theory(DisplayName = "パーステスト")]
        [InlineData('0', NoteType.None, "0")]
        [InlineData('1', NoteType.Dong, "1")]
        [InlineData('2', NoteType.Ka, "2")]
        [InlineData('3', NoteType.LargeDong, "3")]
        [InlineData('4', NoteType.LargeKa, "4")]
        [InlineData('5', NoteType.Consecutive, "5")]
        [InlineData('6', NoteType.LargeConsecutive, "6")]
        [InlineData('7', NoteType.Balloon, "7")]
        [InlineData('8', NoteType.ConsecutiveEnd, "8")]
        [InlineData('9', NoteType.Potato, "9")]
        public void ParseTest(char type, NoteType noteType, string noteStr)
        {
            Note? note = Note.Parse(type);
            if (note.HasValue)
            {
                Note n = note.Value;
                Assert.Equal(n.ToString(), noteStr);
                Assert.Equal(n.Type, noteType);
            }
            else
            {
                Assert.Fail($"'{type}' note is null.");
            }
        }

        [Theory(DisplayName = "パーステスト(null)")]
        [InlineData('\0')]
        [InlineData(' ')]
        [InlineData('A')]
        [InlineData('#')]
        public void ParseToNullTest(char character)
        {
            Note? note = Note.Parse(character);
            if(note.HasValue)
                Assert.Fail($"'{character}' note is not null.");
        }
    }
}
