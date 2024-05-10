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
        /// BPM
        /// </summary>
        public double BPM { get; set; }

        /// <summary>
        /// ノーツの個数を取得します。
        /// </summary>
        public int NotesCount { get => Notes.Count(); }

        /// <summary>
        /// 1小節の長さ（秒）を取得します。
        /// </summary>
        public double Length { get => (60.0 / BPM) * Beat.Value; }
        #endregion

        #region Constructor
        /// <summary>
        /// <see cref="Measure"/>オブジェクトを作成します。
        /// </summary>
        /// <param name="notes">ノーツ</param>
        /// <param name="beat">拍子</param>
        public Measure(IEnumerable<Note> notes, double bpm, Fraction beat)
        {
            this.Notes = notes;
            this.BPM = bpm;
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
        /// ノーツを順番に並べただけの文字列を返します。
        /// </summary>
        /// <returns></returns>
        public string ToNotesString()
        {
            return string.Join("", Notes);
        }

        /// <summary>
        /// 全てのノートの合間に指定したノーツを追加して返します。
        /// </summary>
        /// <param name="paddingNotes"></param>
        /// <returns></returns>
        public Measure Padding(IEnumerable<Note> paddingNotes)
        {
            var notes = Notes.SelectMany(n => paddingNotes.Prepend(n));
            return new Measure(notes, this.BPM, this.Beat);
        }

        /// <summary>
        /// 全てのノートの合間に指定したノーツを追加して返します。
        /// </summary>
        /// <param name="noteStr"></param>
        /// <returns></returns>
        public Measure Padding(string noteStr)
        {
            Measure m = Measure.Parse(noteStr, this.BPM, this.Beat);
            return new(m.Notes, this.BPM, this.Beat);
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

        /// <summary>
        /// 小節の最初から指定した位置にあるノーツまでの長さ（秒）を返します。
        /// 
        /// </summary>
        /// <param name="noteCount">何個目のノーツか</param>
        /// <returns></returns>
        public double GetLengthUntil(int noteCount)
        {
            return ((double)noteCount / NotesCount) * Length;
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 文字列から<see cref="Measure"/>オブジェクトを作成します。
        /// </summary>
        /// <param name="notesStr"></param>
        /// <param name="bpm"></param>
        /// <param name="beat"></param>
        /// <returns></returns>
        public static Measure Parse(string notesStr, double bpm, Fraction beat)
        {
            List<Note> notes = new();
            foreach(char note in notesStr)
            {
                var n = Note.Parse(note);
                if(n.HasValue)
                    notes.Add(n.Value);
            }
            return new(notes, bpm, beat);
        }
        #endregion
    }
}
