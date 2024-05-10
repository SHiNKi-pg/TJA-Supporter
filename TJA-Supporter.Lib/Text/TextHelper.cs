using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TJA_Supporter.Lib.Text
{
    /// <summary>
    /// 文字列ヘルパークラス
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 指定した正規表現パターンにマッチした場合、マッチした値を別の型に変換して返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="pattern">正規表現パターン</param>
        /// <param name="pullValueWhenFound">正規表現パターンにマッチした文字列を変換する処理</param>
        /// <param name="value">マッチした文字列を変換したオブジェクト</param>
        /// <returns></returns>
        public static bool PullOut<T>(this string input,
            [StringSyntax(StringSyntaxAttribute.Regex)] string pattern,
            Func<GroupCollection, T> pullValueWhenFound,
            [NotNullWhen(true)] out T? value)
        {
            Regex regex = new(pattern);
            Match match = regex.Match(input);
            if (match.Success)
            {
                value = pullValueWhenFound(match.Groups)!;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    }
}
