using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;
using Prospect.Utility.Math;

namespace XCharts.Runtime
{
    internal static class LineHelper
    {
        private static List<Vector3> s_CurvesPosList = new List<Vector3>();
        private static Vector2 lastPointIntersection = Vector2.zero;
        private static Vector2 lastPointIntersection1 = Vector2.zero;

        public static int GetDataAverageRate(Serie serie, GridCoord grid, int maxCount, bool isYAxis)
        {
            var sampleDist = serie.sampleDist;
            var rate = 0;
            var width = isYAxis ? grid.context.height : grid.context.width;
            if (sampleDist > 0)
                rate = (int)((maxCount - serie.minShow) / (width / sampleDist));
            if (rate < 1)
                rate = 1;
            return rate;
        }

        public static void DrawSerieLineArea(VertexHelper vh, Serie serie, Serie lastStackSerie,
            ThemeStyle theme, VisualMap visualMap, bool isY, Axis axis, Axis relativedAxis, GridCoord grid)
        {
            Color32 areaColor, areaToColor;
            Color32 areaShadowColor, areaShadowToColor;
            bool innerFill, toTop;
            bool isNormalArea;
            bool isOpenCoeifficient;
            float areaDisCoeifficient;
            float coeifficient;
            bool isOpenAreaShadow;
            float areaShadowDisCoeifficient;
            float areaShadowCoeifficient;
            if (!SerieHelper.GetAreaColor(out areaColor, out areaToColor,out areaShadowColor,out areaShadowToColor ,out innerFill, out toTop,out isNormalArea,out isOpenCoeifficient,out areaDisCoeifficient,out coeifficient,out isOpenAreaShadow,out areaShadowDisCoeifficient,out areaShadowCoeifficient,serie, null, theme, serie.context.colorIndex))
            {
                return;
            } 


            if (innerFill)
            {
                UGL.DrawPolygon(vh, serie.context.dataPoints, areaColor);
                return;
            }

            var gridXY = (isY ? grid.context.x : grid.context.y);
            if (lastStackSerie == null)
            {
                if (isNormalArea)
                {
                    DrawSerieLineNormalArea(vh, serie, isY,
                    gridXY + relativedAxis.context.offset,
                    gridXY,
                    gridXY + (isY ? grid.context.width : grid.context.height),
                    areaColor,
                    areaToColor,
                    visualMap,
                    axis,
                    relativedAxis,
                    grid,
                    toTop);
                }
                //开启外发光系数
                if (isOpenCoeifficient)
                {
                    DrawSerieLineNormalAreaCoeifficient(vh, serie, isY,                 
                        areaColor,
                        areaToColor,               
                        grid,
                        areaDisCoeifficient,
                        coeifficient);
                }

                if (isOpenAreaShadow)
                {
                    DrawSerieLineNormalAreaShadow(vh, serie, isY,
                    areaShadowColor,
                    areaShadowToColor,
                    grid,
                    areaShadowDisCoeifficient,
                    areaShadowCoeifficient);
                }
            }
            else
            {
                DrawSerieLineStackArea(vh, serie, lastStackSerie, isY,
                    gridXY + relativedAxis.context.offset,
                    gridXY,
                    gridXY + (isY ? grid.context.width : grid.context.height),
                    areaColor,
                    areaToColor,
                    visualMap,
                    toTop);
            }
        }

