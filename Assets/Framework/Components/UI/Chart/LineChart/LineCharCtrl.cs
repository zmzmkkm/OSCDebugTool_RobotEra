using System;
using System.Collections.Generic;
using System.Linq;
using Prospect;
using SW;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class LineCharCtrl : GraphAxisBase
{
    /// <summary>
    /// 数值:点位弹框事件
    /// 数据，线索引，点索引，点的Transform
    /// </summary>
    public Action<Vector2, int, int, Transform> setTipActionValue;

    /// <summary>
    /// 类目:点位弹框事件
    /// 数据，线索引，点索引，点的Transform
    /// </summary>
    public Action<LineCharCategoryValue, int, int, Transform> setTipActionCategory;

    /// <summary>
    /// 数值:点位Label事件
    /// 数值，线索引，点索引，TextMeshProUGUI，textTrs
    /// </summary>
    public Action<Vector2, int, int, TextMeshProUGUI, RectTransform> setLabelActionValue;

    /// <summary>
    /// 类目:点位Label事件
    /// 数值，线索引，点索引，TextMeshProUGUI，textTrs
    /// </summary>
    public Action<LineCharCategoryValue, int, int, TextMeshProUGUI, RectTransform> setLabelActionCategory;


    private Transform _points;

    private Transform _drawLinesPanel;
    private DrawLineGraph _drawLineGraph;

    [Header("线的属性：")] public List<LineAttribute> lineAttributeList = new List<LineAttribute>();

    #region 弹框

    [Space(20), Header("弹框：")] public bool isShowTip = true;
    public Transform tip;
    [Tooltip("弹框位置偏移量")] public Vector2 tipOffset;

    #endregion

    [Header("数据："), Tooltip("X轴类型")] public ChartAxisType xAxisType = ChartAxisType.类目;
    [Tooltip("是否为时间")] public bool isTime;


    public List<LineCharCategoryData> dataCategory = new()
    {
        new LineCharCategoryData()
        {
            lineName = "",
            values = new()
            {
                new LineCharCategoryValue() { key = "类目1", value = 20 },
                new LineCharCategoryValue() { key = "类目2", value = 30 },
                new LineCharCategoryValue() { key = "类目3", value = 27 },
                new LineCharCategoryValue() { key = "类目4", value = 18 },
                new LineCharCategoryValue() { key = "类目5", value = 41 },
            }
        }
    };

    public List<LineCharValueData> dataValue = new()
    {
        new LineCharValueData()
        {
            lineName = "",
            values = new()
            {
                new Vector2(1, 23),
                new Vector2(3, 14),
                new Vector2(4, 29),
                new Vector2(10, 33),
                new Vector2(12, 24),
            }
        }
    };


#if UNITY_EDITOR


    private void OnValidate()
    {
        if (Application.isPlaying) return;
        Init();
        SetAttribute();
        Refresh();
    }


#endif

    public override void Init()
    {
        base.Init();
        InitComponent();

        _points = chartRectTrs.Find("Points");
        _drawLinesPanel = chartRectTrs.Find("DrawLinesPanel");
        _drawLineGraph = _drawLinesPanel.GetOrAddComponent<DrawLineGraph>();

        if (!tip)
        {
            tip = chartRectTrs.Find("Tip");
        }

        tip.gameObject.SetActive(false);
    }

    public override void SetAttribute()
    {
        base.SetAttribute();
    }


    /// <summary>
    /// 设置柱图样式
    /// </summary>
    public override void Refresh()
    {
        base.Refresh();
        // chart.ClearData();

        switch (xAxisType)
        {
            case ChartAxisType.类目:
                ResetData();
                break;
            case ChartAxisType.数值:
                ResetValueData();
                break;
        }
    }


    private void ResetData()
    {
        xAxis0.type = Axis.AxisType.Category;
        xAxis0.axisTick.alignWithLabel = true;

        for (var i = 0; i < _points.childCount; i++)
        {
            _points.GetChild(i).gameObject.SetActive(false);
        }

        #region 自动设置Y轴最大值最小值

        if (yAxisAtb0.isAutomatic)
        {
            var positives0 = (from item in dataCategory
                where !item.isUseRightYAxis
                from itemValue in item.values
                where itemValue.value >= 0
                select itemValue.value).ToList();

            if (positives0.Count > 0)
            {
                yAxisAtb0.axisMax = MaxDivisibleValue.GetMaxDivisibleValue(positives0.Max());
            }

            var negatives0 = (from item in dataCategory
                where !item.isUseRightYAxis
                from itemValue in item.values
                where itemValue.value < 0
                select itemValue.value).ToList();
            if (negatives0.Count > 0)
            {
                yAxisAtb0.axisMin = -(float)MaxDivisibleValue.GetMaxDivisibleValue(Mathf.Abs(negatives0.Min()));
            }
            else
            {
                if (yAxisAtb0.axisMin < 0)
                {
                    yAxisAtb0.axisMin = 0;
                }
            }
        }

        yAxis0.max = yAxisAtb0.axisMax;
        yAxis0.min = yAxisAtb0.axisMin;


        if (yAxisAtb1.isAutomatic)
        {
            var positives1 = (from item in dataCategory
                where item.isUseRightYAxis
                from itemValue in item.values
                where itemValue.value >= 0
                select itemValue.value).ToList();
            if (positives1.Count > 0)
            {
                yAxisAtb1.axisMax = MaxDivisibleValue.GetMaxDivisibleValue(positives1.Max());
            }

            var negatives1 = (from item in dataCategory
                where item.isUseRightYAxis
                from itemValue in item.values
                where itemValue.value < 0
                select itemValue.value).ToList();
            if (negatives1.Count > 0)
            {
                yAxisAtb1.axisMin = -(float)MaxDivisibleValue.GetMaxDivisibleValue(Mathf.Abs(negatives1.Min()));
            }
            else
            {
                if (yAxisAtb1.axisMin < 0)
                {
                    yAxisAtb1.axisMin = 0;
                }
            }
        }

        yAxis1.max = yAxisAtb1.axisMax;
        yAxis1.min = yAxisAtb1.axisMin;

        #endregion


        for (var i = 0; i < dataCategory.Count; i++)
        {
            if (lineAttributeList.Count <= i)
            {
                lineAttributeList.Add(new LineAttribute());
            }

            var lineAttribute = lineAttributeList[i];
            lineAttribute.items.Clear();
            lineAttribute.zeroPos = -chartSize.y * 0.5f + YAxisNegativeLenght((float)yAxisAtb0.axisMax, yAxisAtb0.axisMin);

            var lineData = dataCategory[i];
            for (var j = 0; j < lineData.values.Count; j++)
            {
                var item = lineData.values[j];
                if (i == 0)
                {
                    xAxis0.AddData(item.key);
                }

                var pointPos = PointPos(new Vector2(j + 0.5f, item.value),
                    lineData.isUseRightYAxis ? (float)yAxisAtb1.axisMax : (float)yAxisAtb0.axisMax,
                    dataCategory[0].values.Count,
                    lineData.isUseRightYAxis ? yAxisAtb1.axisMin : yAxisAtb0.axisMin, 0);

                if (lineAttribute.isShowPoint)
                {
                    var point = _points.Find("Point" + i + "_" + j);
                    if (!point)
                    {
                        point = new GameObject("Point" + i + "_" + j).AddComponent<RectTransform>();
                        point.transform.parent = _points;
                    }

                    point.gameObject.SetActive(true);

                    var pointImage = point.GetOrAddComponent<Image>();
                    pointImage.sprite = lineAttribute.pointSprite;
                    pointImage.color = lineAttribute.pointColor;
                    var pointRect = point.GetComponent<RectTransform>();
                    pointRect.sizeDelta = lineAttribute.pointSize;
                    point.localScale = Vector3.one;
                    point.localPosition = pointPos + (Vector3)lineAttribute.pointOffset;

                    if (isShowTip)
                    {
                        pointImage.OnPointerEnterAsObservable().Subscribe(_ =>
                        {
                            tip.gameObject.SetActive(true);
                            tip.position = point.position + (Vector3)tipOffset;
                            setTipActionCategory?.Invoke(item, i, j, tip);
                        });
                        pointImage.OnPointerExitAsObservable().Subscribe(_ => tip.gameObject.SetActive(false));
                    }
                }

                if (lineAttribute.isShowPointValue)
                {
                    var label = _points.Find("Label" + i + "_" + j);
                    if (!label)
                    {
                        label = new GameObject("Label" + i + "_" + j).AddComponent<RectTransform>();
                        label.transform.parent = _points;
                    }

                    label.gameObject.SetActive(true);

                    var labelText = label.GetOrAddComponent<TextMeshProUGUI>();
                    labelText.font = lineAttribute.textFont;
                    labelText.color = lineAttribute.fontColor;
                    labelText.fontSize = lineAttribute.fontSize;
                    labelText.alignment = TextAlignmentOptions.Center;
                    labelText.text = item.value.ToString();
                    var labelRect = labelText.GetComponent<RectTransform>();
                    labelRect.sizeDelta = lineAttribute.textRectSize;
                    label.localPosition = pointPos + (Vector3)lineAttribute.textOffset;
                    setLabelActionCategory?.Invoke(item, i, j, labelText, labelRect);
                }
            }

            #region 设置线路点位

            var newLineData = lineData.values.Select((t, i) => new Vector2(i + 0.5f, t.value)).ToList();
            if (lineAttribute.isSmoothCurve)
            {
                newLineData = ConvertLineToCurve(newLineData, lineAttribute.segmentsPerSegment, lineAttribute.alpha);
            }

            foreach (var item in newLineData)
            {
                var pointPos = PointPos(item,
                    lineData.isUseRightYAxis ? (float)yAxisAtb1.axisMax : (float)yAxisAtb0.axisMax,
                    dataCategory[0].values.Count,
                    lineData.isUseRightYAxis ? yAxisAtb1.axisMin : yAxisAtb0.axisMin,
                    xAxisAtb.axisMin);


                lineAttribute.items.Add(pointPos);
            }

            #endregion


            _drawLineGraph.lineAttributes = lineAttributeList;
        }

        _drawLineGraph.RedrawMesh();
    }

    private void ResetValueData()
    {
        xAxis0.type = isTime ? Axis.AxisType.ValueTime : Axis.AxisType.Value;
        xAxis0.minMaxType = Axis.AxisMinMaxType.Custom;
        xAxis0.axisTick.alignWithLabel = false;

        for (var i = 0; i < _points.childCount; i++)
        {
            _points.GetChild(i).gameObject.SetActive(false);
        }

        #region 自动设置Y轴最大值最小值

        if (yAxisAtb0.isAutomatic)
        {
            var positivesY = (from item in dataValue where !item.isUseRightYAxis from itemValue in item.values where itemValue.y >= 0 select itemValue.y)
                .ToList();
            if (positivesY.Count > 0)
            {
                yAxisAtb0.axisMax = MaxDivisibleValue.GetMaxDivisibleValue(positivesY.Max());
            }

            var negativesY = (from item in dataValue where !item.isUseRightYAxis from itemValue in item.values where itemValue.y < 0 select itemValue.y)
                .ToList();
            if (negativesY.Count > 0)
            {
                yAxisAtb0.axisMin = -(float)MaxDivisibleValue.GetMaxDivisibleValue(Mathf.Abs(negativesY.Min()));
            }
            else
            {
                if (yAxisAtb0.axisMin < 0)
                {
                    yAxisAtb0.axisMin = 0;
                }
            }
        }

        if (yAxisAtb1.isAutomatic)
        {
            var positives1 = (from item in dataValue where item.isUseRightYAxis from itemValue in item.values where itemValue.y >= 0 select itemValue.y)
                .ToList();
            if (positives1.Count > 0)
            {
                yAxisAtb1.axisMax = MaxDivisibleValue.GetMaxDivisibleValue(positives1.Max());
            }

            var negatives1 = (from item in dataValue where item.isUseRightYAxis from itemValue in item.values where itemValue.y < 0 select itemValue.y)
                .ToList();
            if (negatives1.Count > 0)
            {
                yAxisAtb1.axisMin = -(float)MaxDivisibleValue.GetMaxDivisibleValue(Mathf.Abs(negatives1.Min()));
            }
            else
            {
                if (yAxisAtb1.axisMin < 0)
                {
                    yAxisAtb1.axisMin = 0;
                }
            }
        }


        if (xAxisAtb.isAutomatic)
        {
            var positivesX = (from item in dataValue from itemValue in item.values where itemValue.x >= 0 select itemValue.x).ToList();
            if (positivesX.Count > 0)
            {
                xAxisAtb.axisMax = MaxDivisibleValue.GetMaxDivisibleValue(positivesX.Max());
            }


            var negativesX = (from item in dataValue from itemValue in item.values where itemValue.x < 0 select itemValue.x).ToList();
            if (negativesX.Count > 0)
            {
                xAxisAtb.axisMin = -(float)MaxDivisibleValue.GetMaxDivisibleValue(Mathf.Abs(negativesX.Min()));
            }
            else
            {
                if (xAxisAtb.axisMin < 0)
                {
                    xAxisAtb.axisMin = 0;
                }
            }
        }


        yAxis0.max = yAxisAtb0.axisMax;
        yAxis0.min = yAxisAtb0.axisMin;
        yAxis1.max = yAxisAtb1.axisMax;
        yAxis1.min = yAxisAtb1.axisMin;
        xAxis0.max = xAxisAtb.axisMax;
        xAxis0.min = xAxisAtb.axisMin;

        #endregion

        for (var i = 0; i < dataValue.Count; i++)
        {
            if (lineAttributeList.Count <= i)
            {
                lineAttributeList.Add(new LineAttribute());
            }

            var lineAttribute = lineAttributeList[i];
            lineAttribute.items.Clear();
            lineAttribute.zeroPos = -chartSize.y * 0.5f + YAxisNegativeLenght((float)yAxisAtb0.axisMax, yAxisAtb0.axisMin);
            var lineData = dataValue[i];
            for (var j = 0; j < lineData.values.Count; j++)
            {
                var item = lineData.values[j];
                var pointPos = PointPos(item,
                    lineData.isUseRightYAxis ? (float)yAxisAtb1.axisMax : (float)yAxisAtb0.axisMax,
                    (float)xAxisAtb.axisMax,
                    lineData.isUseRightYAxis ? yAxisAtb1.axisMin : yAxisAtb0.axisMin,
                    xAxisAtb.axisMin);

                if (lineAttribute.isShowPoint)
                {
                    var point = _points.Find("Point" + i + "_" + j);
                    if (!point)
                    {
                        point = new GameObject("Point" + i + "_" + j).AddComponent<RectTransform>();
                        point.transform.parent = _points;
                    }

                    point.gameObject.SetActive(true);

                    var pointImage = point.GetOrAddComponent<Image>();
                    pointImage.sprite = lineAttribute.pointSprite;
                    pointImage.color = lineAttribute.pointColor;
                    var pointRect = point.GetComponent<RectTransform>();
                    pointRect.sizeDelta = lineAttribute.pointSize;
                    point.localScale = Vector3.one;
                    point.localPosition = pointPos + (Vector3)lineAttribute.pointOffset;

                    if (isShowTip)
                    {
                        pointImage.OnPointerEnterAsObservable().Subscribe(_ =>
                        {
                            tip.gameObject.SetActive(true);
                            tip.position = point.position + (Vector3)tipOffset;
                            setTipActionValue?.Invoke(item, i, j, tip);
                        });
                        pointImage.OnPointerExitAsObservable().Subscribe(_ => tip.gameObject.SetActive(false));
                    }
                }

                if (lineAttribute.isShowPointValue)
                {
                    var label = _points.Find("Label" + i + "_" + j);
                    if (!label)
                    {
                        label = new GameObject("Label" + i + "_" + j).AddComponent<RectTransform>();
                        label.transform.parent = _points;
                    }

                    label.gameObject.SetActive(true);

                    var labelText = label.GetOrAddComponent<TextMeshProUGUI>();
                    labelText.font = lineAttribute.textFont;
                    labelText.color = lineAttribute.fontColor;
                    labelText.fontSize = lineAttribute.fontSize;
                    labelText.alignment = TextAlignmentOptions.Center;
                    labelText.text = item.y.ToString();
                    var labelRect = labelText.GetComponent<RectTransform>();
                    labelRect.sizeDelta = lineAttribute.textRectSize;
                    label.localPosition = pointPos + (Vector3)lineAttribute.textOffset;
                    setLabelActionValue?.Invoke(item, i, j, labelText, labelRect);
                }

                // lineAttribute.items.Add(pointPos);
            }

            #region 设置线路点位

            // 转换为曲线数据
            var newLineData = new List<Vector2>();
            newLineData = lineAttribute.isSmoothCurve ? ConvertLineToCurve(lineData.values, lineAttribute.segmentsPerSegment, lineAttribute.alpha) : lineData.values;
            foreach (var item in newLineData)
            {
                var pointPos = PointPos(item,
                    lineData.isUseRightYAxis ? (float)yAxisAtb1.axisMax : (float)yAxisAtb0.axisMax,
                    (float)xAxisAtb.axisMax,
                    lineData.isUseRightYAxis ? yAxisAtb1.axisMin : yAxisAtb0.axisMin,
                    xAxisAtb.axisMin);


                lineAttribute.items.Add(pointPos);
            }

            #endregion

            _drawLineGraph.lineAttributes = lineAttributeList;
        }

        _drawLineGraph.RedrawMesh();
    }

    #region 插值计算，将折线转为曲线

    private static List<Vector2> ConvertLineToCurve(List<Vector2> linePoints, int segmentsPerSegment = 10, float alpha = 0.5f)
    {
        if (segmentsPerSegment < 2) segmentsPerSegment = 2;

        var curvePoints = new List<Vector2>();

        // 对于每一段线段
        for (var i = 0; i < linePoints.Count - 1; i++)
        {
            // Catmull-Rom样条需要四个点：前一个点，当前点，后一个点，再后一个点
            // 对于首尾点，我们使用重复的首尾点来模拟闭合或开放的曲线
            var p0 = i == 0 ? linePoints[0] : linePoints[i - 1];
            var p1 = linePoints[i];
            var p2 = linePoints[i + 1];
            var p3 = i == linePoints.Count - 2 ? linePoints[linePoints.Count - 1] : linePoints[i + 2];

            // 在这段线段上生成多个点
            for (var t = 0f; t <= 1f; t += 1f / segmentsPerSegment)
            {
                curvePoints.Add(CatmullRom(p0, p1, p2, p3, t, alpha));
            }
        }

        return curvePoints;
    }

    private static Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t, float alpha = 0.5f)
    {
        // var tSquared = t * t;
        // var tCubed = tSquared * t;
        //
        // var result =
        //     0.5f * (
        //         (2f * p1) +
        //         (-p0 + p2) * t +
        //         (2f * p0 - 5f * p1 + 4f * p2 - p3) * tSquared +
        //         (-p0 + 3f * p1 - 3f * p2 + p3) * tCubed
        //     );
        //
        // return result;


        var dt0 = Mathf.Pow(Vector2.Distance(p0, p1), alpha);
        var dt1 = Mathf.Pow(Vector2.Distance(p1, p2), alpha);
        var dt2 = Mathf.Pow(Vector2.Distance(p2, p3), alpha);

        var m1 = (p2 - p0) / (dt0 + dt1);
        var m2 = (p3 - p1) / (dt1 + dt2);

        var a = 2f * (p1 - p2) + m1 + m2;
        var b = -3f * (p1 - p2) - 2f * m1 - m2;

        return p1 + t * (m1 + t * (b + t * a));
    }

    #endregion
}