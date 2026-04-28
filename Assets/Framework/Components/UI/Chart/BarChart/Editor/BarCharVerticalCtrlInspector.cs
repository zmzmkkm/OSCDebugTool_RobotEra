using Prospect;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(BarChartVerticalCtrl))]
public class BarCharVerticalCtrlInspector : GraphAxisBaseInspector
{
    private Editor cacheEditor;
    // private Editor GraphAxisBaseInspector;

    private BarChartVerticalCtrl _barChartVerticalCtrl;


    private SerializedProperty _gradualChangeMode;
    private SerializedProperty _spacing;
    private SerializedProperty _offset;
    private SerializedProperty _barChartType;
    private SerializedProperty _barSprite;
    private SerializedProperty _barColor;
    private SerializedProperty _barWidth;
    private SerializedProperty _barImageType;
    private SerializedProperty _barPixelsPerUnitMultiplier;
    private SerializedProperty _barTopSprite;
    private SerializedProperty _barTopColor;
    private SerializedProperty _barTopHeight;
    private SerializedProperty _barBottomSprite;
    private SerializedProperty _barBottomColor;
    private SerializedProperty _barBottomHeight;
    private SerializedProperty _isShowBarBg;
    private SerializedProperty _barBgSprite;
    private SerializedProperty _barBgColor;
    private SerializedProperty _barBgImageType;
    private SerializedProperty _barBgPixelsPerUnitMultiplier;
    private SerializedProperty _isShowText;
    private SerializedProperty _font;
    private SerializedProperty _fontColor;
    private SerializedProperty _fontsize;
    private SerializedProperty _fontOffset;
    private SerializedProperty _barTextPos;
    private SerializedProperty _isShowTip;
    private SerializedProperty _tip;
    private SerializedProperty _xAxisType;
    private SerializedProperty _isTime;
    private SerializedProperty _dataCategory;
    private SerializedProperty _dataValue;

    void OnEnable()
    {
        base.OnEnable();

        _gradualChangeMode = serializedObject.FindProperty("gradualChangeMode");
        _spacing = serializedObject.FindProperty("spacing");
        _offset = serializedObject.FindProperty("offset");
        _barChartType = serializedObject.FindProperty("barChartType");
        _barSprite = serializedObject.FindProperty("barSprite");
        _barColor = serializedObject.FindProperty("barColor");
        _barWidth = serializedObject.FindProperty("barWidth");
        _barImageType = serializedObject.FindProperty("barImageType");
        _barPixelsPerUnitMultiplier = serializedObject.FindProperty("barPixelsPerUnitMultiplier");
        _barTopSprite = serializedObject.FindProperty("barTopSprite");
        _barTopColor = serializedObject.FindProperty("barTopColor");
        _barTopHeight = serializedObject.FindProperty("barTopHeight");
        _barBottomSprite = serializedObject.FindProperty("barBottomSprite");
        _barBottomColor = serializedObject.FindProperty("barBottomColor");
        _barBottomHeight = serializedObject.FindProperty("barBottomHeight");
        _isShowBarBg = serializedObject.FindProperty("isShowBarBg");
        _barBgSprite = serializedObject.FindProperty("barBgSprite");
        _barBgColor = serializedObject.FindProperty("barBgColor");
        _barBgImageType = serializedObject.FindProperty("barBgImageType");
        _barBgPixelsPerUnitMultiplier = serializedObject.FindProperty("barBgPixelsPerUnitMultiplier");
        _isShowText = serializedObject.FindProperty("isShowText");
        _font = serializedObject.FindProperty("font");
        _fontColor = serializedObject.FindProperty("fontColor");
        _fontsize = serializedObject.FindProperty("fontsize");
        _fontOffset = serializedObject.FindProperty("fontOffset");
        _barTextPos = serializedObject.FindProperty("barTextPos");
        _isShowTip = serializedObject.FindProperty("isShowTip");
        _tip = serializedObject.FindProperty("tip");
        _xAxisType = serializedObject.FindProperty("xAxisType");
        _isTime = serializedObject.FindProperty("isTime");
        _dataCategory = serializedObject.FindProperty("dataCategory");
        _dataValue = serializedObject.FindProperty("dataValue");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        _barChartVerticalCtrl = (BarChartVerticalCtrl)target;

        // var graphAxisBase = _barCharVerticalCtrl.graphAxisBase;
        // if (graphAxisBase != null)
        // {
        //     //创建TestClass的Editor
        //     if (cacheEditor == null)
        //         cacheEditor = CreateEditor(graphAxisBase);
        //     cacheEditor.OnInspectorGUI();
        // }

        GUILayout.Space(20);

        EditorGUILayout.PropertyField(_gradualChangeMode);
        EditorGUILayout.PropertyField(_spacing);
        EditorGUILayout.PropertyField(_offset);

        GUILayout.Space(10);
        EditorGUILayout.PropertyField(_barChartType);
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("↓↓ 柱体样式设置 ↓↓", GUILayout.ExpandWidth(true));
        EditorGUILayout.PropertyField(_barSprite);
        EditorGUILayout.PropertyField(_barColor);
        EditorGUILayout.PropertyField(_barWidth);
        EditorGUILayout.PropertyField(_barImageType);

        if (_barChartVerticalCtrl.barImageType is Image.Type.Sliced or Image.Type.Tiled)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("推荐像素倍率：", GUILayout.Width(90));
            if (_barChartVerticalCtrl.barSprite)
            {
                GUILayout.Label(_barChartVerticalCtrl.barSprite.rect.width / (_barChartVerticalCtrl.barWidth) + "", GUILayout.Width(80));
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(_barPixelsPerUnitMultiplier);
        }

        GUILayout.Space(10);
        // _barCharVerticalCtrl.barChartType = (BarChartType)EditorGUILayout.EnumPopup("BarChartType", _barCharVerticalCtrl.barChartType);
        if (_barChartVerticalCtrl.barChartType == BarChartType.样式_3D)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("↓↓ 顶部样式设置 ↓↓");
            EditorGUILayout.PropertyField(_barTopSprite);
            EditorGUILayout.PropertyField(_barTopColor);
            EditorGUILayout.PropertyField(_barTopHeight);
            GUILayout.Space(10);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("↓↓ 底部样式设置 ↓↓");
            EditorGUILayout.PropertyField(_barBottomSprite);
            EditorGUILayout.PropertyField(_barBottomColor);
            EditorGUILayout.PropertyField(_barBottomHeight);
        }

