using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(Vessel))]
    public class VesselEditor : MainComponentEditor<Vessel>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            var shape = (Vessel.Shape)baseProperty.FindPropertyRelative("m_Shape").intValue;
            PropertyField("m_Shape");
            PropertyField("m_ShapeWidth");
            PropertyField("m_BorderWidth");
            PropertyField("m_Gap");
            PropertyTwoFiled("m_Center");
            PropertyField("m_BackgroundColor");
            PropertyField("m_Color");
            PropertyField("m_toColor");
            PropertyField("m_BorderColor");
            PropertyField("m_AutoColor");
            switch (shape)
            {
                case Vessel.Shape.Circle:
                    PropertyField("m_Radius");
                    PropertyField("m_Smoothness");
                    break;
                case Vessel.Shape.Rect:
                    PropertyField("m_Width");
                    PropertyField("m_Height");
                    PropertyListField("m_CornerRadius", true);
                    break;
            }
            --EditorGUI.indentLevel;
        }
    }
}