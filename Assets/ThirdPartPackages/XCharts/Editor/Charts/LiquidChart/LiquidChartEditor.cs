using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomEditor(typeof(LiquidChart), false)]
    public class LiquidChartEditor : BaseChartEditor
    {
        [MenuItem("XCharts/LiquidChart", priority = 104)]
        [MenuItem("GameObject/XCharts/LiquidChart", priority = 104)]
        public static void AddLiquidChart()
        {
            XChartsEditor.AddChart<LiquidChart>("LiquidChart");
        }
    }
}