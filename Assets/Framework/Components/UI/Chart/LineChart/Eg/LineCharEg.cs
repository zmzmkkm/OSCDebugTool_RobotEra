using System;
using System.Collections;
using System.Collections.Generic;
using Prospect;
using UnityEngine;
using Random = UnityEngine.Random;

public class LineCharEg : MonoBehaviour
{
    private LineCharCtrl _lineCharCtrl;
    private readonly List<Vector2> _values = new List<Vector2>();

    private void Awake()
    {
        _lineCharCtrl = GetComponent<LineCharCtrl>();

        _lineCharCtrl.Init();
        _lineCharCtrl.SetAttribute();
        _lineCharCtrl.yAxisAtb0.unitName = "负荷情况";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _values.Clear();
            for (int i = 0; i < 20; i++)
            {
                _values.Add(new Vector2(1000 + i * 10, Random.Range(0, 100)));
            }

            _lineCharCtrl.dataValue = new()
            {
                new LineCharValueData()
                {
                    lineName = "",
                    values = _values
                }
            };

            _lineCharCtrl.xAxisAtb.isAutomatic = false;
            _lineCharCtrl.xAxisAtb.axisTickCount = _values.Count;
            _lineCharCtrl.xAxisAtb.axisMin = _values[0].x;
            _lineCharCtrl.xAxisAtb.axisMax = _values[^1].x;
            _lineCharCtrl.SetAttribute();
            _lineCharCtrl.Refresh();
        }
    }
}