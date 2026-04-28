using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(SerieSymbol), true)]
    public class SerieSymbolDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Symbol"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                var type = (SymbolType)prop.FindPropertyRelative("m_Type").enumValueIndex;
                PropertyField(prop, "m_Type");
                if (type == SymbolType.Custom)
                {
                    //AddHelpBox("Custom symbol only work in PictorialBar serie", MessageType.Warning);
                    AddHelpBox("此部分已做修改，绘制代码见[SerieSymbolDrawer]脚本，源码不绘制{m_Height}", MessageType.Error);
                    PropertyField(prop, "m_Image");
                    PropertyField(prop, "m_ImageType");
                    PropertyField(prop, "m_Width");
                    PropertyField(prop, "m_Height");
                    // PropertyField(prop, "m_Offset");
                }
                else if (type.Equals(SymbolType.Doughnut))
                {
                    PropertyField(prop, "m_TCInsideRad");

                }
                else if (type.Equals(SymbolType.ThreeCircle))
                {
                    //todo:只显示外半径、内半径、圆心半径的变量及颜色、偏移的距离、偏移的颜色、角度(起始、结束)
                    PropertyField(prop, "m_TCOutLineRad");
                    PropertyField(prop, "m_TCInsideRad");
                    PropertyField(prop, "m_TCCellRad");

                    PropertyField(prop, "m_TCOutLineColor");
                    PropertyField(prop, "m_TCInsideColor");
                    PropertyField(prop, "m_TCCellColor");
                    PropertyField(prop, "m_TCOffset");
                    PropertyField(prop, "m_TCOffsetColor");
                    PropertyField(prop, "m_TCSAngle");
                    PropertyField(prop, "m_TCEAngle");
                }
                PropertyField(prop, "m_Gap");
                PropertyField(prop, "m_SizeType");
                switch ((SymbolSizeType)prop.FindPropertyRelative("m_SizeType").enumValueIndex)
                {
                    case SymbolSizeType.Custom:
                        PropertyField(prop, "m_Size");
                        break;
                    case SymbolSizeType.FromData:
                        PropertyField(prop, "m_DataIndex");
                        PropertyField(prop, "m_DataScale");
                        PropertyField(prop, "m_MinSize");
                        PropertyField(prop, "m_MaxSize");
                        break;
                    case SymbolSizeType.Function:
                        break;
                }
                PropertyField(prop, "m_StartIndex");
                PropertyField(prop, "m_Interval");
                PropertyField(prop, "m_ForceShowLast");
                PropertyField(prop, "m_Repeat");
                --EditorGUI.indentLevel;
            }
        }
    }
}