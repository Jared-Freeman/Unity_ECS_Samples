using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jurd.Project1
{
    [BurstCompile]
    public partial struct BasicEnemyMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BasicEnemy>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (basicEnemy, transform, localToWorld) in
                SystemAPI.Query<RefRO<BasicEnemy>, RefRW<LocalTransform>, RefRO<LocalToWorld>>())
            {
                float3 displacement = SystemAPI.Time.DeltaTime * math.normalize(basicEnemy.ValueRO.Destination - localToWorld.ValueRO.Position) * basicEnemy.ValueRO.Speed;
                transform.ValueRW.Position = transform.ValueRW.Position + displacement;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}