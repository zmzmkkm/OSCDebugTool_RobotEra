using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Prospect
{
    public class BarChartDataModel
    {
    }

    public enum ChartAxisCount
    {
        单轴,
        双轴,
    }

    public enum ChartAxisType
    {
        类目,
        数值
    }

    public enum GradualChangeMode
    {
        单独渐变,
        全局渐变
    }

    public enum BarChartType
    {
        样式_2D,
        样式_3D
    }

    public enum BarTextPos
    {
        样式1,
        样式2,
        样式3
    }

    #region 柱形图数据结构

    /// <summary>
    /// 柱形图数据
    /// </summary>
    [Serializable]
    public class BarCharCategoryData
    {
        public string key;
        public List<BarCharValue> values;
    }

    [Serializable]
    public class BarCharValueData
    {
        public float key;
        public List<BarCharValue> values;
    }

    [Serializable]
    public class BarCharValue
    {
        public float value;
        public bool isUseRightYAxis = false; //是否使用右侧的轴
    }

    #endregion


    #region 折线图数据结构

    [Serializable]
    public class LineCharCategoryData
    {
        public string lineName;
        public bool isUseRightYAxis = false; //是否使用右侧的轴
        public List<LineCharCategoryValue> values;
    }

    [Serializable]
    public class LineCharValueData
    {
        public string lineName;
        public bool isUseRightYAxis = false; //是否使用右侧的轴
        public List<Vector2> values;
    }

    /// <summary>
    /// 折线图数据
    /// </summary>
    [Serializable]
    public class LineCharCategoryValue
    {
        public string key;
        public float value;
    }

    #endregion
}