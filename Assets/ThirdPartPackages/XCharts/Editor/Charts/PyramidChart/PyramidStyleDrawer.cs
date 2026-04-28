using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(PyramidStyle), true)]
    public class PyramidStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "PyramidStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                var m3d = prop.FindPropertyRelative("m_3D").boolValue;
                PropertyField(prop, "m_3D");
                var mClockDataArea = prop.FindPropertyRelative("m_clockDataArea").boolValue;
                PropertyField(prop, "m_clockDataArea");
                if (mClockDataArea)
                {
                    if (m3d)
                        PropertyField(prop, "m_clockData3DVal");
                    else
                        PropertyField(prop, "m_clockData2DVal");
                }         
                PropertyField(prop, "m_DrawTop");
                PropertyField(prop, "m_BottomPointRate");
                PropertyField(prop, "m_LeftPointRate");
                PropertyField(prop, "m_RightPointRate");
                PropertyField(prop, "m_LeftColorOpacity");
                PropertyField(prop, "m_RightColorOpacity");
                PropertyField(prop, "m_LabelLineMargin");
                --EditorGUI.indentLevel;
            }
        }
    }
}