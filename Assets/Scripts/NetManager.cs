using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Prospect;
using SW;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NetManager : SingletonOfMono<NetManager>
{
    private OscInManager oscInManager;
    public LineCharCtrl _rightlineChart;
    private List<PosData> _rightdataList = new List<PosData>();
    public LineCharCtrl _lineChart;
    private List<PosData> dataList = new List<PosData>();
    public LineCharCtrl _leftlineChart;
    private List<PosData> _leftdataList = new List<PosData>();
    public LineCharCtrl _lineChartSpeed;
    private List<SpeedData> speedList = new List<SpeedData>();
    public LineCharCtrl webHeadLineChart;
    public LineCharCtrl unityHeadLineChart;


    public Toggle leftInTrackingToggle;
    public Toggle rightInTrackingToggle;
    public TMP_Text ipText;


    private void Awake()
    {
        ipText.text = "IP: " + GetLocalIPv4();
        
        
        
        
    }


    void Start()
    {
        oscInManager = gameObject.GetComponent<OscInManager>();
        oscInManager.InitMng();

        _rightlineChart.Init();
        _rightlineChart.SetAttribute();

        _lineChart.Init();
        _lineChart.SetAttribute();

        _leftlineChart.Init();
        _leftlineChart.SetAttribute();

        _lineChartSpeed.Init();
        _lineChartSpeed.SetAttribute();

        webHeadLineChart.Init();
        webHeadLineChart.SetAttribute();
        ShowWebHeadLines();
        unityHeadLineChart.Init();
        unityHeadLineChart.SetAttribute();
        ShowUnityHeadLines();
    }


    public void ShowPowerRightLines(Vector7 data)
    {
        var aaa = new PosData()
        {
            time = TimestampConversion.GetNowTimeStamp(true),
            value = data,
        };

        _rightdataList.Add(aaa);


        //移除超出时间范围的数据
        while (aaa.time - _rightdataList[0].time > 2900)
        {
            _rightdataList.RemoveAt(0);
        }


        _rightlineChart.dataValue.Clear();

        _rightlineChart.dataValue.Add(new LineCharValueData()
        {
            values = _rightdataList.Select(value => new Vector2(value.time - _rightdataList[0].time, value.value.x)).ToList()
        });

        _rightlineChart.dataValue.Add(new LineCharValueData()
        {
            values = _rightdataList.Select(value => new Vector2(value.time - _rightdataList[0].time, value.value.y)).ToList()
        });
        _rightlineChart.dataValue.Add(new LineCharValueData()
        {
            values = _rightdataList.Select(value => new Vector2(value.time - _rightdataList[0].time, value.value.z)).ToList()
        });
        // _rightlineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = _rightdataList.Select(value => new Vector2(value.time - _rightdataList[0].time, value.value.qx)).ToList()
        // });
        // _rightlineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = _rightdataList.Select(value => new Vector2(value.time - _rightdataList[0].time, value.value.qy)).ToList()
        // });
        // _rightlineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = _rightdataList.Select(value => new Vector2(value.time - _rightdataList[0].time, value.value.z)).ToList()
        // });
        // _rightlineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = _rightdataList.Select(value => new Vector2(value.time - _rightdataList[0].time, value.value.qw)).ToList()
        // });


        _rightlineChart.Refresh();
    }


    public void ShowPowerLines(Vector7 data)
    {
        // _posData.time = TimestampConversion.GetNowTimeStamp(true);
        // _posData.value = value;

        var aaa = new PosData()
        {
            time = TimestampConversion.GetNowTimeStamp(true),
            value = data,
        };

        dataList.Add(aaa);


        //移除超出时间范围的数据
        while (aaa.time - dataList[0].time > 2900)
        {
            dataList.RemoveAt(0);
        }


        _lineChart.dataValue.Clear();

        _lineChart.dataValue.Add(new LineCharValueData()
        {
            values = dataList.Select(value => new Vector2(value.time - dataList[0].time, value.value.x)).ToList()
        });

        _lineChart.dataValue.Add(new LineCharValueData()
        {
            values = dataList.Select(value => new Vector2(value.time - dataList[0].time, value.value.y)).ToList()
        });
        _lineChart.dataValue.Add(new LineCharValueData()
        {
            values = dataList.Select(value => new Vector2(value.time - dataList[0].time, value.value.z)).ToList()
        });
        // _lineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = dataList.Select(value => new Vector2(value.time - dataList[0].time, value.value.qx)).ToList()
        // });
        // _lineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = dataList.Select(value => new Vector2(value.time - dataList[0].time, value.value.qy)).ToList()
        // });
        // _lineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = dataList.Select(value => new Vector2(value.time - dataList[0].time, value.value.qz)).ToList()
        // });
        // _lineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = dataList.Select(value => new Vector2(value.time - dataList[0].time, value.value.qw)).ToList()
        // });


        _lineChart.Refresh();
    }

    public void ShowPowerLeftLines(Vector7 data)
    {
        var aaa = new PosData()
        {
            time = TimestampConversion.GetNowTimeStamp(true),
            value = data,
        };

        _leftdataList.Add(aaa);


        //移除超出时间范围的数据
        while (aaa.time - _leftdataList[0].time > 2900)
        {
            _leftdataList.RemoveAt(0);
        }


        _leftlineChart.dataValue.Clear();

        _leftlineChart.dataValue.Add(new LineCharValueData()
        {
            values = _leftdataList.Select(value => new Vector2(value.time - _leftdataList[0].time, value.value.x)).ToList()
        });

        _leftlineChart.dataValue.Add(new LineCharValueData()
        {
            values = _leftdataList.Select(value => new Vector2(value.time - _leftdataList[0].time, value.value.y)).ToList()
        });
        _leftlineChart.dataValue.Add(new LineCharValueData()
        {
            values = _leftdataList.Select(value => new Vector2(value.time - _leftdataList[0].time, value.value.z)).ToList()
        });
        // _leftlineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = _leftdataList.Select(value => new Vector2(value.time - _leftdataList[0].time, value.value.qx)).ToList()
        // });
        // _leftlineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = _leftdataList.Select(value => new Vector2(value.time - _leftdataList[0].time, value.value.qy)).ToList()
        // });
        // _leftlineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = _leftdataList.Select(value => new Vector2(value.time - _leftdataList[0].time, value.value.z)).ToList()
        // });
        // _leftlineChart.dataValue.Add(new LineCharValueData()
        // {
        //     values = _leftdataList.Select(value => new Vector2(value.time - _leftdataList[0].time, value.value.qw)).ToList()
        // });


        _leftlineChart.Refresh();
    }

    public void ShowSpeedLines(float data)
    {
        var aaa = new SpeedData()
        {
            time = TimestampConversion.GetNowTimeStamp(true),
            value = data,
        };

        speedList.Add(aaa);


        //移除超出时间范围的数据
        while (aaa.time - speedList[0].time > 2900)
        {
            speedList.RemoveAt(0);
        }


        _lineChartSpeed.dataValue.Clear();

        _lineChartSpeed.dataValue.Add(new LineCharValueData()
        {
            values = speedList.Select(value => new Vector2(value.time - speedList[0].time, value.value)).ToList()
        });


        _lineChartSpeed.Refresh();
    }

    public void ShowWebHeadLines()
    {
        var jsonData = SerializeTool.DeSerializeFileToObjectJson<List<Vector7>>(Application.streamingAssetsPath + "/viewer_pose_dataZ.json");

        webHeadLineChart.dataValue.Clear();
        webHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.x)).ToList()
        });
        webHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.y)).ToList()
        });
        webHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.z)).ToList()
        });

        webHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.qx)).ToList()
        });
        webHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.qy)).ToList()
        });
        webHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.qz)).ToList()
        });
        webHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.qw)).ToList()
        });

        webHeadLineChart.Refresh();
    }

    public void ShowUnityHeadLines()
    {
        var jsonData = SerializeTool.DeSerializeFileToObjectJson<List<Vector7>>(Application.streamingAssetsPath + "/asdadZ.json");

        unityHeadLineChart.dataValue.Clear();
        unityHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.x)).ToList()
        });
        unityHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.y)).ToList()
        });
        unityHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.z)).ToList()
        });

        unityHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.qx)).ToList()
        });
        unityHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.qy)).ToList()
        });
        unityHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.qz)).ToList()
        });
        unityHeadLineChart.dataValue.Add(new LineCharValueData()
        {
            values = jsonData.Select((t, i) => new Vector2(i * 2, t.qw)).ToList()
        });

        unityHeadLineChart.Refresh();
    }


    public string GetLocalIPv4()
    {
        // 获取本地主机名
        string hostName = Dns.GetHostName();
        // 根据主机名获取所有关联的 IP 地址
        IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

        // 遍历地址列表，找到第一个 IPv4 地址并返回
        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork) // InterNetworkV6 是 IPv6
            {
                return ip.ToString();
            }
        }

        // 如果循环结束都没找到，返回错误信息
        return "未找到 IPv4 地址，请检查网络连接";
    }
}

public class PosData
{
    public long time;
    public Vector7 value;
}

public class SpeedData
{
    public long time;
    public float value;
}


public class Ssadgfas
{
    public List<Vector7> leftGamePade;
    public List<Vector7> rightGamePade;
    public List<Vector7> head;
}