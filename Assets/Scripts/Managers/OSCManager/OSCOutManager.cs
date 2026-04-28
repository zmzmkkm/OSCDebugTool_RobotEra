// ========================================================
// 描 述：OSCOutput 向外发送消息(控制端)
// 作 者：SW
// 创建时间：2023/08/23 16:59:16
// 版 本：v 1.0
// ========================================================

using OSCsharp.Data;
using UniOSC;
using Unity.VisualScripting;
using UnityEngine;

namespace Prospect
{
    public class OscOutManager : MonoBehaviour
    {
        private UniOSCConnection _uniOscConnection;
        private OscMessage _oscMessage;

        public void InitMng()
        {
            _uniOscConnection = this.GetOrAddComponent<UniOSCConnection>();
            _oscMessage = new OscMessage("/");
            Debug.Log("Init OSCOutManager...");
        }


        public bool StartServer(string ip, int port)
        {
            _uniOscConnection.oscOutIPAddress = ip;
            _uniOscConnection.oscOutPort = port;
            _uniOscConnection.ConnectOSCOut();

            // Debug.Log($"输入的IP: {_uniOscConnection.oscInIPAddress}");
            // Debug.Log($"输出的ip: {_uniOscConnection.oscOutIPAddress}");
            // Debug.Log($"输入端口: {_uniOscConnection.oscPort}  输出端口: {_uniOscConnection.oscOutPort}");
            // Debug.Log($"输出创建状态: 输入端：{_uniOscConnection.isConnected}  输出端： {_uniOscConnection.isConnectedOut}");

            return _uniOscConnection.isConnectedOut;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMsg(string type, object value)
        {
            // Debug.Log("输出要发送的消息: {" + type+":"+value+"}");
            _oscMessage.Address = $"/{type}";
            _oscMessage.ClearData();
            _oscMessage.Append(value ?? "");
            var uniOscEvent = new UniOSCEventArgs(_uniOscConnection.oscOutPort, _oscMessage);
            uniOscEvent.IPAddress = _uniOscConnection.oscOutIPAddress;
            _uniOscConnection.SendOSCMessage(null, uniOscEvent);
        }
    }
}