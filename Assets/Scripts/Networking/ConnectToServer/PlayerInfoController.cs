using Jurd.Utilities;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Jurd.Networking.ConnectToServer
{
    public class PlayerInfoController : Singleton<PlayerInfoController>
    {
        [Header("Player Name")]
        public GameObject PlayerNamePrefab;
        public string LocalPlayerName;
        protected readonly Dictionary<Entity, GameObject> _nameTags = new();

        [Header("Connection Settings")]
        public string Ip;
        public string Port;
    }
}
