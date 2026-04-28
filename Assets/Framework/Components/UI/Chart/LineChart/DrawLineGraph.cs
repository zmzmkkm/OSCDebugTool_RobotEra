using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LineAttribute
{
    [Header("线的样式："), Tooltip("线的颜色")] public Color lineColor = Color.white;
    [Tooltip("线的宽度")] public float lineWidth = 1;

    [Header("点位设置："), Tooltip("是否显示点位")] public bool isShowPoint = false;
    [Tooltip("点位Sprite")] public Sprite pointSprite;
    [Tooltip("点位颜色")] public Color pointColor = Color.white;
    [Tooltip("点位大小")] public Vector2 pointSize = new Vector2(20, 20);
    [Tooltip("点位偏移量")] public Vector2 pointOffset = Vector2.zero;

    [Header("点位数值："), Tooltip("是否显示点位数值")] public bool isShowPointValue = false;
    [Tooltip("点位数值字体")] public TMP_FontAsset textFont;
    [Tooltip("点位数值颜色")] public Color fontColor = Color.white;
    [Tooltip("点位数值大小")] public float fontSize = 20;
    [Tooltip("点位数值边框大小")] public Vector2 textRectSize = new Vector2(60, 21);
    [Tooltip("点位数值偏移量")] public Vector2 textOffset = new Vector2(0, 21);

    [Header("线的阴影设置："), Tooltip("是否显示线的阴影")]
    public bool isShowShadow = false;

    [Tooltip("线阴影的颜色")] public Color shadowColor = Color.black;
    [Tooltip("线阴影的宽度")] public float shadowWidth = 4;
    [Tooltip("线阴影的偏移量")] public Vector2 shadowOffset = Vector2.zero;
    [Tooltip("线的坐标数据"), HideInInspector] public List<Vector2> items = new List<Vector2>();

    [Header("线的发光设置："), Tooltip("是否显示线的阴影")]
    public bool isLuminous = false;

    [Tooltip("线发光的颜色")] public Color luminousColor = Color.white;
    [Tooltip("线发光的宽度")] public float luminousWidth = 10;

    [Header("线的填充："), Tooltip("是否显示线的填充")] public bool isShowFill = false;
    [Tooltip("线填充的颜色1")] public Color fillColor0 = Color.white;
    [Tooltip("线填充的颜色2")] public Color fillColor1 = new Color(1, 1, 1, 0);
    [Tooltip("填充是否全局渐变")] public FillType fillType = FillType.全局渐变;
    [HideInInspector] public float zeroPos = 0;

    [Header("线的类型："), Tooltip("是否使用平滑曲线")] public bool isSmoothCurve = false;
    [Tooltip("Rom样条"), Range(0, 2)] public float alpha = 0.5f;
    [Tooltip("段数"), Range(2, 30)] public int segmentsPerSegment = 10;
}

[RequireComponent(typeof(CanvasRenderer))]
public class DrawLineGraph : MaskableGraphic
{
    /// <summary>
    /// 强制刷新OnPopulateMesh
    /// </summary>
    public void RedrawMesh()
    {
        SetAllDirty(); // 强制调用OnPopulateMesh
    }

    public List<LineAttribute> lineAttributes;


    /// <summary>
    /// 重写这个类以绘制UI
    /// </summary>
    /// <param name="vh"></param>
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        DrawPosLine(vh, lineAttributes);
    }


    private void DrawPosLine(VertexHelper vh, List<LineAttribute> lineAttributes)
    {
        if (lineAttributes is { Count: > 0 })
        {
            foreach (var lineAttribute in lineAttributes)
            {
                if (lineAttribute.items is not { Count: > 0 }) continue;
                var startPos = lineAttribute.items[0];
                var itemsY = lineAttribute.items.Select(q => q.y).ToList();
                var yMax = itemsY.Max();
                var yMin = itemsY.Min();
                for (var i = 1; i < lineAttribute.items.Count; i++)
                {
                    var endPos = lineAttribute.items[i];

                    if (lineAttribute.isLuminous)
                    {
                        var upPos = i == 1 ? default : lineAttribute.items[i - 2];
                        var nextPos = i == lineAttribute.items.Count - 1 ? default : lineAttribute.items[i + 1];
                        DrawMeshGraphic.GetQuadLuminous(vh, i, upPos, startPos, endPos, nextPos, lineAttribute.luminousWidth, lineAttribute.luminousColor, new Color(0f, 0f, 0f, 0f));
                    }

                    if (lineAttribute.isShowFill)
                    {
                        DrawMeshGraphic.GetQuadFill(vh, startPos, endPos, lineAttribute.zeroPos, lineAttribute.fillColor0, lineAttribute.fillColor1, yMax, yMin, lineAttribute.fillType);
                    }

                    if (lineAttribute.isShowShadow)
                    {
                        vh.AddUIVertexQuad(DrawMeshGraphic.GetQuadUp(startPos + lineAttribute.shadowOffset, endPos + lineAttribute.shadowOffset, new Color(0f, 0f, 0f, 0f), lineAttribute.shadowColor, lineAttribute.shadowWidth));
                        vh.AddUIVertexQuad(DrawMeshGraphic.GetQuadDown(startPos + lineAttribute.shadowOffset, endPos + lineAttribute.shadowOffset, lineAttribute.shadowColor, new Color(0f, 0f, 0f, 0f), lineAttribute.shadowWidth));
                    }


                    startPos = endPos;
                }
            }

            foreach (var lineAttribute in lineAttributes)
            {
                if (lineAttribute.items is not { Count: > 0 }) continue;
                var upPos = lineAttribute.items[0];
                for (var i = 1; i < lineAttribute.items.Count; i++)
                {
                    var endPos = lineAttribute.items[i];
                    vh.AddUIVertexQuad(DrawMeshGraphic.GetQuad(upPos, endPos, lineAttribute.lineColor, lineAttribute.lineColor, lineAttribute.lineWidth));
                    upPos = endPos;
                }
            }
        }
    }
}