        /// <summary>
        /// 绘制常规Area区域
        /// </summary>
        /// <param name="vh">顶点</param>
        /// <param name="serie">系列</param>
        /// <param name="isY">是否以Y轴进行绘制</param>
        /// <param name="zero">原点</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="areaColor">起始色--底部</param>
        /// <param name="areaToColor">末色--顶部</param>
        /// <param name="visualMap"></param>
        /// <param name="axis">轴</param>
        /// <param name="relativedAxis">相对轴</param>
        /// <param name="grid"></param>
        /// <param name="toTop">是否绘制到顶部</param>
        private static void DrawSerieLineNormalArea(VertexHelper vh, Serie serie, bool isY,
            float zero, float min, float max, Color32 areaColor, Color32 areaToColor,
            VisualMap visualMap, Axis axis, Axis relativedAxis, GridCoord grid, bool toTop)
        {
            var points = serie.context.drawPoints;
            var count = points.Count;
            if (count < 2)
                return;

            var isBreak = false;
            var lp = Vector3.zero;
            var isVisualMapGradient = VisualMapHelper.IsNeedAreaGradient(visualMap);
            var areaLerp = !ChartHelper.IsValueEqualsColor(areaColor, areaToColor);
            var zsp = isY ?
                new Vector3(zero, points[0].position.y) :
                new Vector3(points[0].position.x, zero);
            var zep = isY ?
                new Vector3(zero, points[count - 1].position.y) :
                new Vector3(points[count - 1].position.x, zero);

            var lastDataIsIgnore = false;
            for (int i = 0; i < points.Count; i++)
            {
                var pdata = points[i];
                var tp = pdata.position;
                if (serie.clip)
                {
                    grid.Clamp(ref tp);
                }
                var isIgnore = pdata.isIgnoreBreak;
                var color = areaColor;
                var toColor = areaToColor;
                var lerp = areaLerp;

                if (serie.animation.CheckDetailBreak(tp, isY))
                {
                    isBreak = true;

                    var progress = serie.animation.GetCurrDetail();
                    var ip = Vector3.zero;
                    var axisStartPos = isY ? new Vector3(-10000, progress) : new Vector3(progress, -10000);
                    var axisEndPos = isY ? new Vector3(10000, progress) : new Vector3(progress, 10000);

                    if (UGLHelper.GetIntersection(lp, tp, axisStartPos, axisEndPos, ref ip))
                        tp = ip;
                }
 
                var zp = isY ? new Vector3(zero, tp.y) : new Vector3(tp.x, zero);                
                if (isVisualMapGradient)
                {
                    color = VisualMapHelper.GetLineGradientColor(visualMap, zp, grid, axis, relativedAxis, areaColor);
                    toColor = VisualMapHelper.GetLineGradientColor(visualMap, tp, grid, axis, relativedAxis, areaToColor);
                    lerp = true;
                }
                if (i > 0)
                {
                    if ((lp.y - zero > 0 && tp.y - zero < 0) || (lp.y - zero < 0 && tp.y - zero > 0))
                    {
                        var ip = Vector3.zero;
                        if (UGLHelper.GetIntersection(lp, tp, zsp, zep, ref ip))
                        {
                            if (lerp)
                                AddVertToVertexHelperWithLerpColor(vh, ip, ip, color, toColor, isY, min, max, i > 0, toTop);
                            else
                            {
                                if (lastDataIsIgnore)
                                    UGL.AddVertToVertexHelper(vh, ip, ip, ColorUtil.clearColor32, true);

                                UGL.AddVertToVertexHelper(vh, ip, ip, toColor, color, i > 0);

                                if (isIgnore)
                                    UGL.AddVertToVertexHelper(vh, ip, ip, ColorUtil.clearColor32, true);
                            }
                        }
                    }
                }

                if (lerp)
                    AddVertToVertexHelperWithLerpColor(vh, tp, zp, color, toColor, isY, min, max, i > 0, toTop);
                else
                {
                    if (lastDataIsIgnore)
                        UGL.AddVertToVertexHelper(vh, tp, zp, ColorUtil.clearColor32, true);

                    UGL.AddVertToVertexHelper(vh, tp, zp, toColor, color, i > 0);

                    if (isIgnore)
                        UGL.AddVertToVertexHelper(vh, tp, zp, ColorUtil.clearColor32, true);
                }
                lp = tp;
                lastDataIsIgnore = isIgnore;
                if (isBreak)
                    break;
            }
        }

