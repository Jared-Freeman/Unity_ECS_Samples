using System;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine.SceneManagement;

namespace Jurd.Networking.Common
{
    // Create a custom bootstrap, which enables auto-connect.
    // The bootstrap can also be used to configure other settings as well as to
    // manually decide which worlds (client and server) to create based on user input
    // This one is for NetworkedCube
    //[UnityEngine.Scripting.Preserve]
    //public class GameBootstrap : ClientServerBootstrap // Note from Jurd: one per project
    //{
    //    public override bool Initialize(string defaultWorldName)
    //    {
    //        AutoConnectPort = 7979; // Enabled auto connect
    //        return base.Initialize(defaultWorldName); // Use the regular bootstrap
    //    }
    //}

    // For ConnectToServer project
    [UnityEngine.Scripting.Preserve]
    public class GameBootstrap : ClientServerBootstrap
    {
        public override bool Initialize(string defaultWorldName)
        {
#if UNITY_EDITOR
            // If we are in the editor, we check if the loaded scene is "MainMenu",
            // if we are in a player we assume it is in the frontend
            string sceneName = SceneManager.GetActiveScene().name;
            bool isFrontend = sceneName == "MainMenu";

            // If we have Server Only in Editor we skip to the MainScene
            if (RequestedPlayType == PlayType.Server && isFrontend)
            {
                AutoConnectPort = 7979;
                CreateServerWorld("Server");
                SceneManager.LoadScene("Main");
                return true;
            }

            if (isFrontend)
            {
                // Disable the auto-connect in the frontend. The reset is necessary in the Editor since we can start the demos directly and
                // (the AutoConnectPort will lose its default value)
                AutoConnectPort = 0;
                CreateLocalWorld(defaultWorldName);
            }
            else
            {
                // This will enable auto connect. We only enable auto connect if we are not going through frontend.
                // The frontend will parse and validate the address before connecting manually.
                // Using this auto connect feature will deal with the client only connect address from Multiplayer PlayMode Tools
                AutoConnectPort = 7979;
                // Create the default client and server worlds, depending on build type in a player or the Multiplayer PlayMode Tools in the editor
                CreateDefaultClientServerWorlds();
            }
#elif UNITY_SERVER
            AutoConnectPort = 7979;
            CreateServerWorld("Server");
#else
            CreateLocalWorld(defaultWorldName);
#endif
            return true;
        }
    }
}
