using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 水位图
    /// </summary>
    [AddComponentMenu("XCharts/LiquidChart", 22)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/liquid")]
    public class LiquidChart : BaseChart
    {

#pragma warning disable 0414
        [SerializeField][ListForComponent(typeof(Vessel))] private List<Vessel> m_Vessels = new List<Vessel>();
        [SerializeField][ListForSerie(typeof(Liquid))] private List<Liquid> m_SerieLiquids = new List<Liquid>();
#pragma warning restore 0414

        protected override void DefaultChart()
        {
            GetChartComponent<Tooltip>().type = Tooltip.Type.Line;
            RemoveData();

            Liquid.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
    }
}