using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TJA_Supporter.Lib.Math;

namespace TJA_Supporter.Lib.Scores
{
    /// <summary>
    /// 譜面ヘルパー
    /// </summary>
    public static class ScoreHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseBpm">ベースとなるBPM</param>
        /// <param name="baseBeat">ベースとなる拍子</param>
        /// <param name="baseNotesSize">1小節当たりの休符含むノーツ数</param>
        /// <param name="scores">マージする譜面と速度</param>
        /// <returns></returns>
        public static Score NearMerge(double baseBpm, Fraction baseBeat, int baseNotesSize,
            params (Score, Complex)[] scores)
        {
            List<Measure> measures = new();

            // 尺が長い方に合わせる
            double maxLength = scores.Select(t => t.Item1.Length).Max();

            // 最低何小節必要か求める
            double measureCountDbl = (maxLength * baseBpm) / (240 * baseBeat.Value);
            int measureCount = (int)System.Math.Round(measureCountDbl, 0);
            // 求めた小節数分 空の小節を生成する
            for (int i = 0; i < measureCount; i++)
                measures.Add(Measure.CreateBlank(baseBpm, baseBeat, baseNotesSize));

            Score mergedScore = new(measures);
            foreach(var scoreInfo in scores)
            {
                // 小節の列挙
                int i_measure = 0;
                foreach(var measure in scoreInfo.Item1.Measures)
                {
                    double totalLength = scoreInfo.Item1.GetLengthUntil(i_measure);
                    // ノーツと長さの列挙（休符は対象外）
                    foreach(var noteAndLength in measure.EnumerateNotesAndLength().Where(n => n.Item1.Type != NoteType.None))
                    {
                        var offsetAndIndex = mergedScore.FirstMeasure!.Value.GetNearIndex(0, totalLength + noteAndLength.Item2);
                        mergedScore[offsetAndIndex.MeasureOffset][offsetAndIndex.Index].ChangeType(noteAndLength.Item1.Type);
                        mergedScore[offsetAndIndex.MeasureOffset][offsetAndIndex.Index].ChangeScroll(scoreInfo.Item2);
                    }
                    i_measure++;
                }
            }
            return mergedScore;
        }
    }
}
