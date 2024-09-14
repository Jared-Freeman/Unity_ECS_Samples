using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Jurd.Project3
{
    [BurstCompile]
    public partial struct PlayerMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerMovement>();

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var input = SystemAPI.GetSingleton<Input>();
            float3 moveDirection = new(0, 0, 0);
            if (math.lengthsq(input.InputAxisMove) > 0)
            {
                float2 moveAxisNormalized = math.normalize(input.InputAxisMove);
                moveDirection = new(moveAxisNormalized.x, 0, moveAxisNormalized.y);
            }

            foreach(var (playerMovement, transform) in SystemAPI.Query<RefRO<PlayerMovement>, RefRW<LocalTransform>>())
            {
                float3 displacement = moveDirection * SystemAPI.Time.DeltaTime * playerMovement.ValueRO.Speed;
                transform.ValueRW.Position = transform.ValueRW.Position + displacement;
            }
        }
    }
}
