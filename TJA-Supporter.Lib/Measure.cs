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
        public double Length { get => (60.0 / BPM) * 4 * Beat.Value; }

        /// <summary>
        /// この小節のノーツのコンボ数を取得します。
        /// </summary>
        public int Combo { get => Notes
                .Where(n => n.Type is NoteType.Dong or NoteType.Ka or NoteType.LargeDong or NoteType.LargeKa)
                .Count();
        }

        /// <summary>
        /// 指定したインデックスにあるノーツを取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Note this[int index]
        {
            get => Notes.ElementAt(index);
        }

        /// <summary>
        /// 末尾にあるノーツを取得します。
        /// </summary>
        public Note LastNote { get => Notes.Last(); }
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

        /// <summary>
        /// 指定した音符の発音時間から、この小節のどの位置に相当するかを返します。
        /// </summary>
        /// <param name="measureStartTime">この小節が流れ始める時間</param>
        /// <param name="noteHitTime">音符の場所（時間）</param>
        /// <returns></returns>
        public (int MeasureOffset, int Index) GetNearIndex(double measureStartTime, double noteHitTime)
        {
            if (measureStartTime > noteHitTime)
                throw new ArgumentException("measureStartTime が noteHitTimeより前の時間です。");

            // この小節内の音符の相対位置を求める
            double relativeTime = noteHitTime - measureStartTime;

            // 1小節の長さからはみ出している場合は1小節内に収まるようにする
            int measureOffset = (int)(relativeTime / Length);
            if(measureOffset > 0)
                relativeTime /= measureOffset;

            // ノーツ時間 = 1音符の間隔 * 位置
            double noteInterval = Length / NotesCount;
            int index = (int)(relativeTime / noteInterval);

            return (measureOffset, index);
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

        /// <summary>
        /// 全て休符音符で構成されている小節を作成して返します。
        /// </summary>
        /// <param name="bpm">BPM</param>
        /// <param name="measure">拍子</param>
        /// <param name="notesCount">休符音符の数</param>
        /// <returns></returns>
        public static Measure CreateBlank(double bpm, Fraction measure, int notesCount)
        {
            return new(Enumerable.Repeat(new Note(NoteType.None), notesCount), bpm, measure);
        }

        /// <summary>
        /// 指定した音符の発音時間から、小節のどの位置に相当するかを返します。
        /// </summary>
        /// <param name="bpm">BPM</param>
        /// <param name="measure">拍子</param>
        /// <param name="notesCount">1小節内の休符を含めたノーツ数</param>
        /// <param name="measureStartTime">この小節が流れ始める時間</param>
        /// <param name="noteHitTime">音符の場所（時間）</param>
        /// <returns></returns>
        public static (int MeasureOffset, int Index) GetNearIndex(double bpm, Fraction measure, int notesCount, double measureStartTime, double noteHitTime)
        {
            var tmpMeasure = CreateBlank(bpm, measure, notesCount);
            return tmpMeasure.GetNearIndex(measureStartTime, noteHitTime);
        }

        #endregion
    }
}
