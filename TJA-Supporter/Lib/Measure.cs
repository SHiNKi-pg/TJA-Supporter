using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TJA_Supporter.Lib.Math;

namespace TJA_Supporter.Lib
{
    /// <summary>
    /// 1小節を表す構造体
    /// </summary>
    public struct Measure
    {
        #region Property
        /// <summary>
        /// ノーツ
        /// </summary>
        public IEnumerable<Note> Notes { get; }

        /// <summary>
        /// 拍子
        /// </summary>
        public Fraction Beat { get; set; }

        /// <summary>
        /// ノーツの個数を取得します。
        /// </summary>
        public int NotesCount { get => Notes.Count(); }

        /// <summary>
        /// 1小節の長さを取得します。
        /// </summary>
        public double Length { get => NotesCount * Beat.Value; }
        #endregion

        #region Constructor
        /// <summary>
        /// <see cref="Measure"/>オブジェクトを作成します。
        /// </summary>
        /// <param name="notes">ノーツ</param>
        /// <param name="beat">拍子</param>
        public Measure(IEnumerable<Note> notes, Fraction beat)
        {
            this.Notes = notes;
            this.Beat = beat;
        }
        #endregion

        #region Method
        public string ToString(Complex beforeScrollSpeed)
        {
            Complex beforeSpeed = beforeScrollSpeed;
            StringBuilder notes = new();
            foreach(Note note in Notes)
            {
                if(note.Scroll == beforeScrollSpeed)
                {
                    // 前回スクロール値と同じ場合は同じ行に出力
                    notes.Append(note.Type.ToString());
                }
                else
                {
                    notes.AppendLine();
                    notes.AppendLine($"#SCROLL {note.Scroll.ToComplexString()}");
                    beforeScrollSpeed = note.Scroll;
                }
            }
            if (notes.Length > 0)
                return notes.ToString() + ",";
            else
                return NoteType.None.ToString() + ",";
        }

        public override string ToString()
        {
            return ToString(1);
        }

        /// <summary>
        /// 全てのノートの合間に指定したノーツを追加して返します。
        /// </summary>
        /// <param name="paddingNotes"></param>
        /// <returns></returns>
        public Measure Padding(IEnumerable<Note> paddingNotes)
        {
            var notes = Notes.SelectMany(n => paddingNotes.Prepend(n));
            return new Measure(notes, this.Beat);
        }

        /// <summary>
        /// 全てのノートの合間に指定したノーツを追加して返します。
        /// </summary>
        /// <param name="noteType">挿入するノートの種類</param>
        /// <param name="paddingSize">追加するノートの個数</param>
        /// <returns></returns>
        public Measure Padding(NoteType noteType, int paddingSize)
        {
            Note note = new(noteType, 1);
            return Padding(Enumerable.Repeat(note, paddingSize));
        }

        /// <summary>
        /// 全てのノートの合間に指定した数の休符ノーツを追加して返します。
        /// </summary>
        /// <param name="paddingSize">各ノートに対して追加するノートの個数</param>
        /// <returns></returns>
        public Measure Padding(int paddingSize)
        {
            return Padding(NoteType.None, paddingSize);
        }

        #endregion
    }
}
