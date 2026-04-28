// ========================================================
// 描 述：OSCInput  向外发送消息(大屏端)
// 作 者：SW
// 创建时间：2023/09/18 13:37:47
// 版 本：v 1.0
// ========================================================

using System.Collections.Generic;
using ProtoBuf;
using SW;
using UniOSC;
using Unity.VisualScripting;
using UnityEngine;


namespace Prospect
{
    public class OscInManager : UniOSCEventTarget
    {
        public void InitMng()
        {
            var oscOutManager = ComponentHolderProtocol.GetOrAddComponent<UniOSCConnection>(gameObject);
            receiveAllAddresses = true;
            _useExplicitConnection = true;
            explicitConnection = oscOutManager;

            explicitConnection.oscInIPAddress = "127.0.0.1";
            explicitConnection.oscPort = 8000;
            explicitConnection.ConnectOSC();

            print("Init OSCInManager...");
        }

        /// <summary>
        /// 消息接收后，处理分发
        /// </summary>
        /// <param name="args"></param>
        public override void OnOSCMessageReceived(UniOSCEventArgs args)
        {
            if (args.Packet.Data.Count <= 0) return;

            foreach (object item in args.Packet.Data)
            {
                // Debug.Log("获取到的数据: \naddress:" + args.Address + "\nvalue:" + item);
                

                DistributeMessage(args.Address.TrimStart('/'), item);
            }
        }


        private List<Vector7> aaa = new List<Vector7>();
        private bool vvv = true;

        /// <summary>
        /// 消息处理分发
        /// </summary>
        private void DistributeMessage(string address, object value)
        {
            switch (address)
            {
                case "gamePadPosRight":
                    NetManager.Instance.ShowPowerRightLines(SerializeTool.DeSerialize<Vector7>(value as byte[]));
                    break;
                case "headPos":
                    NetManager.Instance.ShowPowerLines(SerializeTool.DeSerialize<Vector7>(value  as byte[]));
                    // NetManager.Instance.ShowPowerLines(SerializeTool.DeSerializeJson<Vector7>(value  as string));
                    // if (vvv)
                    // {
                    //     aaa.Add(SerializeTool.DeSerializeJson<Vector7>(value));
                    //     if (aaa.Count > 1300)
                    //     {
                    //         SerializeTool.SerializeToFileJson(Application.streamingAssetsPath + "/OldX.json", aaa);
                    //         vvv = false;
                    //     }
                    // }
                
                
                    break;
                case "gamePadPosLeft":
                    NetManager.Instance.ShowPowerLeftLines(SerializeTool.DeSerialize<Vector7>(value  as byte[]));
                    break;
            
                case "Log":
                    var logMsg = SerializeTool.DeSerializeJson<LogMsg>(value as string);
                    switch (logMsg.type)
                    {
                        case LogType.Assert or LogType.Log:
                            Debug.Log(logMsg.log + "\n\n" + logMsg.stackTrace + "\n\n");
                            break;
                        case LogType.Warning:
                            Debug.LogWarning(logMsg.log + "\n\n" + logMsg.stackTrace + "\n\n");
                            break;
                        case LogType.Error or LogType.Exception:
                            Debug.LogError(logMsg.log + "\n\n" + logMsg.stackTrace + "\n\n");
                            break;
                    }
            
                    break;
            
                // case "speed":
                //     var parts = value.Replace("(", "").Replace(")", "").Split(',');
                //     NetManager.Instance.ShowSpeedLines(new Vector2(float.Parse(parts[0]), float.Parse(parts[1])));
                //     break;
            
                case "CHAZHI":
                    NetManager.Instance.ShowSpeedLines((float)value);
                    break;
                case "rightInTracking":
                    NetManager.Instance.rightInTrackingToggle.isOn = (bool)value;
                    break;
                case "leftInTracking":
                    NetManager.Instance.leftInTrackingToggle.isOn =  (bool)value;
                    break;
            }
        }
    }
}

/// <summary>
/// 手柄位姿信息
/// </summary>
[ProtoContract]
public class Vector7
{
    [ProtoMember(1)] public float x;
    [ProtoMember(2)] public float y;
    [ProtoMember(3)] public float z;
    [ProtoMember(4)] public float qx;
    [ProtoMember(5)] public float qy;
    [ProtoMember(6)] public float qz;
    [ProtoMember(7)] public float qw;
}

public class LogMsg
{
    public LogType type;
    public string log;
    public string stackTrace;
}