using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jurd.Project4
{
    [BurstCompile]
    public partial struct PrefabSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PrefabSpawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float dt = SystemAPI.Time.DeltaTime;

            foreach(var (prefabSpawner,localTransform, localToWorld) 
                in SystemAPI.Query<RefRW<PrefabSpawner>, RefRO<LocalTransform>, RefRO <LocalToWorld>>())
            {
                while(prefabSpawner.ValueRO.CurrentSpawnCooldown <= 0)
                {
                    // random point in radius.
                    // Note: using UnityEngine.Random since uh there isn't a mathematics package one...
                    float3 spawnDirection = new(1, 0, 0);
                    float randMagnitude = UnityEngine.Random.Range(0, prefabSpawner.ValueRO.Radius);
                    float randRotationRadians = UnityEngine.Random.Range(0, math.PI2);
                    quaternion rotation = quaternion.RotateY(randRotationRadians);

                    spawnDirection = math.rotate(rotation, spawnDirection);
                    float3 spawnPosition = randMagnitude * spawnDirection;

                    Entity instance = state.EntityManager.Instantiate(prefabSpawner.ValueRO.Prefab);

                    // new function call dropped
                    state.EntityManager.SetComponentData(instance, new LocalTransform
                    {
                        Position = localToWorld.ValueRO.Position + spawnPosition,
                        Rotation = localToWorld.ValueRO.Rotation,
                        Scale = localTransform.ValueRO.Scale,
                    });

                    float overflow = prefabSpawner.ValueRW.CurrentSpawnCooldown; // (-inf, 0]
                    prefabSpawner.ValueRW.CurrentSpawnCooldown = prefabSpawner.ValueRO.SpawnInterval + overflow;
                }

                prefabSpawner.ValueRW.CurrentSpawnCooldown -= dt;
            }
        }
    }
}
