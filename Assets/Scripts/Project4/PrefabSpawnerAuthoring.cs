using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace Jurd.Project4
{
    [BurstCompile]
    public struct PrefabSpawner : IComponentData
    {
        public Entity Prefab;
        public float Radius;
        /// <summary>
        /// Maximum number of spawns this spawner can produce
        /// </summary>
        public int MaxSpawns;
        /// <summary>
        /// Time between spawns
        /// </summary>
        public float SpawnInterval;
        /// <summary>
        /// Once this hits 0 we can spawn again
        /// </summary>
        public float CurrentSpawnCooldown;
    }

    public class PrefabSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public float Radius;
        public int MaxSpawns;
        public float SpawnInterval;

        class Baker : Baker<PrefabSpawnerAuthoring>
        {
            public override void Bake(PrefabSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PrefabSpawner
                {
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic), // note GetEntity used with a GameObject prefab
                    Radius = authoring.Radius,
                    MaxSpawns = authoring.MaxSpawns,
                    SpawnInterval = authoring.SpawnInterval,
                });
            }
        }
    }
}
