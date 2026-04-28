using Prospect;
using UnityEditor;

[CustomEditor(typeof(GraphAxisBase))]
public class GraphAxisBaseInspector : Editor
{
    private GraphAxisBase _graphAxisBase;

    private SerializedProperty _chartBgImage;
    private SerializedProperty _chartAxisCountX;
    private SerializedProperty _xAxisAtb;
    private SerializedProperty _chartAxisCountY;
    private SerializedProperty _yAxisAtb0;
    private SerializedProperty _yAxisAtb1;

    public void OnEnable()
    {
        _chartBgImage = serializedObject.FindProperty("chartBgImage");
        _chartAxisCountX = serializedObject.FindProperty("chartAxisCountX");
        _xAxisAtb = serializedObject.FindProperty("xAxisAtb");
        _chartAxisCountY = serializedObject.FindProperty("chartAxisCountY");
        _yAxisAtb0 = serializedObject.FindProperty("yAxisAtb0");
        _yAxisAtb1 = serializedObject.FindProperty("yAxisAtb1");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _graphAxisBase = (GraphAxisBase)target;

        EditorGUILayout.PropertyField(_chartBgImage);
        EditorGUILayout.PropertyField(_chartAxisCountX);
        EditorGUILayout.PropertyField(_xAxisAtb);
        EditorGUILayout.PropertyField(_chartAxisCountY);
        EditorGUILayout.PropertyField(_yAxisAtb0);

        if (_graphAxisBase.chartAxisCountY == ChartAxisCount.双轴)
        {
            EditorGUILayout.PropertyField(_yAxisAtb1);
        }

        serializedObject.ApplyModifiedProperties();
    }
}