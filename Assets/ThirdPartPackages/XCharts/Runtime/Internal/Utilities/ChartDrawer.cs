using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    public static class ChartDrawer
    {
        public static void DrawSymbol(VertexHelper vh, SymbolType type, float symbolSize, float tickness,
            Vector3 pos, Color32 color, Color32 toColor, float gap, float[] cornerRadius,
            Color32 emptyColor, Color32 backgroundColor, Color32 borderColor, float smoothness, Vector3 startPos,
            float m_TCOutLineRad = 0f,float m_TCInsideRad = 0f,float m_TCCellRad = 0f,
            Color32 m_TCOutLineColor = default(Color32),Color32 m_TCInsideColor = default(Color32),Color32 m_TCCellColor = default(Color32),
            float m_TCOffset = 0f,Color32 m_TCOffsetColor = default(Color32),float m_TCSAngle = 0f,float m_TCEAngle = 0f)
        {
            switch (type)
            {
                case SymbolType.None:
                    break;
                case SymbolType.Circle:
                    if (gap > 0)
                    {
                        UGL.DrawDoughnut(vh, pos, symbolSize, symbolSize + gap, backgroundColor, backgroundColor, color, smoothness);
                    }
                    else
                    {    
                        if (tickness > 0)            
                            UGL.DrawDoughnut(vh, pos, symbolSize, symbolSize + tickness, borderColor, borderColor, color, smoothness);              
                        else                
                            UGL.DrawCricle(vh, pos, symbolSize, color, toColor, smoothness);                        
                    }
                    break;
                case SymbolType.EmptyCircle:
                    if (gap > 0)
                    {
                        UGL.DrawCricle(vh, pos, symbolSize + gap, backgroundColor, smoothness);
                        UGL.DrawEmptyCricle(vh, pos, symbolSize, tickness, color, color, emptyColor, smoothness);
                    }
                    else
                    {
                        UGL.DrawEmptyCricle(vh, pos, symbolSize, tickness, color, color, emptyColor, smoothness);
                    }
                    break;
                case SymbolType.Rect:
                    if (gap > 0)
                    {
                        UGL.DrawSquare(vh, pos, symbolSize + gap, backgroundColor);
                        UGL.DrawSquare(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        if (tickness > 0)
                        {
                            UGL.DrawRoundRectangle(vh, pos, symbolSize * 2, symbolSize * 2, color, color, 0, cornerRadius, true);
                            UGL.DrawBorder(vh, pos, symbolSize, symbolSize, tickness, borderColor, 0, cornerRadius);
                        }
                        else
                            UGL.DrawRoundRectangle(vh, pos, symbolSize * 2, symbolSize * 2, color, color, 0, cornerRadius, true);
                    }
                    break;
                case SymbolType.EmptyRect:
                    if (gap > 0)
                    {
                        UGL.DrawSquare(vh, pos, symbolSize + gap, backgroundColor);
                        UGL.DrawBorder(vh, pos, symbolSize * 2, symbolSize * 2, tickness, color);
                    }
                    else
                    {
                        UGL.DrawBorder(vh, pos, symbolSize * 2 - tickness * 2, symbolSize * 2 - tickness * 2, tickness, color);
                    }
                    break;
                case SymbolType.Triangle:
                case SymbolType.EmptyTriangle:
                    if (gap > 0)
                    {
                        UGL.DrawEmptyTriangle(vh, pos, symbolSize * 1.4f + gap * 2, gap * 2, backgroundColor);
                    }
                    if (type == SymbolType.EmptyTriangle)
                    {
                        UGL.DrawEmptyTriangle(vh, pos, symbolSize * 1.4f, tickness * 2f, color, emptyColor);
                    }
                    else
                    {
                        UGL.DrawTriangle(vh, pos, symbolSize * 1.4f, color, toColor);
                    }
                    break;
                case SymbolType.Diamond:
                case SymbolType.EmptyDiamond:
                    var xRadius = symbolSize;
                    var yRadius = symbolSize * 1.5f;
                    if (gap > 0)
                    {
                        UGL.DrawEmptyDiamond(vh, pos, xRadius + gap, yRadius + gap, gap, backgroundColor);
                    }
                    if (type == SymbolType.EmptyDiamond)
                    {
                        UGL.DrawEmptyDiamond(vh, pos, xRadius, yRadius, tickness, color, emptyColor);
                    }
                    else
                    {
                        UGL.DrawDiamond(vh, pos, xRadius, yRadius, color, toColor);
                    }
                    break;
                case SymbolType.Arrow:
                case SymbolType.EmptyArrow:
                    var arrowWidth = symbolSize * 2;
                    var arrowHeight = arrowWidth * 1.5f;
                    var arrowOffset = 0;
                    var arrowDent = arrowWidth / 3.3f;
                    if (gap > 0)
                    {
                        arrowWidth = (symbolSize + gap) * 2;
                        arrowHeight = arrowWidth * 1.5f;
                        arrowOffset = 0;
                        arrowDent = arrowWidth / 3.3f;
                        var dir = (pos - startPos).normalized;
                        var sharpPos = pos + gap * dir;
                        UGL.DrawArrow(vh, startPos, sharpPos, arrowWidth, arrowHeight,
                            arrowOffset, arrowDent, backgroundColor);
                    }
                    arrowWidth = symbolSize * 2;
                    arrowHeight = arrowWidth * 1.5f;
                    arrowOffset = 0;
                    arrowDent = arrowWidth / 3.3f;
                    UGL.DrawArrow(vh, startPos, pos, arrowWidth, arrowHeight,
                        arrowOffset, arrowDent, color);
                    if (type == SymbolType.EmptyArrow)
                    {
                        arrowWidth = (symbolSize - tickness) * 2;
                        arrowHeight = arrowWidth * 1.5f;
                        arrowOffset = 0;
                        arrowDent = arrowWidth / 3.3f;
                        var dir = (pos - startPos).normalized;
                        var sharpPos = pos - tickness * dir;
                        UGL.DrawArrow(vh, startPos, sharpPos, arrowWidth, arrowHeight,
                            arrowOffset, arrowDent, backgroundColor);
                    }
                    break;
                case SymbolType.Plus:
                    if (gap > 0)
                    {
                        UGL.DrawPlus(vh, pos, symbolSize + gap, tickness + gap, backgroundColor);
                    }
                    UGL.DrawPlus(vh, pos, symbolSize, tickness, color);
                    break;
                case SymbolType.Minus:
                    if (gap > 0)
                    {
                        UGL.DrawMinus(vh, pos, symbolSize + gap, tickness + gap, backgroundColor);
                    }
                    UGL.DrawMinus(vh, pos, symbolSize, tickness, color);
                    break;   
                case SymbolType.Doughnut:
                    if (gap > 0)
                    {
                        UGL.DrawDoughnut(vh, pos, symbolSize, symbolSize + gap, backgroundColor, backgroundColor, color, smoothness);
                    }
                    else
                    {
                        //UGL.DrawDoughnut(vh, pos, symbolSize, symbolSize + tickness, borderColor, borderColor, color, smoothness);
                        //todo:创建内半径的变量，外半径依然为symbolSize，空心颜色
                        UGL.DrawDoughnut(vh, pos, m_TCInsideRad, symbolSize + tickness, color, toColor, borderColor, smoothness);
                    }
                    break;
                case SymbolType.ThreeCircle:
                    if (gap > 0)
                    {
                        //UGL.DrawDoughnut(vh, pos, symbolSize, symbolSize + gap, backgroundColor, backgroundColor, color, smoothness);
                        UGL.DrawSemicircle(vh, pos - new Vector3(0f,m_TCOffset, 0f), m_TCOutLineRad + gap, m_TCOffsetColor, m_TCOffsetColor, m_TCSAngle, m_TCEAngle, gap, smoothness, 0);
                        UGL.DrawCricle(vh, pos, m_TCOutLineRad + gap, m_TCOutLineColor, smoothness);
                        UGL.DrawCricle(vh, pos, m_TCInsideRad + gap, m_TCInsideColor, smoothness);
                        UGL.DrawCricle(vh, pos, m_TCCellRad + gap, m_TCCellColor, smoothness);
                    }
                    else
                    {
                        //todo:依次创建外半径、内半径、圆心半径的变量及颜色,偏移的距离、偏移的颜色、角度(起始、结束)
                        UGL.DrawSemicircle(vh, pos - new Vector3(0f, m_TCOffset, 0f), m_TCOutLineRad, m_TCOffsetColor, m_TCOffsetColor, m_TCSAngle, m_TCEAngle,gap, smoothness,0);
                        UGL.DrawCricle(vh, pos, m_TCOutLineRad, m_TCOutLineColor, smoothness);              
                        UGL.DrawCricle(vh, pos, m_TCInsideRad, m_TCInsideColor, smoothness);
                        UGL.DrawCricle(vh, pos, m_TCCellRad, m_TCCellColor, smoothness);    
                    }
                    break;
            }
        }

        

        /// <summary>
        /// 用于折线图自定义标记绘制
        /// </summary>
        public static void DrawSymbolExtension(VertexHelper vh, SymbolType type,Vector3 pos,Transform parent, SymbolStyle symbol)
        {
            switch (type)
            {
                case SymbolType.Custom:
                    //Debug.Log("自定义标记pos: " + pos);
                    ChartHelperExtension.AddLineSeriesIcon
                    ($"Icon_{pos.y}", parent,
                        symbol,
                        pos
                    );
                    break;
                case SymbolType.Doughnut:
                case SymbolType.ThreeCircle:                       
                case SymbolType.Circle:
                case SymbolType.EmptyCircle:      
                case SymbolType.Rect:                  
                case SymbolType.EmptyRect:          
                case SymbolType.Triangle:         
                case SymbolType.EmptyTriangle:            
                case SymbolType.Diamond:           
                case SymbolType.EmptyDiamond:           
                case SymbolType.Arrow:            
                case SymbolType.EmptyArrow:             
                case SymbolType.Plus:                 
                case SymbolType.Minus:
                    ChartHelperExtension.AddLineSeriesIcon
                      ($"Icon_{pos.y}", parent,
                          symbol,
                          pos,
                          false
                      );
                    break;
            }
        }

        public static void DrawLineStyle(VertexHelper vh, LineStyle lineStyle, Vector3 startPos, Vector3 endPos,
            Color32 defaultColor, float themeWidth, LineStyle.Type themeType)
        {
            var type = lineStyle.GetType(themeType);
            var width = lineStyle.GetWidth(themeWidth);
            var color = lineStyle.GetColor(defaultColor);
            DrawLineStyle(vh, type, width, startPos, endPos, color, color);
        }

        public static void DrawLineStyle(VertexHelper vh, LineStyle lineStyle, Vector3 startPos, Vector3 endPos,
            float themeWidth, LineStyle.Type themeType, Color32 defaultColor, Color32 defaultToColor)
        {
            var type = lineStyle.GetType(themeType);
            var width = lineStyle.GetWidth(themeWidth);
            var color = lineStyle.GetColor(defaultColor);
            var toColor = ChartHelper.IsClearColor(defaultToColor) ? color : defaultToColor;
            DrawLineStyle(vh, type, width, startPos, endPos, color, toColor);
        }

        public static void DrawLineStyle(VertexHelper vh, LineStyle.Type lineType, float lineWidth,
            Vector3 startPos, Vector3 endPos, Color32 color)
        {
            DrawLineStyle(vh, lineType, lineWidth, startPos, endPos, color, color);
        }

        public static void DrawLineStyle(VertexHelper vh, LineStyle.Type lineType, float lineWidth,
            Vector3 startPos, Vector3 endPos, Color32 color, Color32 toColor)
        {
            switch (lineType)
            {
                case LineStyle.Type.Dashed:
                    UGL.DrawDashLine(vh, startPos, endPos, lineWidth, color, toColor);
                    break;
                case LineStyle.Type.Dotted:
                    UGL.DrawDotLine(vh, startPos, endPos, lineWidth, color, toColor);
                    break;
                case LineStyle.Type.Solid:
                    UGL.DrawLine(vh, startPos, endPos, lineWidth, color, toColor);
                    break;
                case LineStyle.Type.DashDot:
                    UGL.DrawDashDotLine(vh, startPos, endPos, lineWidth, color);
                    break;
                case LineStyle.Type.DashDotDot:
                    UGL.DrawDashDotDotLine(vh, startPos, endPos, lineWidth, color);
                    break;
            }
        }
    }
}