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
                    NetManager.Instance.ShowPowerLines(SerializeTool.DeSerialize<Vector7>(value as byte[]));
                    break;
                case "gamePadPosLeft":
                    NetManager.Instance.ShowPowerLeftLines(SerializeTool.DeSerialize<Vector7>(value as byte[]));
                    break;

                case "handPosRight":
                    NetManager.Instance.ShowPowerRightLines(SerializeTool.DeSerialize<List<Vector7>>(value as byte[])[0]);
                    break;
                case "handPosLeft":
                    NetManager.Instance.ShowPowerLeftLines(SerializeTool.DeSerialize<List<Vector7>>(value as byte[])[0]);
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
                case "rightInTracking":
                    NetManager.Instance.rightInTrackingToggle.isOn = (bool)value;
                    break;
                case "leftInTracking":
                    NetManager.Instance.leftInTrackingToggle.isOn = (bool)value;
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