        /// <summary>
        /// 增加扩展外发光渐变系数
        /// </summary>
        private static void DrawSerieLineNormalAreaCoeifficient(VertexHelper vh, Serie serie, bool isY,
            Color32 areaColor, Color32 areaToColor,GridCoord grid,float areaDisCoefficient = 1f,float coefficient = 0f)
        {
            var points = serie.context.drawPoints;
            var count = points.Count;
            if (count < 2)
                return;
            var isBreak = false;
            var lp = Vector3.zero;

            for (int i = 0; i < points.Count; i++)
            {
                var pdata = points[i];
                var tp = pdata.position;
                PointInfo lastPdata;
                Vector3 lastTp = Vector3.zero;
                PointInfo nextPData;
                Vector3 nextTp = Vector3.zero;
                if (i != points.Count - 1)
                {
                    nextPData = points[i + 1];
                    nextTp = nextPData.position;
                }

                if (i != 0)
                {
                    lastPdata = points[i - 1];
                    lastTp = lastPdata.position;
                }

                if (serie.clip)
                {
                    grid.Clamp(ref tp);
                }

                var color = areaColor;
                var toColor = areaToColor;

                if (serie.animation.CheckDetailBreak(tp, isY))
                {
                    isBreak = true;
                    var progress = serie.animation.GetCurrDetail();
                    var ip = Vector3.zero;
                    var axisStartPos = isY ? new Vector3(-10000, progress) : new Vector3(progress, -10000);
                    var axisEndPos = isY ? new Vector3(10000, progress) : new Vector3(progress, 10000);
                    if (UGLHelper.GetIntersection(lp, tp, axisStartPos, axisEndPos, ref ip))
                        tp = ip;
                }

                #region -- 绘制图形

                if (i > 0)
                {
                    Vector2 startPerpendicularPoint = MathfUtility.CalcPerpendicularPointToStart(lastTp, tp, areaDisCoefficient);
                    Vector2 endPerpendicularPoint = MathfUtility.CalcPerpendicularPointToEnd(lastTp, tp, areaDisCoefficient);
                    Vector2 startNextPerpendicularPoint = MathfUtility.CalcPerpendicularPointToStart(tp, nextTp, areaDisCoefficient);
                    Vector2 endNextPerpendicularPoint = MathfUtility.CalcPerpendicularPointToEnd(tp, nextTp, areaDisCoefficient);
                    Vector2 mPointIntersection = MathfUtility.CalcPointIntersection(startPerpendicularPoint, endPerpendicularPoint, startNextPerpendicularPoint, endNextPerpendicularPoint);

                    Vector2 startPerpendicularPoint1 = MathfUtility.CalcPerpendicularPointToStart(lastTp, tp, coefficient);
                    Vector2 endPerpendicularPoint1 = MathfUtility.CalcPerpendicularPointToEnd(lastTp, tp, coefficient);
                    Vector2 startNextPerpendicularPoint1 = MathfUtility.CalcPerpendicularPointToStart(tp, nextTp, coefficient);
                    Vector2 endNextPerpendicularPoint1 = MathfUtility.CalcPerpendicularPointToEnd(tp, nextTp, coefficient);
                    Vector2 mPointIntersection1 = MathfUtility.CalcPointIntersection(startPerpendicularPoint1, endPerpendicularPoint1, startNextPerpendicularPoint1, endNextPerpendicularPoint1);

                    //Debug.Log($"数据点tp: {tp} 交点： {mPointIntersection}");

                    if (i == 1)
                    {
                        Vector2 v2 = MathfUtility.CalcPointIntersection(lastTp, new Vector2(lastTp.x, lastTp.y - 10f), startPerpendicularPoint, endPerpendicularPoint);
                        Vector2 v2_NextPoint = MathfUtility.CalcPointIntersection(lastTp, new Vector2(lastTp.x, lastTp.y - 10f), startPerpendicularPoint1, endPerpendicularPoint1);
                        vh.AddUIVertexQuad(GetQuad(v2,v2_NextPoint, mPointIntersection1, mPointIntersection, color, toColor));
                    }
                    if (i > 1)
                    {          
                        if (i == points.Count - 1)
                        {
                            Vector2 v2_End = MathfUtility.CalcPointIntersection(startPerpendicularPoint, endPerpendicularPoint, new Vector2(tp.x, tp.y - 10f),tp);
                            Vector2 v2_EndNextPoint = MathfUtility.CalcPointIntersection(startPerpendicularPoint1, endPerpendicularPoint1, new Vector2(tp.x, tp.y - 10f), tp);
                            vh.AddUIVertexQuad(GetQuad(lastPointIntersection, lastPointIntersection1, v2_EndNextPoint, v2_End, color, toColor));
                        }
                        else
                        {
                            vh.AddUIVertexQuad(GetQuad(lastPointIntersection, lastPointIntersection1, mPointIntersection1, mPointIntersection, color, toColor));
                        }
                    }
                    lastPointIntersection = mPointIntersection;
                    lastPointIntersection1 = mPointIntersection1;
                }                

                UIVertex[] GetQuad(Vector2 first, Vector2 second, Vector2 third, Vector2 four, Color color,Color toColor)
                {
                    var vertexs = new UIVertex[4];
                    vertexs[0] = GetUIVertex(first, color);
                    vertexs[1] = GetUIVertex(second, toColor);
                    vertexs[2] = GetUIVertex(third, toColor);
                    vertexs[3] = GetUIVertex(four, color);
                    return vertexs;
                }

                UIVertex GetUIVertex(Vector2 point,Color color)
                { 
                    var vertex = new UIVertex 
                    {
                        position = point,
                        color = color,
                        uv0 = Vector2.zero
                    };
                    return vertex;
                }  

                #endregion
            }
        }


