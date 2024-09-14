using System.Linq;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;
using System.Net;
using Unity.Entities;

// This code is mostly lifted from the UnityTechnologies ECS-Network-Racing sample

namespace Jurd.Networking.ConnectToServer
{
    public class ServerConnectionUtils
    {
        protected const ushort NETWORK_PORT = 7979;
        protected static World _server;
        protected static World _client;
        public static bool IsTryingToConnect = false;

        /// <summary>
        ///     Start a Client and Server in your local IP
        /// </summary>
        public static void StartClientServer(string port)
        {
            IsTryingToConnect = true;
            if (ClientServerBootstrap.RequestedPlayType != ClientServerBootstrap.PlayType.ClientAndServer)
            {
                Debug.LogError($"Creating client/server worlds is not allowed if playmode is set to {ClientServerBootstrap.RequestedPlayType}");
                return;
            }

            if (_server != null && _server.IsCreated)
            {
                _server.Dispose();
            }

            if (_client != null && _client.IsCreated)
            {
                _client.Dispose();
            }

            _server = ClientServerBootstrap.CreateServerWorld("ServerWorld");
            _client = ClientServerBootstrap.CreateClientWorld("ClientWorld");

            //Destroy the local simulation world to avoid the game scene to be loaded into it
            //This prevent rendering (rendering from multiple world with presentation is not greatly supported)
            //and other issues.
            DestroyLocalSimulationWorld();

            World.DefaultGameObjectInjectionWorld ??= _server;

            SceneLoader.Instance.LoadScene(SceneType.Main);

            var networkEndpoint = NetworkEndpoint.AnyIpv4.WithPort(ParsePortOrDefault(port));
            {
                using var drvQuery = _server.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(networkEndpoint);
            }

            networkEndpoint = NetworkEndpoint.LoopbackIpv4.WithPort(ParsePortOrDefault(port));
            {
                using var drvQuery = _client.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(_client.EntityManager, networkEndpoint);
            }
        }

        /// <summary>
        ///     Connect to the server in the IP address and port
        /// </summary>
        /// <param name="ip">Server IP Address</param>
        /// <param name="port">Port</param>
        public static void ConnectToServer(string ip, string port)
        {
            IsTryingToConnect = true;
            var client = ClientServerBootstrap.CreateClientWorld("ClientWorld");

            DestroyLocalSimulationWorld();

            World.DefaultGameObjectInjectionWorld ??= client;

            SceneLoader.Instance.LoadScene(SceneType.Main);

            var networkEndpoint = NetworkEndpoint.Parse(ip, ParsePortOrDefault(port));
            {
                using var drvQuery = client.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(client.EntityManager, networkEndpoint);
            }
        }

        public static bool ValidateIPv4(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            var splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            return splitValues.All(r => byte.TryParse(r, out var tempForParsing)) && ValidateIP(ipString);
        }

        private static bool ValidateIP(string addrString)
        {
            return IPAddress.TryParse(addrString, out var address);
        }

        /// <summary>
        /// Tries to parse a port, returns true if successful, otherwise false
        /// The port will be set to whatever is parsed, otherwise the default port of k_NetworkPort
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static ushort ParsePortOrDefault(string s)
        {
            if (!ushort.TryParse(s, out var port))
            {
                Debug.LogWarning($"Unable to parse port, using default port {NETWORK_PORT}");
                return NETWORK_PORT;
            }

            return port;
        }

        private static void DestroyLocalSimulationWorld()
        {
            foreach (var world in World.All)
            {
                if (world.Flags == WorldFlags.Game)
                {
                    world.Dispose();
                    break;
                }
            }
        }
    }
}