        if (_barChartVerticalCtrl.gradualChangeMode == GradualChangeMode.全局渐变 && _barChartVerticalCtrl.barChartType == BarChartType.样式_2D)
        {
            GUILayout.Label("↓↓ 仅用于需要背景的情况 ↓↓", GUILayout.ExpandWidth(true));

            EditorGUILayout.PropertyField(_isShowBarBg);
            if (_barChartVerticalCtrl.isShowBarBg)
            {
                EditorGUILayout.PropertyField(_barBgSprite);
                EditorGUILayout.PropertyField(_barBgColor);
                EditorGUILayout.PropertyField(_barBgImageType);
                if (_barChartVerticalCtrl.barBgImageType is Image.Type.Sliced or Image.Type.Tiled)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("推荐像素倍率：", GUILayout.Width(90));
                    if (_barChartVerticalCtrl.barBgSprite)
                    {
                        GUILayout.Label(_barChartVerticalCtrl.barBgSprite.rect.width / _barChartVerticalCtrl.barWidth + "", GUILayout.Width(80));
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.PropertyField(_barBgPixelsPerUnitMultiplier);
                }
            }
        }
        else
        {
            _barChartVerticalCtrl.isShowBarBg = false;
        }

        GUILayout.Space(10);
        EditorGUILayout.PropertyField(_isShowText);
        if (_barChartVerticalCtrl.isShowText)
        {
            EditorGUILayout.PropertyField(_font);
            EditorGUILayout.PropertyField(_fontColor);
            EditorGUILayout.PropertyField(_fontsize);
            EditorGUILayout.PropertyField(_fontOffset);
            EditorGUILayout.PropertyField(_barTextPos);
        }

        GUILayout.Space(10);
        EditorGUILayout.PropertyField(_isShowTip);
        if (_barChartVerticalCtrl.isShowTip)
        {
            EditorGUILayout.PropertyField(_tip);
        }


        EditorGUILayout.PropertyField(_xAxisType);
        switch (_barChartVerticalCtrl.xAxisType)
        {
            case ChartAxisType.类目:
                EditorGUILayout.PropertyField(_dataCategory);
                break;
            case ChartAxisType.数值:
                EditorGUILayout.PropertyField(_isTime);
                EditorGUILayout.PropertyField(_dataValue);
                break;
        }


        serializedObject.ApplyModifiedProperties();
        // base.OnInspectorGUI();
    }
}