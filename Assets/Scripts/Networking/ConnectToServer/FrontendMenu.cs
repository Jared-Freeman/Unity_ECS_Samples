using Unity.NetCode;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Jurd.Networking.ConnectToServer
{
    public class FrontendMenu : MonoBehaviour
    {
        [Header("Client")]
        public TMP_InputField InputField_ClientIp;
        public TMP_InputField InputField_ClientPort;
        public Button Button_Connect;

        [Header("Client/Server")]
        public TMP_InputField InputField_HostPort;
        public Button Button_Host;

        protected void OnEnable()
        {
            // client
            Button_Connect.onClick.AddListener(OnConnectButtonPressed);
            InputField_ClientIp.onSubmit.AddListener(OnClientIpFieldSubmit);
            InputField_ClientPort.onSubmit.AddListener(OnClientPortFieldSubmit);

            // client/host
            Button_Host.onClick.AddListener(OnHostButtonPressed);
            InputField_HostPort.onSubmit.AddListener(OnHostPortFieldSubmit);
        }

        protected void OnDisable()
        {
            // client
            Button_Connect.onClick.RemoveListener(OnConnectButtonPressed);
            InputField_ClientIp.onSubmit.RemoveListener(OnClientIpFieldSubmit);
            InputField_ClientPort.onSubmit.RemoveListener(OnClientPortFieldSubmit);

            // client/host
            Button_Host.onClick.RemoveListener(OnHostButtonPressed);
            InputField_HostPort.onSubmit.RemoveListener(OnHostPortFieldSubmit);
        }

        protected void OnClientIpFieldSubmit(string text)
        {
            TryAssignIpField(text);
        }

        protected void OnClientPortFieldSubmit(string text)
        {
            TryAssignPortField(text);
        }

        protected void OnHostPortFieldSubmit(string text)
        {
            TryAssignPortField(text);
        }

        protected bool TryAssignIpField(string text)
        {
            if (!ServerConnectionUtils.ValidateIPv4(text))
            {
                // can do a warning or something here 
                PlayerInfoController.Instance.Ip = "";
                return false;
            }

            PlayerInfoController.Instance.Ip = InputField_ClientIp.text;
            return true;
        }

        protected bool TryAssignPortField(string text)
        {
            if (!ushort.TryParse(text, out ushort port))
            {
                // can do a warning or something here 
                PlayerInfoController.Instance.Port = "";
                return false;
            }

            PlayerInfoController.Instance.Port = text;
            return true;
        }

        protected void OnHostButtonPressed()
        {
            TryAssignPortField(InputField_HostPort.text);

            Debug.Log($"Starting server on port {PlayerInfoController.Instance.Port}");

            ServerConnectionUtils.StartClientServer(PlayerInfoController.Instance.Port);
        }

        protected void OnConnectButtonPressed()
        {
            TryAssignIpField(InputField_ClientIp.text);
            TryAssignPortField(InputField_ClientPort.text);

            if (!ServerConnectionUtils.ValidateIPv4(InputField_ClientIp.text))
            {
                Debug.Log($"Ip not valid");
                return;
            }

            Debug.Log($"Connecting to server IP: {PlayerInfoController.Instance.Ip}, port: {PlayerInfoController.Instance.Port}");

            ServerConnectionUtils.ConnectToServer(PlayerInfoController.Instance.Ip, PlayerInfoController.Instance.Port);

            // Connect to Server of Create Client & Server
            //if (ClientServerBootstrap.RequestedPlayType == ClientServerBootstrap.PlayType.ClientAndServer)
            //{
            //    ServerConnectionUtils.StartClientServer(PlayerInfoController.Instance.Port);
            //}
            //else if (ClientServerBootstrap.RequestedPlayType == ClientServerBootstrap.PlayType.Client)
            //{
            //    ServerConnectionUtils.ConnectToServer(PlayerInfoController.Instance.Ip,
            //        PlayerInfoController.Instance.Port);
            //}
        }
    }
}
