// ========================================================
// 描 述：LineHadnler扩展
// 作 者：张成
// 创建时间：2023/12/22 13:55:46
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using UnityEngine.UI;


namespace XCharts.Runtime
{
    internal sealed partial class LineHandler : SerieHandler<Line>
    {
        private void DrawLinePointSymbolCustom(VertexHelper vh, Serie serie,GameObject game)
        {
            if (!serie.show || serie.IsPerformanceMode())
                return;

            if (m_SerieGrid == null)
                return;

            int count = serie.context.dataPoints.Count;
            for (int i = 0; i < count; i++)
            {
                var index = serie.context.dataIndexs[i];
                var serieData = serie.GetSerieData(index);
                if (serieData == null)
                    continue;    
                var state = SerieHelper.GetSerieState(serie, serieData, true);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData, state);

                if (!symbol.show || !symbol.ShowSymbol(index, count))
                    continue;

                var pos = serie.context.dataPoints[i];   
                if (ChartHelper.IsIngore(pos))
                    continue;    
                //Debug.Log($"<color=blue>数据坐标: {serie.context.dataPoints[i]}</color>");
                chart.DrawClipSymbol(vh, symbol.type,pos,game, symbol);
            }
            chart.RefreshPainter(serie);
        }
    }	
}