        /// <summary>
        /// 增加扩展阴影系数
        /// </summary>
        private static void DrawSerieLineNormalAreaShadow(VertexHelper vh, Serie serie, bool isY,
             Color32 areaColor, Color32 areaToColor,GridCoord grid,float shadowDisCoefficient = 1f,float coefficient = 0f)
        {
            var points = serie.context.drawPoints;
            var count = points.Count;
            if (count < 2)
                return;
            var isBreak = false;
            var lp = Vector3.zero;

            for (int i = 0; i < points.Count; i++)
            {
                var pdata = points[i];
                var tp = pdata.position;
                PointInfo lastPdata;
                Vector3 lastTp = Vector3.zero;
                PointInfo nextPData;
                Vector3 nextTp = Vector3.zero;
                if (i != points.Count - 1)
                {
                    nextPData = points[i + 1];
                    nextTp = nextPData.position;
                }

                if (i != 0)
                {
                    lastPdata = points[i - 1];
                    lastTp = lastPdata.position;
                }

                if (serie.clip)
                {
                    grid.Clamp(ref tp);
                }

                var color = areaColor;
                var toColor = areaToColor;

                if (serie.animation.CheckDetailBreak(tp, isY))
                {
                    isBreak = true;
                    var progress = serie.animation.GetCurrDetail();
                    var ip = Vector3.zero;
                    var axisStartPos = isY ? new Vector3(-10000, progress) : new Vector3(progress, -10000);
                    var axisEndPos = isY ? new Vector3(10000, progress) : new Vector3(progress, 10000);
                    if (UGLHelper.GetIntersection(lp, tp, axisStartPos, axisEndPos, ref ip))
                        tp = ip;
                }

                #region -- 绘制图形

                if (i > 0)
                {
                    Vector2 startPerpendicularPoint = MathfUtility.CalcPerpendicularPointToStart(lastTp, tp, shadowDisCoefficient);
                    Vector2 endPerpendicularPoint = MathfUtility.CalcPerpendicularPointToEnd(lastTp, tp, shadowDisCoefficient);
                    Vector2 startNextPerpendicularPoint = MathfUtility.CalcPerpendicularPointToStart(tp, nextTp, shadowDisCoefficient);
                    Vector2 endNextPerpendicularPoint = MathfUtility.CalcPerpendicularPointToEnd(tp, nextTp, shadowDisCoefficient);
                    Vector2 mPointIntersection = MathfUtility.CalcPointIntersection(startPerpendicularPoint, endPerpendicularPoint, startNextPerpendicularPoint, endNextPerpendicularPoint);

                    Vector2 startPerpendicularPoint1 = MathfUtility.CalcPerpendicularPointToStart(lastTp, tp, coefficient);
                    Vector2 endPerpendicularPoint1 = MathfUtility.CalcPerpendicularPointToEnd(lastTp, tp, coefficient);
                    Vector2 startNextPerpendicularPoint1 = MathfUtility.CalcPerpendicularPointToStart(tp, nextTp, coefficient);
                    Vector2 endNextPerpendicularPoint1 = MathfUtility.CalcPerpendicularPointToEnd(tp, nextTp, coefficient);
                    Vector2 mPointIntersection1 = MathfUtility.CalcPointIntersection(startPerpendicularPoint1, endPerpendicularPoint1, startNextPerpendicularPoint1, endNextPerpendicularPoint1);

                    //Debug.Log($"数据点tp: {tp} 交点： {mPointIntersection}");

                    if (i == 1)
                    {
                        Vector2 v2 = MathfUtility.CalcPointIntersection(lastTp, new Vector2(lastTp.x, lastTp.y - 10f), startPerpendicularPoint, endPerpendicularPoint);
                        Vector2 v2_NextPoint = MathfUtility.CalcPointIntersection(lastTp, new Vector2(lastTp.x, lastTp.y - 10f), startPerpendicularPoint1, endPerpendicularPoint1);
                        vh.AddUIVertexQuad(GetQuad(v2, v2_NextPoint, mPointIntersection1, mPointIntersection, color, toColor));
                    }
                    if (i > 1)
                    {
                        if (i == points.Count - 1)
                        {
                            Vector2 v2_End = MathfUtility.CalcPointIntersection(startPerpendicularPoint, endPerpendicularPoint, new Vector2(tp.x, tp.y - 10f), tp);
                            Vector2 v2_EndNextPoint = MathfUtility.CalcPointIntersection(startPerpendicularPoint1, endPerpendicularPoint1, new Vector2(tp.x, tp.y - 10f), tp);
                            vh.AddUIVertexQuad(GetQuad(lastPointIntersection, lastPointIntersection1, v2_EndNextPoint, v2_End, color, toColor));
                        }
                        else
                        {
                            vh.AddUIVertexQuad(GetQuad(lastPointIntersection, lastPointIntersection1, mPointIntersection1, mPointIntersection, color, toColor));
                        }
                    }
                    lastPointIntersection = mPointIntersection;
                    lastPointIntersection1 = mPointIntersection1;
                }

                UIVertex[] GetQuad(Vector2 first, Vector2 second, Vector2 third, Vector2 four, Color color, Color toColor)
                {
                    var vertexs = new UIVertex[4];
                    vertexs[0] = GetUIVertex(first, color);
                    vertexs[1] = GetUIVertex(second, toColor);
                    vertexs[2] = GetUIVertex(third, toColor);
                    vertexs[3] = GetUIVertex(four, color);
                    return vertexs;
                }

                UIVertex GetUIVertex(Vector2 point, Color color)
                {
                    var vertex = new UIVertex
                    {
                        position = point,
                        color = color,
                        uv0 = Vector2.zero
                    };
                    return vertex;
                }

                #endregion
            }
        }


