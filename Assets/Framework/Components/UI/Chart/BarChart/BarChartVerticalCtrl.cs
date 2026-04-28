// ========================================================
// 描 述：柱形图——1
// 作 者：SW
// 创建时间：2023/10/11 09:21:31
// 版 本：v 1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using SW;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;
using Image = UnityEngine.UI.Image;


namespace Prospect
{
    public class BarChartVerticalCtrl : GraphAxisBase
    {
        /// <summary>
        /// 组名，组索引，单柱索引,tipTrs
        /// </summary>
        public Action<string, int, int, Transform> setTipActionCategory;

        /// <summary>
        /// 组名，组索引，单柱索引，背景图片，顶部图片，底部图片，数值文本
        /// </summary>
        public Action<string, int, int, Image, Image, Image, TMP_Text> setBarActionCategory;


        /// <summary>
        /// X轴数值，组索引，单柱索引,tipTrs
        /// </summary>
        public Action<float, int, int, Transform> setTipActionValue;

        /// <summary>
        /// X轴数值，组索引，单柱索引，背景图片，顶部图片，底部图片，数值文本
        /// </summary>
        public Action<float, int, int, Image, Image, Image, TMP_Text> setBarActionValue;


        #region 柱子

        [Space(20), Header("柱子："), Tooltip("渐变方式")]
        public GradualChangeMode gradualChangeMode = GradualChangeMode.单独渐变;

        private Transform _bars;
        private GameObject _egGo;

        [Tooltip("同组柱间距")] public float spacing;
        [Tooltip("柱子向上偏移量")] public float offset = 0.8f;
        [Tooltip("柱形图样式")] public BarChartType barChartType = BarChartType.样式_2D;

        [Tooltip("柱的样式Sprite")] public Sprite barSprite;
        [Tooltip("柱的颜色")] public Color barColor = Color.white;
        [Tooltip("柱整体宽度")] public float barWidth = 20;
        [Tooltip("柱的Image类型")] public Image.Type barImageType = Image.Type.Simple;
        [Tooltip("柱的像素单位倍率")] public float barPixelsPerUnitMultiplier = 1;


        [Tooltip("柱顶部的图片")] public Sprite barTopSprite;
        [Tooltip("柱顶部的颜色")] public Color barTopColor = Color.white;
        [Tooltip("柱顶部的高度")] public float barTopHeight = 18;

        [Tooltip("柱底部的图片")] public Sprite barBottomSprite;
        [Tooltip("柱底部的颜色")] public Color barBottomColor = Color.white;
        [Tooltip("柱底部的高度")] public float barBottomHeight = 18;

        [Tooltip("是否显示柱的背景")] public bool isShowBarBg;
        [Tooltip("柱的背景Sprite")] public Sprite barBgSprite;
        [Tooltip("柱的背景颜色")] public Color barBgColor = Color.white;
        [Tooltip("柱的背景Image类型")] public Image.Type barBgImageType = Image.Type.Simple;
        [Tooltip("柱的背景像素单位倍率")] public float barBgPixelsPerUnitMultiplier = 1;

        [Header("柱文本："), Tooltip("是否显示柱的数值")] public bool isShowText = true;
        [Tooltip("柱数值的字体")] public TMP_FontAsset font;
        [Tooltip("柱数值的颜色")] public Color fontColor = Color.white;
        [Tooltip("柱数值的大小")] public float fontsize = 20;
        [Tooltip("柱数值的偏移量")] public Vector2 fontOffset;
        [Tooltip("柱数值的位置样式")] public BarTextPos barTextPos = BarTextPos.样式1;

        #endregion


        #region 弹框

        [Space(20), Header("弹框：")] public bool isShowTip = true;
        public Transform tip;

        #endregion

        [Header("数据："), Tooltip("X轴类型")] public ChartAxisType xAxisType = ChartAxisType.类目;
        [Tooltip("是否为时间")] public bool isTime;

