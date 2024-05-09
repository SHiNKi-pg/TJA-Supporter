using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TJA_Supporter.Lib
{
    /// <summary>
    /// ノーツタイプ
    /// </summary>
    public enum NoteType : int
    {
        /// <summary>
        /// 休符
        /// </summary>
        None = 0,
        /// <summary>
        /// ドン
        /// </summary>
        Dong = 1,
        /// <summary>
        /// カ
        /// </summary>
        Ka = 2,
        /// <summary>
        /// ドン（大）
        /// </summary>
        LargeDong = 3,
        /// <summary>
        /// カッ（大）
        /// </summary>
        LargeKa = 4,
        /// <summary>
        /// 連打開始
        /// </summary>
        Consecutive = 5,
        /// <summary>
        /// 連打開始（大）
        /// </summary>
        LargeConsecutive = 6,
        /// <summary>
        /// 風船
        /// </summary>
        Balloon = 7,
        /// <summary>
        /// 連打終点
        /// </summary>
        ConsecutiveEnd = 8,
        /// <summary>
        /// 芋連打／くす玉
        /// </summary>
        Potato = 9,
    }

    /// <summary>
    /// ノート構造体
    /// </summary>
    public struct Note
    {
        /// <summary>
        /// ノーツタイプ
        /// </summary>
        public NoteType Type { get; set; }

        /// <summary>
        /// スクロール速度
        /// </summary>
        public Complex Scroll { get; set; }

        #region Constructor
        public Note(NoteType type, Complex speed)
        {
            this.Type = type;
            this.Scroll = speed;
        }

        public Note(NoteType type) : this(type, 1) { }
        #endregion

        #region Method
        public override string ToString()
        {
            return Type.ToString();
        }
        #endregion
    }
}