        /// <summary>
        /// 增加扩展渐变系数 -- bak
        /// </summary>
        private static void DrawSerieLineNormalArea(VertexHelper vh, Serie serie, bool isY,
            float zero, float min, float max, Color32 areaColor, Color32 areaToColor,
            VisualMap visualMap, Axis axis, Axis relativedAxis, GridCoord grid, bool toTop,bool isOpenCoefficient = true,float coefficient = 0f)
        {
            var points = serie.context.drawPoints;
            var count = points.Count;
            if (count < 2)
                return;
            
            var isBreak = false;
            var lp = Vector3.zero;
            var isVisualMapGradient = VisualMapHelper.IsNeedAreaGradient(visualMap);
            var areaLerp = !ChartHelper.IsValueEqualsColor(areaColor, areaToColor);
            var zsp = isY ?
                new Vector3(zero, points[0].position.y) :
                new Vector3(points[0].position.x, zero);
            var zep = isY ?
                new Vector3(zero, points[count - 1].position.y) :
                new Vector3(points[count - 1].position.x, zero);

            var lastDataIsIgnore = false;
            for (int i = 0; i < points.Count; i++)
            {       
                var pdata = points[i];   
                var tp = pdata.position;
                PointInfo lastPdata;
                Vector3 lastTp = Vector3.zero;
                PointInfo nextPData;
                Vector3 nextTp = Vector3.zero;
                if (i != points.Count - 1)
                {
                    nextPData = points[i + 1];
                    nextTp = nextPData.position;
                }

                if (i != 0)
                {
                    lastPdata = points[i - 1];
                    lastTp = lastPdata.position;
                }
                
                if (serie.clip)
                {
                    grid.Clamp(ref tp);
                }
                var isIgnore = pdata.isIgnoreBreak;
                var color = areaColor;
                var toColor = areaToColor;
                var lerp = areaLerp;

                if (serie.animation.CheckDetailBreak(tp, isY))
                {
                    isBreak = true;

                    var progress = serie.animation.GetCurrDetail();
                    var ip = Vector3.zero;
                    var axisStartPos = isY ? new Vector3(-10000, progress) : new Vector3(progress, -10000);
                    var axisEndPos = isY ? new Vector3(10000, progress) : new Vector3(progress, 10000);

                    if (UGLHelper.GetIntersection(lp, tp, axisStartPos, axisEndPos, ref ip))
                        tp = ip;
                }

                Vector3 zp = isY ? new Vector3(zero, tp.y) : new Vector3(tp.x, tp.y - 50 * coefficient);
                if (isVisualMapGradient)
                {
                    color = VisualMapHelper.GetLineGradientColor(visualMap, zp, grid, axis, relativedAxis, areaColor);
                    toColor = VisualMapHelper.GetLineGradientColor(visualMap, tp, grid, axis, relativedAxis, areaToColor);
                    lerp = true;
                }
                if (i > 0)
                {
                    if ((lp.y - zero > 0 && tp.y - zero < 0) || (lp.y - zero < 0 && tp.y - zero > 0))
                    {
                        var ip = Vector3.zero;
                        if (UGLHelper.GetIntersection(lp, tp, zsp, zep, ref ip))
                        {
                            if (lerp)
                                AddVertToVertexHelperWithLerpColor(vh, ip, ip, color, toColor, isY, min, max, i > 0, toTop);
                            else
                            {
                                if (lastDataIsIgnore)
                                    UGL.AddVertToVertexHelper(vh, ip, ip, ColorUtil.clearColor32, true);

                                UGL.AddVertToVertexHelper(vh, ip, ip, toColor, color, i > 0);

                                if (isIgnore)
                                    UGL.AddVertToVertexHelper(vh, ip, ip, ColorUtil.clearColor32, true);
                            }
                        }
                    }
                }

                if (lerp)
                    AddVertToVertexHelperWithLerpColor(vh, tp, zp, color, toColor, isY, min, max, i > 0, toTop);
                else
                {
                    if (lastDataIsIgnore)
                        UGL.AddVertToVertexHelper(vh, tp, zp, ColorUtil.clearColor32, true);

                    UGL.AddVertToVertexHelper(vh, tp, zp, toColor, color, i > 0);

                    if (isIgnore)
                        UGL.AddVertToVertexHelper(vh, tp, zp, ColorUtil.clearColor32, true);
                }
                lp = tp;
                lastDataIsIgnore = isIgnore;
                if (isBreak)
                    break;
            }
        }

        private static void DrawSerieLineStackArea(VertexHelper vh, Serie serie, Serie lastStackSerie, bool isY,
            float zero, float min, float max, Color32 color, Color32 toColor, VisualMap visualMap, bool toTop)
        {
            if (lastStackSerie == null)
                return;

            var upPoints = serie.context.drawPoints;
            var downPoints = lastStackSerie.context.drawPoints;
            var upCount = upPoints.Count;
            var downCount = downPoints.Count;

            if (upCount <= 0 || downCount <= 0)
                return;

            var lerp = !ChartHelper.IsValueEqualsColor(color, toColor);
            var ltp = upPoints[0].position;
            var lbp = downPoints[0].position;

            if (lerp)
                AddVertToVertexHelperWithLerpColor(vh, ltp, lbp, color, toColor, isY, min, max, false, toTop);
            else
                UGL.AddVertToVertexHelper(vh, ltp, lbp, color, false);

            int u = 1, d = 1;
            var isBreakTop = false;
            var isBreakBottom = false;

            while ((u < upCount || d < downCount))
            {
                var tp = u < upCount ? upPoints[u].position : upPoints[upCount - 1].position;
                var bp = d < downCount ? downPoints[d].position : downPoints[downCount - 1].position;

                var tnp = (u + 1) < upCount ? upPoints[u + 1].position : upPoints[upCount - 1].position;
                var bnp = (d + 1) < downCount ? downPoints[d + 1].position : downPoints[downCount - 1].position;

                if (serie.animation.CheckDetailBreak(tp, isY))
                {
                    isBreakTop = true;

                    var progress = serie.animation.GetCurrDetail();
                    var ip = Vector3.zero;

                    if (UGLHelper.GetIntersection(ltp, tp,
                            new Vector3(progress, -10000),
                            new Vector3(progress, 10000), ref ip))
                        tp = ip;
                    else
                        tp = new Vector3(progress, tp.y);
                }
                if (serie.animation.CheckDetailBreak(bp, isY))
                {
                    isBreakBottom = true;

                    var progress = serie.animation.GetCurrDetail();
                    var ip = Vector3.zero;

                    if (UGLHelper.GetIntersection(lbp, bp,
                            new Vector3(progress, -10000),
                            new Vector3(progress, 10000), ref ip))
                        bp = ip;
                    else
                        bp = new Vector3(progress, bp.y);
                }

                if (lerp)
                    AddVertToVertexHelperWithLerpColor(vh, tp, bp, color, toColor, isY, min, max, true, toTop);
                else
                    UGL.AddVertToVertexHelper(vh, tp, bp, color, true);
                u++;
                d++;
                if (bp.x < tp.x && bnp.x < tp.x)
                    u--;
                if (tp.x < bp.x && tnp.x < bp.x)
                    d--;

                ltp = tp;
                lbp = bp;
                if (isBreakTop && isBreakBottom)
                    break;
            }
        }

