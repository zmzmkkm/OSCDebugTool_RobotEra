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
    private OscInManager _oscInManager;


    public LineCharCtrl _rightlineChart;
    private List<PosData> _rightdataList = new List<PosData>();

    public LineCharCtrl _leftlineChart;
    private List<PosData> _leftdataList = new List<PosData>();

    private Transform _canvas;

    [HideInInspector] public Toggle leftInTrackingToggle;
    [HideInInspector] public Toggle rightInTrackingToggle;
    private TMP_Text _ipText;

    private RectTransform _headLinePanel;
    private Vector3 _headLinePanelPos;

    #region 头部数据

    private RectTransform _headLineChartRect;
    private LineCharCtrl _headLineChart;
    private readonly List<PosData> _headDataList = new List<PosData>();
    private Transform _headLegend;
    private GameObject _headEg;
    private readonly string[] _legendNames = new[] { "Pos_X", "Pos_Y", "Pos_Z", "Rot_X", "Rot_Y", "Rot_Z", "Rot_W" };

    private readonly Color[] _legendColors = new[]
    {
        new Color(1, 0, 0, 1),
        new Color(0, 0.7f, 0, 1),
        new Color(0, 0, 1, 1),
        new Color(1, 0, 0.5f, 1),
        new Color(0.44f, 1, 0, 1),
        new Color(0, 1, 1, 1),
        new Color(1, 1, 1, 1)
    };

    private readonly List<Toggle> _headLegendToggles = new List<Toggle>();

    private Toggle _sizeToggle;

    #endregion


    private void Awake()
    {
        _oscInManager = gameObject.GetComponent<OscInManager>();
        _oscInManager.InitMng();

        _canvas = GameObject.Find("Canvas").transform;

        leftInTrackingToggle = _canvas.Find("Left_Toggle").GetComponent<Toggle>();
        rightInTrackingToggle = _canvas.Find("Right_Toggle").GetComponent<Toggle>();
        _ipText = _canvas.Find("IP_Text").GetComponent<TMP_Text>();

        _headLinePanel = _canvas.Find("LineDataPanel/HeadLinePanel").GetComponent<RectTransform>();
        _headLinePanelPos = _headLinePanel.localPosition;
        _headLineChartRect = _headLinePanel.Find("LineChart").GetComponent<RectTransform>();
        _headLineChart = _headLineChartRect.GetComponent<LineCharCtrl>();
        _headLegend = _headLinePanel.Find("Legend");
        _headEg = _headLegend.Find("Eg").gameObject;
        _headEg.SetActive(false);
        _sizeToggle = _headLinePanel.Find("Size_Toggle").GetComponent<Toggle>();
    }


    // 起始时间戳（例如：Time.time 或 DateTime 转换的秒数）
    private float startTimestamp = 0f;


    void Start()
    {
        _ipText.text = "IP: " + GetLocalIPv4();

        _rightlineChart.Init();
        _rightlineChart.SetAttribute();

        _headLineChart.Init();
        _headLineChart.SetAttribute();
        for (int i = 0; i < 7; i++)
        {
            var go = GameObject.Instantiate(_headEg, _headLegend);
            go.SetActive(true);
            go.GetComponentInChildren<TMP_Text>().text = _legendNames[i];
            go.transform.Find("Ioc_Image").GetComponent<Image>().color = _legendColors[i];
            var toggle = go.GetComponentInChildren<Toggle>();
            toggle.isOn = i < 3;
            _headLegendToggles.Add(toggle);
            toggle.onValueChanged.AddListener(isOn => { _headDataList.Clear(); });
        }

        _sizeToggle.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                _headLinePanel.sizeDelta = new Vector2(2490.971f, 1284.816f);
                _headLinePanel.localPosition = new Vector3(0, -48.335f, 0);

                _headLineChartRect.sizeDelta = new Vector2(2337.558f, 1089.625f);
                _headLineChartRect.localPosition = new Vector2(34.17361f, -17.95428f);

                _headLineChartRect.Find("Chart/DrawLinesPanel").GetComponent<RectTransform>().sizeDelta = new Vector2(2337.5f, 1089.6f);
            }
            else
            {
                _headLinePanel.sizeDelta = new Vector2(1010.674f, 571.2064f);
                _headLinePanel.localPosition = _headLinePanelPos;

                _headLineChartRect.sizeDelta = new Vector2(841.5327f, 338.6641f);
                _headLineChartRect.localPosition = new Vector2(27.29303f, -2.227295f);

                _headLineChartRect.Find("Chart/DrawLinesPanel").GetComponent<RectTransform>().sizeDelta = new Vector2(841.55f, 338.66f);
            }

            _leftlineChart.Init();
            _leftlineChart.SetAttribute();
            _headLineChart.Refresh();
        });

        _leftlineChart.Init();
        _leftlineChart.SetAttribute();
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
        var aaa = new PosData()
        {
            time = TimestampConversion.GetNowTimeStamp(true),
            value = data,
        };

        _headDataList.Add(aaa);


        //移除超出时间范围的数据
        while (aaa.time - _headDataList[0].time > 2900)
        {
            _headDataList.RemoveAt(0);
        }


        _headLineChart.dataValue.Clear();

        for (var i = 0; i < 7; i++)
        {
            var idx = i;
            if (_headLegendToggles[i].isOn)
            {
                _headLineChart.dataValue.Add(new LineCharValueData()
                {
                    values = _headDataList.Select(value => new Vector2(
                        value.time - _headDataList[0].time,
                        idx switch
                        {
                            0 => value.value.x,
                            1 => value.value.y,
                            2 => value.value.z,
                            3 => value.value.qx,
                            4 => value.value.qy,
                            5 => value.value.qz,
                            6 => value.value.qw,
                        }
                    )).ToList()
                });
            }
            else
            {
                _headLineChart.dataValue.Add(new LineCharValueData()
                {
                    values = new List<Vector2>() { Vector2.zero, Vector2.zero },
                });
            }
        }


        _headLineChart.Refresh();
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


// public string GetLocalIPv4()
// {
//     // 获取本地主机名
//     string hostName = Dns.GetHostName();
//     // 根据主机名获取所有关联的 IP 地址
//     IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
//
//     // 遍历地址列表，找到第一个 IPv4 地址并返回
//     foreach (IPAddress ip in hostEntry.AddressList)
//     {
//         if (ip.AddressFamily == AddressFamily.InterNetwork) // InterNetworkV6 是 IPv6
//         {
//             return ip.ToString();
//         }
//     }
//
//     // 如果循环结束都没找到，返回错误信息
//     return "未找到 IPv4 地址，请检查网络连接";
// }

    public static string GetLocalIPv4()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        var ip = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        return ip?.ToString() ?? "未检测到 IPv4 地址";
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