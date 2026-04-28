// ========================================================
// 描 述：
// 作 者：SW
// 创建时间：2024/01/18 14:36:03
// 版 本：v 1.0
// ========================================================

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using XCharts.Runtime;


namespace Prospect
{
    [Serializable]
    public class GraphAxisBase : ComponentRoot
    {
        [HideInInspector] public RectTransform thisRect;
        [HideInInspector] public RectTransform chartRectTrs;
        [HideInInspector] public Vector2 chartSize;
        [HideInInspector] public BaseChart chart;

        [Header("背景图片：")] public ChartBgImage chartBgImage;

        [HideInInspector] public XAxis xAxis0;
        [HideInInspector] public YAxis yAxis0;
        [HideInInspector] public YAxis yAxis1;


        [Header("X轴："), Tooltip("设置X轴的数量")] public ChartAxisCount chartAxisCountX = ChartAxisCount.单轴;
        public ChartAxisAttribute xAxisAtb = new();
        [Header("Y1轴："), Tooltip("设置Y轴的数量")] public ChartAxisCount chartAxisCountY = ChartAxisCount.单轴;
        public ChartAxisAttribute yAxisAtb0 = new();
        [Header("Y2轴：")] public ChartAxisAttribute yAxisAtb1 = new();


        public void InitComponent()
        {
            thisRect = this.GetComponent<RectTransform>();
            chartRectTrs = transform.Find("Chart").GetComponent<RectTransform>();
            chartSize = GetRectSize(chartRectTrs);
            chart = chartRectTrs.GetComponent<BaseChart>();


            xAxis0 = chart.EnsureChartComponent<XAxis>();
            yAxis0 = chart.GetChartComponent<YAxis>(0);
            yAxis1 = chart.GetChartComponent<YAxis>(1);


            chartBgImage.bgImageRect = this.transform.Find("Bg_Image").GetComponent<RectTransform>();
            chartBgImage.bgImage = chartBgImage.bgImageRect.GetComponent<Image>();

            xAxisAtb.frame = chartBgImage.bgImageRect.Find("TopFrame").GetComponent<RectTransform>();
            xAxisAtb.axisLineWidth = xAxisAtb.axisLineWidth == 0 ? 0.8f : xAxisAtb.axisLineWidth;

            yAxisAtb0.frame = chartBgImage.bgImageRect.Find("RightFrame").GetComponent<RectTransform>();
            yAxisAtb0.axisLineWidth = yAxisAtb0.axisLineWidth == 0 ? 0.8f : yAxisAtb0.axisLineWidth;
            yAxisAtb1.axisLineWidth = yAxisAtb1.axisLineWidth == 0 ? 0.8f : yAxisAtb1.axisLineWidth;


            SetBgImage();
            SetXAxis();
            SetYAxis();
        }

        /// <summary>
        /// 设置背景图片的大小、位置、Sprite
        /// </summary>
        private void SetBgImage()
        {
            chartBgImage.bgImageRect.gameObject.SetActive(chartBgImage.isShowBgImage);

            if (!chartBgImage.isShowBgImage) return;
            chartBgImage.bgImage.color = chartBgImage.bgImageColor;
            chartBgImage.bgImage.sprite = chartBgImage.bgImageSprite;
            var bgSizeX = chartSize.x + (chartAxisCountY == ChartAxisCount.双轴 ? 0 : xAxisAtb.axisLineOffset);
            var bgSizeY = chartSize.y + chartBgImage.bgImageOffset + yAxisAtb0.axisLineOffset;
            chartBgImage.bgImageRect.sizeDelta = new Vector2(bgSizeX, bgSizeY);

            var bgPosX = bgSizeX / 2f - chartSize.x * 0.5f + yAxisAtb0.axisLineWidth;
            var bgPosY = bgSizeY / 2f - chartSize.y * 0.5f + xAxisAtb.axisLineWidth;
            chartBgImage.bgImageRect.localPosition = new Vector3(bgPosX, bgPosY, 0) + chartRectTrs.localPosition;
        }

        #region X轴设置

