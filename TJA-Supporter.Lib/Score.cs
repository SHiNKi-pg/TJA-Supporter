using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TJA_Supporter.Lib.Math;
using TJA_Supporter.Lib.Text;

namespace TJA_Supporter.Lib
{
    /// <summary>
    /// 譜面クラス
    /// </summary>
    public class Score
    {
        #region Property
        /// <summary>
        /// 小節
        /// </summary>
        public IList<Measure> Measures { get; }

        /// <summary>
        /// 指定したインデックスにある小節を返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Measure this[int index]
        {
            get => Measures.ElementAt(index);
        }

        /// <summary>
        /// 先頭にある小節を取得します。小節が存在しない場合は nullを返します。
        /// </summary>
        public Measure? FirstMeasure
        {
            get
            {
                if (Measures.Count > 0)
                    return Measures.First();
                else
                    return null;
            }
        }

        /// <summary>
        /// 末尾にある小節を取得します。小節が存在しない場合は nullを返します。
        /// </summary>
        public Measure? LastMeasure
        {
            get
            {
                if (Measures.Count > 0)
                    return Measures.Last();
                else
                    return null;
            }
        }

        /// <summary>
        /// 小節の個数を返します。
        /// </summary>
        public int MeasureCount { get => Measures.Count(); }

        /// <summary>
        /// この譜面のコンボ数を取得します。
        /// </summary>
        public int Combo { get => Measures.Sum(m => m.Combo); }

        /// <summary>
        /// この譜面の長さ（秒）を取得します。
        /// </summary>
        public double Length { get => Measures.Sum(m => m.Length); }
        #endregion

        #region Constructor
        /// <summary>
        /// <see cref="Score"/>オブジェクトを作成します。
        /// </summary>
        /// <param name="measures"></param>
        public Score(IList<Measure> measures)
        {
            this.Measures = measures;
        }
        #endregion

        #region Method
        /// <summary>
        /// 小節を追加します。
        /// </summary>
        /// <param name="measure"></param>
        public void AddMeasure(Measure measure)
        {
            Measures.Add(measure);
        }

        /// <summary>
        /// 小節を追加します。
        /// </summary>
        /// <param name="getMeasure">小節を返す関数。引数には1つ前の小節が渡されます。</param>
        public void AddMeasure(Func<Measure?, Measure> getMeasure)
        {
            Measures.Add(getMeasure(LastMeasure));
        }

        /// <summary>
        /// 空白の小節を追加します。
        /// </summary>
        /// <param name="notesCount">休符数</param>
        /// <param name="defaultBPM">デフォルトBPM</param>
        /// <param name="defaultBeatNumerator">デフォルトの拍子（分子）</param>
        /// <param name="defaultBeatDenominator">デフォルトの拍子（分母）</param>
        public void AddBlankMeasure(int notesCount, double defaultBPM = 120, long defaultBeatNumerator = 4, long defaultBeatDenominator = 4)
        {
            AddMeasure(m =>
            {
                if (m.HasValue)
                {
                    return Measure.CreateBlank(m.Value.BPM, m.Value.Beat, notesCount);
                }
                else
                {
                    return Measure.CreateBlank(defaultBPM, new Fraction(defaultBeatNumerator, defaultBeatDenominator), notesCount);
                }
            });
        }

        /// <summary>
        /// 指定した小節のインデックス番号までの長さ（秒）を返します。
        /// </summary>
        /// <param name="measureIndex"></param>
        /// <returns></returns>
        public double GetLengthUntil(int measureIndex)
        {
            return Measures.Take(measureIndex).Sum(m => m.Length);
        }

        /// <summary>
        /// 譜面データを文字列として返します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder score = new();
            Complex beforeScroll = 0;
            double bpm = double.NegativeInfinity;
            Fraction meas = new(0, 1);
            foreach(var measure in Measures)
            {
                if(bpm != measure.BPM)
                {
                    score.AppendLine($"#BPMCHANGE {measure.BPM}");
                    bpm = measure.BPM;
                }
                if(meas.Value != measure.Beat.Value)
                {
                    score.AppendLine($"#MEASURE {measure.Beat}");
                    meas = measure.Beat;
                }
                
                score.AppendLine(measure.ToString(beforeScroll));
                Note? lastNote = measure.LastNote;
                if(lastNote is not null)
                    beforeScroll = lastNote.Scroll;
            }
            return score.ToString();
        }
        #endregion

        #region Static Method
        public static Score Parse(string scoreStr)
        {
            // CR文字を空白に置換する
            scoreStr = scoreStr.Replace('\r', '\0');

            List<Measure> measures = new();
            List<Note> notes = new();

            // 初期BPMと拍子、スクロール
            double bpm = 120;
            Fraction beat = new(4, 4);
            Complex scroll = 1;

            // 1行ずつ譜面解析
            var lines = scoreStr.Split('\n');
            foreach(var line in lines)
            {
                // BPMCHANGE
                if(line.PullOut(@"#BPMCHANGE (?<bpm>.+)", g => double.Parse(g["bpm"].Value), out var outbpm))
                {
                    bpm = outbpm;
                    continue;
                }
                // MEASURE
                else if(line.PullOut(@"#MEASURE (?<n>\d+)/(?<d>\d+)",
                    g => (long.Parse(g["n"].Value), long.Parse(g["d"].Value)),
                    out var meas))
                {
                    beat = new(meas.Item1, meas.Item2);
                    continue;
                }
                // SCROLL
                else if(line.PullOut(@"#SCROLL (?<c>.+)", g => g["c"].Value, out var complexStr))
                {
                    Complex scr = MathHelper.ParseToComplex(complexStr);
                    scroll = scr;
                    continue;
                }
                // ノーツ解析
                else
                {
                    foreach(var c in line)
                    {
                        if(c == ',')
                        {
                            // 終点記号
                            Measure measure = new(notes, bpm, beat);
                            measures.Add(measure);
                            notes = new();
                        }
                        else
                        {
                            Note? note = Note.Parse(c);
                            if (note is not null)
                            {
                                // NOTE: note.Value.Scroll のように直接指定できないので一時変数に代入してから設定している
                                Note nt = note;
                                nt.Scroll = scroll;
                                notes.Add(nt);
                            }
                        }
                    }
                }
            }
            return new Score(measures);
        }
        #endregion

    }
}
