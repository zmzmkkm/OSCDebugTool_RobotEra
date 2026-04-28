using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomEditor(typeof(PyramidChart), false)]
    public class PyramidChartEditor : BaseChartEditor
    {
        [MenuItem("XCharts/PyramidChart", priority = 106)]
        [MenuItem("GameObject/XCharts/PyramidChart", priority = 106)]
        public static void AddPyramidChart()
        {
            XChartsEditor.AddChart<PyramidChart>("PyramidChart");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if(target == null) return;
            m_Chart = (PyramidChart)target;
        }
    }
}