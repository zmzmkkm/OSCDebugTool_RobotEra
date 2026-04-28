// ========================================================
// 描 述：BaseChart.Draw扩展绘制折线图下的symbol
// 作 者：张成
// 创建时间：2023/12/22 14:05:13
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using UnityEngine.UI;


namespace XCharts.Runtime
{
    public partial class BaseChart
    {
        public void DrawClipSymbol(VertexHelper vh, SymbolType type,Vector3 pos,GameObject parent,SymbolStyle symbol)
        {
            if (!IsInChart(pos)) 
                return;
            if (parent == null)
                return;
            DrawSymbol(vh, type,pos,parent,symbol);
        }

        public void DrawSymbol(VertexHelper vh, SymbolType type,Vector3 pos, GameObject parent, SymbolStyle symbol)
        {            
            DrawSymbolExtension(vh, type,pos,parent.transform, symbol);
        }

        public void DrawSymbolExtension(VertexHelper vh, SymbolType type, Vector3 pos,Transform parent, SymbolStyle symbol)
        {            
            ChartDrawer.DrawSymbolExtension(vh, type,pos, parent, symbol);
        }
    }	
}