        /// <summary>
        /// 设置X轴
        /// </summary>
        private void SetXAxis()
        {
            SetXUnit();

            switch (chartAxisCountX)
            {
                case ChartAxisCount.单轴:
                    xAxis0.axisLine.lineStyle.color = xAxisAtb.axisLineColor;
                    xAxis0.axisTick.lineStyle.color = xAxisAtb.axisLineColor;
                    xAxis0.axisLabel.textStyle.color = xAxisAtb.axisLabelColor;
                    break;
                case ChartAxisCount.双轴:
                    break;
            }

            if (xAxisAtb.isShowAxis)
            {
                xAxis0.axisLine.lineStyle.color = xAxisAtb.axisLineColor;
                xAxis0.axisTick.lineStyle.color = xAxisAtb.axisLineColor;
                xAxis0.axisLabel.textStyle.color = xAxisAtb.axisLabelColor;
            }
            else
            {
                xAxis0.axisLine.lineStyle.color = new Color(1, 1, 1, 0);
                xAxis0.axisTick.lineStyle.color = new Color(1, 1, 1, 0);
                xAxis0.axisLabel.textStyle.color = new Color(1, 1, 1, 0);
            }


            xAxis0.axisLine.showArrow = true;
            xAxis0.axisLine.arrow.width = 0;
            xAxis0.axisLine.arrow.height = 0;
            xAxis0.axisLine.arrow.offset = xAxisAtb.axisLineOffset;
            xAxis0.axisLine.arrow.dent = 0;

            xAxis0.splitNumber = xAxisAtb.axisTickCount;
            xAxis0.max = xAxisAtb.axisMax;
            xAxis0.min = xAxisAtb.axisMin;

            xAxis0.axisLine.lineStyle.width = xAxisAtb.axisLineWidth;
            xAxis0.axisTick.lineStyle.width = xAxisAtb.axisTickSize.x;
            xAxis0.axisTick.lineStyle.length = xAxisAtb.axisTickSize.y;
            xAxis0.axisTick.showStartTick = xAxisAtb.isShowStartTick;
            xAxis0.axisLabel.showStartLabel = xAxisAtb.isShowStartTick;
            xAxis0.axisLabel.textStyle.tmpFont = xAxisAtb.axisLabelFont;
            xAxis0.axisLabel.textStyle.fontSize = xAxisAtb.axisLabelSize;
            xAxis0.axisLabel.formatter = xAxisAtb.axisLabelFormatter;
            xAxis0.axisLabel.offset = xAxisAtb.axisLabelOffset;

            if (xAxisAtb.isShowFrame)
            {
                xAxisAtb.frame.gameObject.SetActive(true);
                xAxisAtb.frame.sizeDelta = new Vector2(xAxisAtb.frame.sizeDelta.x, xAxisAtb.axisLineWidth * 2);
                xAxisAtb.frame.GetComponent<Image>().color = xAxisAtb.axisLineColor;
            }
            else
            {
                xAxisAtb.frame.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 设置X轴单位
        /// </summary>
        private void SetXUnit()
        {
            xAxis0.axisName.show = xAxisAtb.isShowUnit;

            if (!xAxisAtb.isShowUnit) return;
            xAxis0.axisName.name = xAxisAtb.unitName;
            xAxis0.axisName.labelStyle.offset = xAxisAtb.unitOffset;
            xAxis0.axisName.labelStyle.textStyle.tmpFont = xAxisAtb.unitFont;
            xAxis0.axisName.labelStyle.textStyle.fontSize = xAxisAtb.unitFontSize;
            xAxis0.axisName.labelStyle.textStyle.color = xAxisAtb.unitFontColor;
        }

        #endregion

        #region Y轴设置

        private void SetYAxisAttribute(YAxis axis, ChartAxisAttribute axisAtb)
        {
            axis.axisLine.showArrow = true;
            axis.axisLine.arrow.width = 0;
            axis.axisLine.arrow.height = 0;
            axis.axisLine.arrow.offset = axisAtb.axisLineOffset;
            axis.axisLine.arrow.dent = 0;

            axis.splitNumber = axisAtb.axisTickCount;
            axis.max = axisAtb.axisMax;
            axis.min = axisAtb.axisMin;

            axis.axisLine.lineStyle.width = axisAtb.axisLineWidth;
            axis.axisTick.lineStyle.width = axisAtb.axisTickSize.x;
            axis.axisTick.lineStyle.length = axisAtb.axisTickSize.y;
            axis.axisTick.showStartTick = axisAtb.isShowStartTick;
            axis.axisLabel.showStartLabel = axisAtb.isShowStartTick;
            axis.axisLabel.textStyle.tmpFont = axisAtb.axisLabelFont;
            axis.axisLabel.textStyle.fontSize = axisAtb.axisLabelSize;
            axis.axisLabel.formatter = axisAtb.axisLabelFormatter;
            axis.axisLabel.offset = axisAtb.axisLabelOffset;
        }


        /// <summary>
        /// 设置Y轴
        /// </summary>
        private void SetYAxis()
        {
            switch (chartAxisCountY)
            {
                case ChartAxisCount.单轴:
                    yAxis1.show = false;
                    SetYAxisAttribute(yAxis0, yAxisAtb0);
                    SetYUnit(yAxis0, yAxisAtb0);
                    break;
                case ChartAxisCount.双轴:
                    yAxis1.show = true;
                    xAxis0.axisLine.arrow.offset = 0;
                    yAxisAtb0.isShowFrame = false;
                    yAxisAtb1.isShowFrame = false;
                    SetYAxisAttribute(yAxis0, yAxisAtb0);
                    SetYAxisAttribute(yAxis1, yAxisAtb1);
                    SetYUnit(yAxis0, yAxisAtb0);
                    SetYUnit(yAxis1, yAxisAtb1);
                    break;
            }


            #region 设置轴是否可见

            if (yAxisAtb0.isShowAxis)
            {
                yAxis0.axisLine.lineStyle.color = yAxisAtb0.axisLineColor;
                yAxis0.axisTick.lineStyle.color = yAxisAtb0.axisLineColor;
                yAxis0.axisLabel.textStyle.color = yAxisAtb0.axisLabelColor;
            }
            else
            {
                yAxis0.axisLine.lineStyle.color = new Color(1, 1, 1, 0);
                yAxis0.axisTick.lineStyle.color = new Color(1, 1, 1, 0);
                yAxis0.axisLabel.textStyle.color = new Color(1, 1, 1, 0);
            }

            if (yAxisAtb1.isShowAxis)
            {
                yAxis1.axisLine.lineStyle.color = yAxisAtb1.axisLineColor;
                yAxis1.axisTick.lineStyle.color = yAxisAtb1.axisLineColor;
                yAxis1.axisLabel.textStyle.color = yAxisAtb1.axisLabelColor;
            }
            else
            {
                yAxis1.axisLine.lineStyle.color = new Color(1, 1, 1, 0);
                yAxis1.axisTick.lineStyle.color = new Color(1, 1, 1, 0);
                yAxis1.axisLabel.textStyle.color = new Color(1, 1, 1, 0);
            }

            #endregion

            if (yAxisAtb0.isShowFrame)
            {
                yAxisAtb0.frame.gameObject.SetActive(true);
                yAxisAtb0.frame.sizeDelta = new Vector2(yAxisAtb0.axisLineWidth * 2, yAxisAtb0.frame.sizeDelta.y);
                yAxisAtb0.frame.GetComponent<Image>().color = yAxisAtb0.axisLineColor;
            }
            else
            {
                yAxisAtb0.frame.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 设置Y0轴单位
        /// </summary>
        private void SetYUnit(YAxis axis, ChartAxisAttribute axisAtb)
        {
            axis.axisName.show = axisAtb.isShowUnit;

            if (!axisAtb.isShowUnit) return;
            axis.axisName.name = axisAtb.unitName;
            axis.axisName.labelStyle.offset = axisAtb.unitOffset;
            axis.axisName.labelStyle.textStyle.tmpFont = axisAtb.unitFont;
            axis.axisName.labelStyle.textStyle.fontSize = axisAtb.unitFontSize;
            axis.axisName.labelStyle.textStyle.color = axisAtb.unitFontColor;
        }

        #endregion

        #region char坐标转vector3D

        public Vector3 PointPos(Vector2 value, float yAxisMax, float xAxisMax, float yAxisMin = 0, float xAxisMin = 0)
        {
            var xLenght = ValueLenghtX(value.x, xAxisMax, xAxisMin);
            var yLenght = ValueLenghtY(value.y, yAxisMax, yAxisMin);

            var xPos = 0f;
            var yPos = 0f;

            if (xAxisMin >= 0)
            {
                xPos = xLenght - chartSize.x * 0.5f;
            }
            else
            {
                if (value.x > 0)
                {
                    xPos = XAxisNegativeLenght(xAxisMax, xAxisMin) + xLenght - chartSize.x * 0.5f;
                }
                else
                {
                    xPos = XAxisNegativeLenght(xAxisMax, xAxisMin) - xLenght - chartSize.x * 0.5f;
                }
            }

            if (yAxisMin >= 0)
            {
                yPos = yLenght - chartSize.y * 0.5f;
            }
            else
            {
                if (value.y > 0)
                {
                    yPos = YAxisNegativeLenght(yAxisMax, yAxisMin) + yLenght - chartSize.y * 0.5f;
                }
                else
                {
                    yPos = YAxisNegativeLenght(yAxisMax, yAxisMin) - yLenght - chartSize.y * 0.5f;
                }
            }

            return new Vector3(xPos, yPos, 0);
        }


        /// <summary>
        /// 根据y值获取相对于Chart的局部坐标
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="xAxisMax">x轴最大值</param>
        /// <param name="xAxisMin">x轴最小值</param>
        /// <returns></returns>
        public float ValueLenghtX(float xValue, float xAxisMax, float xAxisMin = 0)
        {
            if (xAxisMin >= 0)
            {
                return Mathf.Abs(chartSize.x * ValuePercent(xValue, xAxisMax, xAxisMin));
            }

            return Mathf.Abs(chartSize.x * ((xValue > 0 ? xAxisMax : xAxisMin) / (xAxisMax - xAxisMin)) * ValuePercent(xValue, xAxisMax, xAxisMin));
        }


        /// <summary>
        /// 根据y值获取相对于Chart的局部坐标
        /// </summary>
        /// <param name="yValue"></param>
        /// <param name="yAxisMax">y轴最大值</param>
        /// <param name="yAxisMin">y轴最小值</param>
        /// <returns></returns>
        public float ValueLenghtY(float yValue, float yAxisMax, float yAxisMin = 0)
        {
            if (yAxisMin >= 0)
            {
                return Mathf.Abs(chartSize.y * ValuePercent(yValue, yAxisMax, yAxisMin));
            }

            return Mathf.Abs(chartSize.y * ((yValue > 0 ? yAxisMax : yAxisMin) / (yAxisMax - yAxisMin)) * ValuePercent(yValue, yAxisMax, yAxisMin));
        }

        /// <summary>
        /// 根据y值获取相对于Chart正负方向的百分比
        /// </summary>
        /// <param name="value"></param>
        /// <param name="xisMax">y轴最大值</param>
        /// <param name="xisMin">y轴最小值</param>
        /// <returns></returns>
        public float ValuePercent(float value, float xisMax, float xisMin = 0)
        {
            if (xisMin >= 0)
            {
                value -= xisMin;
                value = value > 0 ? value : 0;
                return Mathf.Abs(1 / (xisMax - xisMin) * value);
            }

            return Mathf.Abs(1 / (value > 0 ? xisMax : xisMin) * value);
        }

        #endregion

        #region 轴正负方向长度

        /// <summary>
        /// X轴正方向长度
        /// </summary>
        /// <param name="xAxisMax"></param>
        /// <param name="xAxisMin"></param>
        /// <returns></returns>
        public float XAxisPositiveLenght(float xAxisMax, float xAxisMin = 0)
        {
            return Mathf.Abs(chartSize.x * xAxisMax / (xAxisMax - xAxisMin));
        }

        /// <summary>
        /// X轴负方向长度
        /// </summary>
        /// <param name="xAxisMax"></param>
        /// <param name="xAxisMin"></param>
        /// <returns></returns>
        public float XAxisNegativeLenght(float xAxisMax, float xAxisMin = 0)
        {
            return Mathf.Abs(chartSize.x * xAxisMin / (xAxisMax - xAxisMin));
        }

        /// <summary>
        /// Y轴正方向长度
        /// </summary>
        /// <param name="yAxisMax"></param>
        /// <param name="yAxisMin"></param>
        /// <returns></returns>
        public float YAxisPositiveLenght(float yAxisMax, float yAxisMin = 0)
        {
            return Mathf.Abs(chartSize.y * yAxisMax / (yAxisMax - yAxisMin));
        }

        /// <summary>
        /// Y轴负方向长度
        /// </summary>
        /// <param name="yAxisMax"></param>
        /// <param name="yAxisMin"></param>
        /// <returns></returns>
        public float YAxisNegativeLenght(float yAxisMax, float yAxisMin = 0)
        {
            return Mathf.Abs(chartSize.y * yAxisMin / (yAxisMax - yAxisMin));
        }

        #endregion

        /// <summary>
        /// 获取Rect的大小
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Vector2 GetRectSize(RectTransform rect)
        {
            var size = rect.sizeDelta;
            if (rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.one)
            {
                size = rect.parent.GetComponent<RectTransform>().sizeDelta;
            }

            return size;
        }
    }


    [Serializable]
    public class ChartBgImage
    {
        [Tooltip("是否显示背景图片")] public bool isShowBgImage = true;
        [HideInInspector] public RectTransform bgImageRect;
        [HideInInspector] public Image bgImage;
        [Tooltip("背景图片Sprite")] public Sprite bgImageSprite;
        [Tooltip("背景图片颜色")] public Color bgImageColor = Color.grey;
        [Tooltip("背景图片向上延伸量")] public float bgImageOffset = 0;
    }

    /// <summary>
    /// 轴属性
    /// </summary>
    [Serializable]
    public class ChartAxisAttribute
    {
        [Header("    轴线："), Tooltip("是否显示轴")] public bool isShowAxis = true;

        [Tooltip("X轴顶端向外伸出多少：Axis——Axis Line——Arrow——Offset")]
        public float axisLineOffset;

        [Tooltip("轴颜色：Axis——Axis Line——Line Style——Color")]
        public Color axisLineColor = Color.white;

        [Tooltip("轴宽度：Axis——Axis Line——Line Style——Width")]
        public float axisLineWidth = 0.8f;

        #region 轴刻度

        [Space(-10), Header("    轴刻度："), Tooltip("轴刻度宽度：YAxis——Axis Tick——Line Style——Width")]
        public Vector2 axisTickSize = Vector2.zero;

        [Tooltip("轴刻度段数：Axis——SplitNumber")] public int axisTickCount = 5;


        [Tooltip("是否自动调节轴刻度：\n注意：isAutomatic=false不再自动调节最值")]
        public bool isAutomatic = true;

        [Tooltip("轴刻度最大值：Axis——Min Min Type——Max\n注意：isAutomatic=true时，自动计算最大值，不可手动调节")]
        public double axisMax = 5;

        [Tooltip("轴刻度最小值：Axis——Min Min Type——Min\n注意：isAutomatic=true时,并且数据中有负值时可以自动计算最小值")]
        public float axisMin = 0;

        [Tooltip("是否显示轴起始刻度：Axis——Min Min Type——Min\n注意：isAutomatic=true时,并且数据中有负值时可以自动计算最小值")]
        public bool isShowStartTick = true;

        [Space(-10), Header("    轴刻度文字："), Tooltip("轴刻度文字字体：XAxis——Axis Label——Text Style——TMP Font")]
        public TMP_FontAsset axisLabelFont;

        [Tooltip("轴刻度文字大小：Axis——Axis Label——Text Style——Font Size")]
        public int axisLabelSize = 20;

        [Tooltip("轴刻度文字颜色：Axis——Axis Label——Text Style——Color")]
        public Color axisLabelColor = Color.white;

        [Tooltip("轴刻度文字Formatter：Axis——Axis Label——Formatter")]
        public string axisLabelFormatter; //{value:f0}%

        [Tooltip("轴刻度文字偏移：Axis——Axis Label——Offset")]
        public Vector3 axisLabelOffset;

        #endregion


        #region 轴单位

        [Space(-10), Header("    轴单位：")] public bool isShowUnit = true;

        [Tooltip("轴单位名称：Axis——Axis Name——Name")]
        public string unitName = "轴名称(单位)";

        [Tooltip("轴单位偏移量：Axis——Axis Name——Label Style——Offset")]
        public Vector2 unitOffset = new(12, 53);

        [Tooltip("轴单位字体：Axis——Axis Name——Label Style——Text Style——TMP Font")]
        public TMP_FontAsset unitFont;

        [Tooltip("轴单位字体大小：Axis——Axis Name——Label Style——Text Style——Font Size")]
        public int unitFontSize = 20;

        [Tooltip("轴单位字体颜色：Axis——Axis Name——Label Style——Text Style——Auto Color ")]
        public Color unitFontColor = Color.white;

        #endregion

        [Space(-10), Header("    轴边框：")] public bool isShowFrame;
        [HideInInspector] public RectTransform frame;
    }
}