        private static void AddVertToVertexHelperWithLerpColor(VertexHelper vh, Vector3 tp, Vector3 bp,
            Color32 color, Color32 toColor, bool isY, float min, float max, bool needTriangle, bool toTop)
        {
            if (toTop)
            {
                var range = max - min;
                var color1 = Color32.Lerp(color, toColor, ((isY ? tp.x : tp.y) - min) / range);
                var color2 = Color32.Lerp(color, toColor, ((isY ? bp.x : bp.y) - min) / range);

                UGL.AddVertToVertexHelper(vh, tp, bp, color1, color2, needTriangle);
            }
            else
            {
                UGL.AddVertToVertexHelper(vh, tp, bp, toColor, color, needTriangle);
            }
        }

        internal static void DrawSerieLine(VertexHelper vh, ThemeStyle theme, Serie serie, VisualMap visualMap,
            GridCoord grid, Axis axis, Axis relativedAxis, float lineWidth)
        {
            if (!serie.lineStyle.show || serie.lineStyle.type == LineStyle.Type.None)
                return;

            var datas = serie.context.drawPoints;

            var dataCount = datas.Count;
            if (dataCount < 2)
                return;

            var ltp = Vector3.zero;
            var lbp = Vector3.zero;
            var ntp = Vector3.zero;
            var nbp = Vector3.zero;
            var itp = Vector3.zero;
            var ibp = Vector3.zero;
            var clp = Vector3.zero;
            var crp = Vector3.zero;

            var isBreak = false;
            var isY = axis is YAxis;
            var isVisualMapGradient = VisualMapHelper.IsNeedLineGradient(visualMap);
            var isLineStyleGradient = serie.lineStyle.IsNeedGradient();
            var lineColor = SerieHelper.GetLineColor(serie, null, theme, serie.context.colorIndex);

            var lastDataIsIgnore = datas[0].isIgnoreBreak;
            var firstInGridPointIndex = serie.clip ? -1 : 1;
            var segmentCount = 0;
            var dashLength = serie.lineStyle.dashLength;
            var gapLength = serie.lineStyle.gapLength;
            var dotLength = serie.lineStyle.dotLength;
            for (int i = 1; i < dataCount; i++)
            {
                var cdata = datas[i];
                var isIgnore = cdata.isIgnoreBreak;
                var cp = cdata.position;
                var lp = datas[i - 1].position;

                var np = i == dataCount - 1 ? cp : datas[i + 1].position;
                if (serie.animation.CheckDetailBreak(cp, isY))
                {
                    isBreak = true;
                    var ip = Vector3.zero;
                    var progress = serie.animation.GetCurrDetail();
                    if (AnimationStyleHelper.GetAnimationPosition(serie.animation, isY, lp, cp, progress, ref ip))
                        cp = np = ip;
                }
                serie.context.lineEndPostion = cp;
                serie.context.lineEndValue = AxisHelper.GetAxisPositionValue(grid, relativedAxis, cp);
                var handled = false;
                var isClip = false;
                if (serie.clip)
                {
                    if (!grid.Contains(cp))
                        isClip = true;
                    else if (firstInGridPointIndex <= 0)
                        firstInGridPointIndex = i;
                    if (isClip) isIgnore = true;
                }
                if (serie.lineStyle.type == LineStyle.Type.None)
                {
                    handled = true;
                    break;
                }
                {
                    segmentCount++;
                    var index = 0f;
                    switch (serie.lineStyle.type)
                    {
                        case LineStyle.Type.Dashed:
                            index = segmentCount % (dashLength + gapLength);
                            if (index >= dashLength)
                                isIgnore = true;
                            break;
                        case LineStyle.Type.Dotted:
                            index = segmentCount % (dotLength + gapLength);
                            if (index >= dotLength)
                                isIgnore = true;
                            break;
                        case LineStyle.Type.DashDot:
                            index = segmentCount % (dashLength + dotLength + 2 * gapLength);
                            if (index >= dashLength && index < dashLength + gapLength)
                                isIgnore = true;
                            else if (index >= dashLength + gapLength + dotLength)
                                isIgnore = true;
                            break;
                        case LineStyle.Type.DashDotDot:
                            index = segmentCount % (dashLength + 2 * dotLength + 3 * gapLength);
                            if (index >= dashLength && index < dashLength + gapLength)
                                isIgnore = true;
                            else if (index >= dashLength + gapLength + dotLength && index < dashLength + dotLength + 2 * gapLength)
                                isIgnore = true;
                            else if (index >= dashLength + 2 * gapLength + 2 * dotLength)
                                isIgnore = true;
                            break;
                    }
                }

                if (handled)
                {
                    lastDataIsIgnore = isIgnore;
                    if (isBreak)
                        break;
                    else
                        continue;
                }
                bool bitp = true, bibp = true;
                UGLHelper.GetLinePoints(lp, cp, np, lineWidth,
                    ref ltp, ref lbp,
                    ref ntp, ref nbp,
                    ref itp, ref ibp,
                    ref clp, ref crp,
                    ref bitp, ref bibp, i);

                if (i == 1)
                {
                    if (isClip) lastDataIsIgnore = true;
                    AddLineVertToVertexHelper(vh, ltp, lbp, lineColor, isVisualMapGradient, isLineStyleGradient,
                        visualMap, serie.lineStyle, grid, axis, relativedAxis, false, lastDataIsIgnore, isIgnore);
                    if (dataCount == 2 || isBreak)
                    {
                        AddLineVertToVertexHelper(vh, clp, crp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                        serie.context.lineEndPostion = cp;
                        serie.context.lineEndValue = AxisHelper.GetAxisPositionValue(grid, relativedAxis, cp);
                        break;
                    }
                }

                if (bitp == bibp)
                {
                    if (bitp)
                        AddLineVertToVertexHelper(vh, itp, ibp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                    else
                    {
                        AddLineVertToVertexHelper(vh, ltp, clp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                        AddLineVertToVertexHelper(vh, ltp, crp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                    }
                }
                else
                {
                    if (bitp)
                    {
                        AddLineVertToVertexHelper(vh, itp, clp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                        AddLineVertToVertexHelper(vh, itp, crp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                    }
                    else if (bibp)
                    {
                        AddLineVertToVertexHelper(vh, clp, ibp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                        AddLineVertToVertexHelper(vh, crp, ibp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                    }
                }
                lastDataIsIgnore = isIgnore;
                if (isBreak)
                    break;
            }
        }

        public static float GetLineWidth(ref bool interacting, Serie serie, float defaultWidth)
        {
            var lineWidth = 0f;
            if (!serie.interact.TryGetValue(ref lineWidth, ref interacting, serie.animation.GetInteractionDuration()))
            {
                lineWidth = serie.lineStyle.GetWidth(defaultWidth);
                serie.interact.SetValue(ref interacting, lineWidth);
            }
            return lineWidth;
        }

        private static void AddLineVertToVertexHelper(VertexHelper vh, Vector3 tp, Vector3 bp,
            Color32 lineColor, bool visualMapGradient, bool lineStyleGradient, VisualMap visualMap,
            LineStyle lineStyle, GridCoord grid, Axis axis, Axis relativedAxis, bool needTriangle,
            bool lastIgnore, bool ignore)
        {
            if (lastIgnore && needTriangle)
                UGL.AddVertToVertexHelper(vh, tp, bp, ColorUtil.clearColor32, true);

            if (visualMapGradient)
            {
                var color1 = VisualMapHelper.GetLineGradientColor(visualMap, tp, grid, axis, relativedAxis, lineColor);
                var color2 = VisualMapHelper.GetLineGradientColor(visualMap, bp, grid, axis, relativedAxis, lineColor);
                UGL.AddVertToVertexHelper(vh, tp, bp, color1, color2, needTriangle);
            }
            else if (lineStyleGradient)
            {
                var color1 = VisualMapHelper.GetLineStyleGradientColor(lineStyle, tp, grid, axis, lineColor);
                var color2 = VisualMapHelper.GetLineStyleGradientColor(lineStyle, bp, grid, axis, lineColor);
                UGL.AddVertToVertexHelper(vh, tp, bp, color1, color2, needTriangle);
            }
            else
            {
                UGL.AddVertToVertexHelper(vh, tp, bp, lineColor, needTriangle);
            }
            if (lastIgnore && !needTriangle)
            {
                UGL.AddVertToVertexHelper(vh, tp, bp, ColorUtil.clearColor32, false);
            }
            if (ignore && needTriangle)
            {
                UGL.AddVertToVertexHelper(vh, tp, bp, ColorUtil.clearColor32, false);
            }
        }

        internal static void UpdateSerieDrawPoints(Serie serie, Settings setting, ThemeStyle theme, VisualMap visualMap,
            float lineWidth, bool isY = false)
        {
            serie.context.drawPoints.Clear();
            var last = Vector3.zero;
            switch (serie.lineType)
            {
                case LineType.Smooth:
                    UpdateSmoothLineDrawPoints(serie, setting, isY);
                    break;
                case LineType.StepStart:
                case LineType.StepMiddle:
                case LineType.StepEnd:
                    UpdateStepLineDrawPoints(serie, setting, theme, isY, lineWidth);
                    break;
                default:
                    UpdateNormalLineDrawPoints(serie, setting, visualMap);
                    break;
            }
        }

        private static void UpdateNormalLineDrawPoints(Serie serie, Settings setting, VisualMap visualMap)
        {
            var isVisualMapGradient = VisualMapHelper.IsNeedGradient(visualMap);
            if (isVisualMapGradient || serie.clip || (serie.lineStyle.IsNotSolidLine()))
            {
                var dataPoints = serie.context.dataPoints;
                if (dataPoints.Count > 1)
                {
                    var sp = dataPoints[0];
                    for (int i = 1; i < dataPoints.Count; i++)
                    {
                        var ep = dataPoints[i];
                        var ignore = serie.context.dataIgnores[i];

                        var dir = (ep - sp).normalized;
                        var dist = Vector3.Distance(sp, ep);
                        var segment = (int)(dist / setting.lineSegmentDistance);
                        serie.context.drawPoints.Add(new PointInfo(sp, ignore));
                        for (int j = 1; j < segment; j++)
                        {
                            var np = sp + dir * dist * j / segment;
                            serie.context.drawPoints.Add(new PointInfo(np, ignore));
                        }
                        sp = ep;
                        if (i == dataPoints.Count - 1)
                        {
                            serie.context.drawPoints.Add(new PointInfo(ep, ignore));
                        }
                    }
                }
                else
                {
                    serie.context.drawPoints.Add(new PointInfo(dataPoints[0], serie.context.dataIgnores[0]));
                }
            }
            else
            {
                for (int i = 0; i < serie.context.dataPoints.Count; i++)
                {
                    serie.context.drawPoints.Add(new PointInfo(serie.context.dataPoints[i], serie.context.dataIgnores[i]));
                }
            }
        }

        private static void UpdateSmoothLineDrawPoints(Serie serie, Settings setting, bool isY)
        {
            var points = serie.context.dataPoints;
            float smoothness = setting.lineSmoothness;
            for (int i = 0; i < points.Count - 1; i++)
            {
                var sp = points[i];
                var ep = points[i + 1];
                var lsp = i > 0 ? points[i - 1] : sp;
                var nep = i < points.Count - 2 ? points[i + 2] : ep;
                var ignore = serie.context.dataIgnores[i];
                if (isY)
                    UGLHelper.GetBezierListVertical(ref s_CurvesPosList, sp, ep, smoothness, setting.lineSmoothStyle);
                else
                    UGLHelper.GetBezierList(ref s_CurvesPosList, sp, ep, lsp, nep, smoothness, setting.lineSmoothStyle, serie.smoothLimit);
                for (int j = 1; j < s_CurvesPosList.Count; j++)
                {
                    serie.context.drawPoints.Add(new PointInfo(s_CurvesPosList[j], ignore));
                }
            }
        }

        private static void UpdateStepLineDrawPoints(Serie serie, Settings setting, ThemeStyle theme, bool isY, float lineWidth)
        {
            var points = serie.context.dataPoints;
            var lp = points[0];
            serie.context.drawPoints.Clear();
            serie.context.drawPoints.Add(new PointInfo(lp, serie.context.dataIgnores[0]));
            for (int i = 1; i < points.Count; i++)
            {
                var cp = points[i];
                var ignore = serie.context.dataIgnores[i];
                if ((isY && Mathf.Abs(lp.x - cp.x) <= lineWidth) ||
                    (!isY && Mathf.Abs(lp.y - cp.y) <= lineWidth))
                {
                    serie.context.drawPoints.Add(new PointInfo(cp, ignore));
                    lp = cp;
                    continue;
                }
                switch (serie.lineType)
                {
                    case LineType.StepStart:
                        serie.context.drawPoints.Add(new PointInfo(isY ?
                            new Vector3(cp.x, lp.y) :
                            new Vector3(lp.x, cp.y), ignore));
                        break;
                    case LineType.StepMiddle:
                        serie.context.drawPoints.Add(new PointInfo(isY ?
                            new Vector3(lp.x, (lp.y + cp.y) / 2) :
                            new Vector3((lp.x + cp.x) / 2, lp.y), ignore));
                        serie.context.drawPoints.Add(new PointInfo(isY ?
                            new Vector3(cp.x, (lp.y + cp.y) / 2) :
                            new Vector3((lp.x + cp.x) / 2, cp.y), ignore));
                        break;
                    case LineType.StepEnd:
                        serie.context.drawPoints.Add(new PointInfo(isY ?
                            new Vector3(lp.x, cp.y) :
                            new Vector3(cp.x, lp.y), ignore));
                        break;
                }
                serie.context.drawPoints.Add(new PointInfo(cp, ignore));
                lp = cp;
            }
        }
    }
}