using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(AreaStyle), true)]
    public class AreaStyleDrawer : BasePropertyDrawer
    { 
        public override string ClassName { get { return "AreaStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Origin");
                PropertyField(prop, "m_Color");
                PropertyField(prop, "m_ToColor");
                PropertyField(prop, "m_Opacity");
                PropertyField(prop, "m_ToTop");
                PropertyField(prop, "m_InnerFill");
                PropertyField(prop, "m_OpenNormalArea", "m_OpenNormalArea");
                PropertyField(prop, "m_OpenAreaCoefficient", "m_OpenAreaCoefficient");
                PropertyField(prop, "m_AreaDisCoefficient", string.Empty);
                PropertyField(prop, "m_AreaCoefficient",string.Empty);
                PropertyField(prop, "m_AreaShadow", "m_AreaShadow");
                PropertyField(prop, "m_AreaShadowDisCoefficient");
                PropertyField(prop, "m_AreaShadowCoefficient");
                PropertyField(prop, "m_AreaShadowColor", string.Empty);
                PropertyField(prop, "m_AreaShadowToColor", string.Empty);
                PropertyField(prop, "m_AreaShadowOpacity", string.Empty);
                --EditorGUI.indentLevel;
            }
        }
    }
}