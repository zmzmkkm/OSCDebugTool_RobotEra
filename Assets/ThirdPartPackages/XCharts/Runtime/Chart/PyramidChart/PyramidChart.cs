using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    [HelpURL("https://xcharts-team.github.io/docs/pyramid")]
    public class PyramidChart : BaseChart
    {
#pragma warning disable 0414
        [SerializeField] [ListForSerie(typeof(Pyramid))] private List<Pyramid> m_SeriePyramids = new List<Pyramid>();
#pragma warning restore 0414

        protected override void DefaultChart()
        {
            RemoveData();
            Pyramid.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
    }
}