        public List<BarCharCategoryData> dataCategory = new()
        {
            new BarCharCategoryData
            {
                key = "07-01", values = new()
                {
                    new BarCharValue { value = 518 },
                }
            },
            new BarCharCategoryData
            {
                key = "07-02", values = new()
                {
                    new BarCharValue { value = 613 }
                }
            },
            new BarCharCategoryData
            {
                key = "07-03", values = new()
                {
                    new BarCharValue { value = 345 }
                }
            },
            new BarCharCategoryData
            {
                key = "07-04", values = new()
                {
                    new BarCharValue { value = 425 }
                }
            },
            new BarCharCategoryData
            {
                key = "07-05", values = new()
                {
                    new BarCharValue { value = 647 }
                }
            }
        };


        public List<BarCharValueData> dataValue = new()
        {
            new BarCharValueData
            {
                key = 1, values = new()
                {
                    new BarCharValue { value = 518 },
                }
            },
            new BarCharValueData
            {
                key = 3, values = new()
                {
                    new BarCharValue { value = 613 }
                }
            },
            new BarCharValueData
            {
                key = 4, values = new()
                {
                    new BarCharValue { value = 345 }
                }
            },
            new BarCharValueData
            {
                key = 10, values = new()
                {
                    new BarCharValue { value = 425 }
                }
            },
            new BarCharValueData
            {
                key = 12, values = new()
                {
                    new BarCharValue { value = 647 }
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

            _bars = chartRectTrs.Find("Bars");
            _egGo = chartRectTrs.Find("Eg").gameObject;
            _egGo.SetActive(false);

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

            chart.ClearData();

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

            for (var i = 0; i < _bars.childCount; i++)
            {
                _bars.GetChild(i).gameObject.SetActive(false);
            }

            #region 自动设置Y轴最大值最小值

            if (yAxisAtb0.isAutomatic)
            {
                var positives0 = (from item in dataCategory from itemValue in item.values where !itemValue.isUseRightYAxis && itemValue.value >= 0 select itemValue.value).ToList();
                if (positives0.Count > 0)
                {
                    yAxisAtb0.axisMax = MaxDivisibleValue.GetMaxDivisibleValue(positives0.Max());
                }


                var negatives0 = (from item in dataCategory from itemValue in item.values where !itemValue.isUseRightYAxis && itemValue.value < 0 select itemValue.value).ToList();
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

            if (yAxisAtb1.isAutomatic)
            {
                var positives1 = (from item in dataCategory from itemValue in item.values where itemValue.isUseRightYAxis && itemValue.value >= 0 select itemValue.value).ToList();
                if (positives1.Count > 0)
                {
                    yAxisAtb1.axisMax = MaxDivisibleValue.GetMaxDivisibleValue(positives1.Max());
                }


                var negatives1 = (from item in dataCategory from itemValue in item.values where itemValue.isUseRightYAxis && itemValue.value < 0 select itemValue.value).ToList();
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
                xAxisAtb.axisMax = dataCategory.Count;
                xAxisAtb.axisMin = 0;
            }

            yAxis0.splitNumber = yAxisAtb0.axisTickCount;
            yAxis1.splitNumber = yAxisAtb1.axisTickCount;
            yAxis0.max = yAxisAtb0.axisMax;
            yAxis1.max = yAxisAtb1.axisMax;
            yAxis0.min = yAxisAtb0.axisMin;
            yAxis1.min = yAxisAtb1.axisMin;
            xAxis0.splitNumber = xAxisAtb.axisTickCount;
            xAxis0.max = xAxisAtb.axisMax;
            xAxis0.min = xAxisAtb.axisMin;

            #endregion


            for (var i = 0; i < dataCategory.Count; i++)
            {
                var item = dataCategory[i];
                xAxis0.AddData(item.key);
                var goGroup = _bars.Find("Group" + i);
                if (!goGroup)
                {
                    goGroup = new GameObject("Group" + i).AddComponent<RectTransform>();
                    goGroup.transform.parent = _bars;
                }

                var horizontalLayoutGroup = goGroup.gameObject.GetOrAddComponent<HorizontalLayoutGroup>();
                horizontalLayoutGroup.spacing = spacing;
                horizontalLayoutGroup.childAlignment = TextAnchor.LowerCenter;
                horizontalLayoutGroup.childControlHeight = false;
                horizontalLayoutGroup.childControlWidth = false;
                var contentSizeFitter = goGroup.GetOrAddComponent<ContentSizeFitter>();
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                goGroup.gameObject.SetActive(true);
                var goGroupRec = goGroup.GetComponent<RectTransform>();
                goGroupRec.pivot = new Vector2(0.5f, 0);
                goGroupRec.localScale = Vector3.one;
                goGroupRec.sizeDelta = new Vector2(goGroupRec.sizeDelta.x, chartSize.y);
                var goGroupPos = PointPos(new Vector2(i + 0.5f, 0), (float)yAxisAtb0.axisMax, dataCategory.Count, yAxisAtb0.axisMin, 0) + Vector3.up * offset;
                goGroup.localPosition = new Vector3(goGroupPos.x, goGroupPos.y, 0);


                for (var j = 0; j < goGroupRec.childCount; j++)
                {
                    goGroupRec.GetChild(j).gameObject.SetActive(false);
                }


                for (int j = 0; j < item.values.Count; j++)
                {
                    var go = goGroup.Find("Bar" + j);
                    if (!go)
                    {
                        go = Instantiate(_egGo, goGroup).transform;
                        go.name = "Bar" + j;
                    }

                    go.gameObject.SetActive(true);

                    var value = item.values[j];
                    var goTextRect = go.Find("Handle Slide Area/Handle/ValueText").GetComponent<RectTransform>();
                    var goText = goTextRect.GetComponent<TMP_Text>();
                    goText.text = value.value.ToString(CultureInfo.InvariantCulture);

                    var goChildRect = go.GetComponent<RectTransform>();
                    var barBody = go.Find("Fill Area/Fill/Background").GetComponent<RectTransform>();
                    var barBodyImage = barBody.GetComponent<Image>();
                    barBody.anchorMin = Vector2.zero;
                    barBody.anchorMax = Vector2.one;

                    //设置高度
                    var yLenght = 0f;
                    var yuePercent = 0f;
                    var slider = goChildRect.GetComponent<Slider>();
                    slider.interactable = false;

                    switch (gradualChangeMode)
                    {
                        case GradualChangeMode.单独渐变:
                            yLenght =
                                ValueLenghtY(value.value,
                                    (float)(value.isUseRightYAxis ? yAxisAtb1.axisMax : yAxisAtb0.axisMax),
                                    value.isUseRightYAxis ? yAxisAtb1.axisMin : yAxisAtb0.axisMin);
                            yLenght = yLenght > 0 ? yLenght : 0;

                            if (!Application.isPlaying)
                            {
                                slider.value = 1;
                            }
                            else
                            {
                                //柱子添加增长动画
                                slider.value = 0;
                                DOTween.To(() => slider.value, x => slider.value = x, 1, 1f);
                            }

                            break;
                        case GradualChangeMode.全局渐变:
                            yLenght = BlockSize(value.value > 0);
                            slider.value = 0;
                            yuePercent = ValuePercent(value.value,
                                (float)(value.isUseRightYAxis ? yAxisAtb1.axisMax : yAxisAtb0.axisMax),
                                value.isUseRightYAxis ? yAxisAtb1.axisMin : yAxisAtb0.axisMin);

                            if (!Application.isPlaying)
                            {
                                slider.value = yuePercent;
                            }
                            else
                            {
                                //柱子添加增长动画
                                slider.value = 0;
                                DOTween.To(() => slider.value, x => slider.value = x, yuePercent, 1f);
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                    //设置样式
                    goChildRect.sizeDelta = new Vector2(barWidth, yLenght);
                    goChildRect.localEulerAngles = value.value < 0 ? new Vector3(0, 180, 180) : Vector3.zero;
                    barBody.sizeDelta = new Vector2(barBody.sizeDelta.x, yLenght);
                    barBody.anchorMin = Vector2.zero;
                    barBody.anchorMax = Vector2.right;
                    barBodyImage.sprite = barSprite;
                    barBodyImage.color = barColor;
                    barBodyImage.type = barImageType;
                    barBodyImage.pixelsPerUnitMultiplier = barPixelsPerUnitMultiplier;

                    var barTopRect = goChildRect.Find("Handle Slide Area/Handle").GetComponent<RectTransform>();
                    var barTopImage = barTopRect.GetComponent<Image>();
                    var barBottomRect = goChildRect.Find("Fill Area/BottomImage").GetComponent<RectTransform>();
                    var barBottomImage = barBottomRect.GetComponent<Image>();
                    switch (barChartType)
                    {
                        case BarChartType.样式_3D:
                            barTopRect.sizeDelta = new Vector2(barTopRect.sizeDelta.x, barTopHeight);
                            barTopImage.enabled = true;
                            barTopImage.sprite = barTopSprite;
                            barTopImage.color = barTopColor;


                            barBottomRect.sizeDelta = new Vector2(barBottomRect.sizeDelta.x, barBottomHeight);
                            barBottomImage.enabled = true;
                            barBottomImage.sprite = barBottomSprite;
                            barBottomImage.color = barBottomColor;
                            break;
                        default:
                            barTopImage.enabled = false;
                            barBottomImage.enabled = false;
                            break;
                    }


                    var bgImage = go.Find("Fill Area").GetComponent<Image>();
                    if (isShowBarBg)
                    {
                        bgImage.enabled = true;
                        bgImage.sprite = barBgSprite;
                        bgImage.color = barBgColor;
                        bgImage.type = barBgImageType;
                        bgImage.pixelsPerUnitMultiplier = barBgPixelsPerUnitMultiplier;
                    }
                    else
                    {
                        bgImage.enabled = false;
                    }


                    goText.gameObject.SetActive(isShowText);
                    if (isShowText)
                    {
                        goText.font = font;
                        goText.color = fontColor;
                        goText.fontSize = fontsize;

                        if (value.value < 0)
                        {
                            goTextRect.localEulerAngles = new Vector3(0, 180, 180);
                            switch (barTextPos)
                            {
                                case BarTextPos.样式1:
                                    goTextRect.localPosition = fontOffset + Vector2.up * goTextRect.sizeDelta.y;
                                    break;
                                case BarTextPos.样式2:
                                    goTextRect.localPosition = -fontOffset - Vector2.up * yLenght;
                                    break;
                                case BarTextPos.样式3:

                                    switch (gradualChangeMode)
                                    {
                                        case GradualChangeMode.单独渐变:
                                            goTextRect.localPosition = fontOffset + Vector2.up * (BlockSize(false) - yLenght + goTextRect.sizeDelta.y);
                                            break;
                                        case GradualChangeMode.全局渐变:
                                            goTextRect.localPosition = fontOffset + Vector2.up * ((1 - yuePercent) * yLenght + goTextRect.sizeDelta.y);
                                            break;
                                    }

                                    break;
                            }
                        }
                        else
                        {
                            switch (barTextPos)
                            {
                                case BarTextPos.样式1 or BarTextPos.样式2:
                                    goTextRect.localEulerAngles = Vector3.zero;
                                    goText.transform.localPosition = fontOffset;
                                    break;
                                case BarTextPos.样式3:
                                    switch (gradualChangeMode)
                                    {
                                        case GradualChangeMode.单独渐变:
                                            goTextRect.localPosition = fontOffset + Vector2.up * (BlockSize(true) - yLenght);
                                            break;
                                        case GradualChangeMode.全局渐变:
                                            goTextRect.localPosition = fontOffset + Vector2.up * (1 - yuePercent) * yLenght;
                                            break;
                                    }

                                    break;
                            }
                        }
                    }

                    setBarActionCategory?.Invoke(item.key, i, j, barBodyImage, barTopImage, barBottomImage, goText);

                    if (isShowTip)
                    {
                        barBodyImage.OnPointerEnterAsObservable().Subscribe(_ =>
                        {
                            tip.gameObject.SetActive(true);
                            tip.position = barBody.position;
                            setTipActionCategory?.Invoke(item.key, i, j, tip);
                        });
                        barBodyImage.OnPointerExitAsObservable().Subscribe(_ => tip.gameObject.SetActive(false));
                    }
                }
            }
        }


        private void ResetValueData()
        {
            xAxis0.type = isTime ? Axis.AxisType.ValueTime : Axis.AxisType.Value;
            xAxis0.minMaxType = Axis.AxisMinMaxType.Custom;

            for (var i = 0; i < _bars.childCount; i++)
            {
                _bars.GetChild(i).gameObject.SetActive(false);
            }

            #region 自动设置Y轴最大值最小值

            if (yAxisAtb0.isAutomatic)
            {
                var positives0 = (from item in dataValue from itemValue in item.values where !itemValue.isUseRightYAxis && itemValue.value >= 0 select itemValue.value).ToList();
                if (positives0.Count > 0)
                {
                    yAxisAtb0.axisMax = MaxDivisibleValue.GetMaxDivisibleValue(positives0.Max());
                }


                var negatives0 = (from item in dataValue from itemValue in item.values where !itemValue.isUseRightYAxis && itemValue.value < 0 select itemValue.value).ToList();
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

            if (yAxisAtb1.isAutomatic)
            {
                var positives1 = (from item in dataValue from itemValue in item.values where itemValue.isUseRightYAxis && itemValue.value >= 0 select itemValue.value).ToList();
                if (positives1.Count > 0)
                {
                    yAxisAtb1.axisMax = MaxDivisibleValue.GetMaxDivisibleValue(positives1.Max());
                }


                var negatives1 = (from item in dataValue from itemValue in item.values where itemValue.isUseRightYAxis && itemValue.value < 0 select itemValue.value).ToList();
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
                var positivesX = (from item in dataValue where item.key >= 0 select item.key).ToList();
                if (positivesX.Count > 0)
                {
                    xAxisAtb.axisMax = MaxDivisibleValue.GetMaxDivisibleValue(positivesX.Max());
                }


                var negativesX = (from item in dataValue where item.key < 0 select item.key).ToList();
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

            yAxis0.splitNumber = yAxisAtb0.axisTickCount;
            yAxis1.splitNumber = yAxisAtb1.axisTickCount;
            yAxis0.max = yAxisAtb0.axisMax;
            yAxis1.max = yAxisAtb1.axisMax;
            yAxis0.min = yAxisAtb0.axisMin;
            yAxis1.min = yAxisAtb1.axisMin;
            xAxis0.splitNumber = xAxisAtb.axisTickCount;
            xAxis0.max = xAxisAtb.axisMax;
            xAxis0.min = xAxisAtb.axisMin;

            #endregion


            for (var i = 0; i < dataValue.Count; i++)
            {
                var item = dataValue[i];
                var goGroup = _bars.Find("Group" + i);
                if (!goGroup)
                {
                    goGroup = new GameObject("Group" + i).AddComponent<RectTransform>();
                    goGroup.transform.parent = _bars;
                }

                var horizontalLayoutGroup = goGroup.gameObject.GetOrAddComponent<HorizontalLayoutGroup>();
                horizontalLayoutGroup.spacing = spacing;
                horizontalLayoutGroup.childAlignment = TextAnchor.LowerCenter;
                horizontalLayoutGroup.childControlHeight = false;
                horizontalLayoutGroup.childControlWidth = false;
                var contentSizeFitter = goGroup.GetOrAddComponent<ContentSizeFitter>();
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                goGroup.gameObject.SetActive(true);
                var goGroupRec = goGroup.GetComponent<RectTransform>();
                goGroupRec.pivot = new Vector2(0.5f, 0);
                goGroupRec.localScale = Vector3.one;
                goGroupRec.sizeDelta = new Vector2(goGroupRec.sizeDelta.x, chartSize.y);
                var goGroupPos = PointPos(new Vector2(item.key, 0), (float)yAxisAtb0.axisMax, (float)xAxisAtb.axisMax, yAxisAtb0.axisMin, xAxisAtb.axisMin) + Vector3.up * offset;
                goGroup.localPosition = new Vector3(goGroupPos.x, goGroupPos.y, 0);

                for (var j = 0; j < goGroupRec.childCount; j++)
                {
                    goGroupRec.GetChild(j).gameObject.SetActive(false);
                }

                for (int j = 0; j < item.values.Count; j++)
                {
                    var go = goGroup.Find("Bar" + j);
                    if (!go)
                    {
                        go = Instantiate(_egGo, goGroup).transform;
                        go.name = "Bar" + j;
                    }

                    go.gameObject.SetActive(true);

                    var value = item.values[j];
                    var goTextRect = go.Find("Handle Slide Area/Handle/ValueText").GetComponent<RectTransform>();
                    var goText = goTextRect.GetComponent<TMP_Text>();
                    goText.text = value.value.ToString(CultureInfo.InvariantCulture);

                    var goChildRect = go.GetComponent<RectTransform>();
                    var barBody = go.Find("Fill Area/Fill/Background").GetComponent<RectTransform>();
                    var barBodyImage = barBody.GetComponent<Image>();
                    barBody.anchorMin = Vector2.zero;
                    barBody.anchorMax = Vector2.one;

                    //设置高度
                    var yLenght = 0f;
                    var yuePercent = 0f;
                    var slider = goChildRect.GetComponent<Slider>();
                    slider.interactable = false;

                    switch (gradualChangeMode)
                    {
                        case GradualChangeMode.单独渐变:
                            yLenght =
                                ValueLenghtY(value.value,
                                    (float)(value.isUseRightYAxis ? yAxisAtb1.axisMax : yAxisAtb0.axisMax),
                                    value.isUseRightYAxis ? yAxisAtb1.axisMin : yAxisAtb0.axisMin);
                            yLenght = yLenght > 0 ? yLenght : 0;

                            if (!Application.isPlaying)
                            {
                                slider.value = 1;
                            }
                            else
                            {
                                //柱子添加增长动画
                                slider.value = 0;
                                DOTween.To(() => slider.value, x => slider.value = x, 1, 1f);
                            }

                            break;
                        case GradualChangeMode.全局渐变:
                            yLenght = BlockSize(value.value > 0);
                            slider.value = 0;
                            yuePercent = ValuePercent(value.value,
                                (float)(value.isUseRightYAxis ? yAxisAtb1.axisMax : yAxisAtb0.axisMax),
                                value.isUseRightYAxis ? yAxisAtb1.axisMin : yAxisAtb0.axisMin);

                            if (!Application.isPlaying)
                            {
                                slider.value = yuePercent;
                            }
                            else
                            {
                                //柱子添加增长动画
                                slider.value = 0;
                                DOTween.To(() => slider.value, x => slider.value = x, yuePercent, 1f);
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                    //设置样式
                    goChildRect.sizeDelta = new Vector2(barWidth, yLenght);
                    goChildRect.localEulerAngles = value.value < 0 ? new Vector3(0, 180, 180) : Vector3.zero;
                    barBody.sizeDelta = new Vector2(barBody.sizeDelta.x, yLenght);
                    barBody.anchorMin = Vector2.zero;
                    barBody.anchorMax = Vector2.right;
                    barBodyImage.sprite = barSprite;
                    barBodyImage.color = barColor;
                    barBodyImage.type = barImageType;
                    barBodyImage.pixelsPerUnitMultiplier = barPixelsPerUnitMultiplier;

                    var barTopRect = goChildRect.Find("Handle Slide Area/Handle").GetComponent<RectTransform>();
                    var barTopImage = barTopRect.GetComponent<Image>();
                    var barBottomRect = goChildRect.Find("Fill Area/BottomImage").GetComponent<RectTransform>();
                    var barBottomImage = barBottomRect.GetComponent<Image>();
                    switch (barChartType)
                    {
                        case BarChartType.样式_3D:
                            barTopRect.sizeDelta = new Vector2(barTopRect.sizeDelta.x, barTopHeight);
                            barTopImage.enabled = true;
                            barTopImage.sprite = barTopSprite;
                            barTopImage.color = barTopColor;


                            barBottomRect.sizeDelta = new Vector2(barBottomRect.sizeDelta.x, barBottomHeight);
                            barBottomImage.enabled = true;
                            barBottomImage.sprite = barBottomSprite;
                            barBottomImage.color = barBottomColor;
                            break;
                        default:
                            barTopImage.enabled = false;
                            barBottomImage.enabled = false;
                            break;
                    }


                    var bgImage = go.Find("Fill Area").GetComponent<Image>();
                    if (isShowBarBg)
                    {
                        bgImage.enabled = true;
                        bgImage.sprite = barBgSprite;
                        bgImage.color = barBgColor;
                        bgImage.type = barBgImageType;
                        bgImage.pixelsPerUnitMultiplier = barBgPixelsPerUnitMultiplier;
                    }
                    else
                    {
                        bgImage.enabled = false;
                    }


                    goText.gameObject.SetActive(isShowText);
                    if (isShowText)
                    {
                        goText.font = font;
                        goText.color = fontColor;
                        goText.fontSize = fontsize;

                        if (value.value < 0)
                        {
                            goTextRect.localEulerAngles = new Vector3(0, 180, 180);
                            switch (barTextPos)
                            {
                                case BarTextPos.样式1:
                                    goTextRect.localPosition = fontOffset + Vector2.up * goTextRect.sizeDelta.y;
                                    break;
                                case BarTextPos.样式2:
                                    goTextRect.localPosition = -fontOffset - Vector2.up * yLenght;
                                    break;
                                case BarTextPos.样式3:

                                    switch (gradualChangeMode)
                                    {
                                        case GradualChangeMode.单独渐变:
                                            goTextRect.localPosition = fontOffset + Vector2.up * (BlockSize(false) - yLenght + goTextRect.sizeDelta.y);
                                            break;
                                        case GradualChangeMode.全局渐变:
                                            goTextRect.localPosition = fontOffset + Vector2.up * ((1 - yuePercent) * yLenght + goTextRect.sizeDelta.y);
                                            break;
                                    }

                                    break;
                            }
                        }
                        else
                        {
                            switch (barTextPos)
                            {
                                case BarTextPos.样式1 or BarTextPos.样式2:
                                    goTextRect.localEulerAngles = Vector3.zero;
                                    goText.transform.localPosition = fontOffset;
                                    break;
                                case BarTextPos.样式3:
                                    switch (gradualChangeMode)
                                    {
                                        case GradualChangeMode.单独渐变:
                                            goTextRect.localPosition = fontOffset + Vector2.up * (BlockSize(true) - yLenght);
                                            break;
                                        case GradualChangeMode.全局渐变:
                                            goTextRect.localPosition = fontOffset + Vector2.up * (1 - yuePercent) * yLenght;
                                            break;
                                    }

                                    break;
                            }
                        }
                    }

                    setBarActionValue?.Invoke(item.key, i, j, barBodyImage, barTopImage, barBottomImage, goText);

                    if (isShowTip)
                    {
                        barBodyImage.OnPointerEnterAsObservable().Subscribe(_ =>
                        {
                            tip.gameObject.SetActive(true);
                            tip.position = barBody.position;
                            setTipActionValue?.Invoke(item.key, i, j, tip);
                        });
                        barBodyImage.OnPointerExitAsObservable().Subscribe(_ => tip.gameObject.SetActive(false));
                    }
                }
            }
        }


        /// <summary>
        /// 根据X位置分割，isPositive=true:上半部分的大小，isPositive=false:上半部分的大小
        /// </summary>
        /// <param name="isPositive"></param>
        /// <returns></returns>
        private float BlockSize(bool isPositive)
        {
            var sizeHigh = 0f;

            if (yAxisAtb0.axisMin >= 0)
            {
                sizeHigh = chartSize.y;
            }
            else
            {
                if (isPositive)
                {
                    sizeHigh = chartSize.y * Mathf.Abs((float)yAxisAtb0.axisMax) / ((float)yAxisAtb0.axisMax - yAxisAtb0.axisMin);
                }
                else
                {
                    sizeHigh = chartSize.y * Mathf.Abs(yAxisAtb0.axisMin) / ((float)yAxisAtb0.axisMax - yAxisAtb0.axisMin);
                }
            }

            return sizeHigh;
        }

        /// <summary>
        /// 设置图片单元格占用像素点的比例
        /// </summary>
        /// <param name="image"></param>
        private void SetImagePixelsPerUnitMultiplier(Image image)
        {
            var sprite = image.sprite;
            if (!sprite) return;

            var width = GetRectSize(image.rectTransform).x;
            image.pixelsPerUnitMultiplier = sprite.rect.width / width;
        }
    }
}