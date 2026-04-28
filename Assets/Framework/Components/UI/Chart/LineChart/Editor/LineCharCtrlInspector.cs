using Prospect;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LineCharCtrl))]
public class LineCharCtrlInspector : GraphAxisBaseInspector
{
    private LineCharCtrl _lineCharCtrl;

    private SerializedProperty _lineAttributeList;

    private SerializedProperty _isShowTip;
    private SerializedProperty _tip;
    private SerializedProperty _tipOffset;
    private SerializedProperty _xAxisType;
    private SerializedProperty _dataCategory;
    private SerializedProperty _dataValue;
    private SerializedProperty _isTime;

    void OnEnable()
    {
        base.OnEnable();

        _lineAttributeList = serializedObject.FindProperty("lineAttributeList");

        _isShowTip = serializedObject.FindProperty("isShowTip");
        _tip = serializedObject.FindProperty("tip");
        _tipOffset = serializedObject.FindProperty("tipOffset");
        _xAxisType = serializedObject.FindProperty("xAxisType");
        _dataCategory = serializedObject.FindProperty("dataCategory");
        _dataValue = serializedObject.FindProperty("dataValue");
        _isTime = serializedObject.FindProperty("isTime");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        _lineCharCtrl = (LineCharCtrl)target;

        GUILayout.Space(10);

        EditorGUILayout.PropertyField(_lineAttributeList);

        EditorGUILayout.PropertyField(_isShowTip);
        if (_lineCharCtrl.isShowTip)
        {
            EditorGUILayout.PropertyField(_tip);
            EditorGUILayout.PropertyField(_tipOffset);
        }

        EditorGUILayout.PropertyField(_xAxisType);
        switch (_lineCharCtrl.xAxisType)
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
    }
}