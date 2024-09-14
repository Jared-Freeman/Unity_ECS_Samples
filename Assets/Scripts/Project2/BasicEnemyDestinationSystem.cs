using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jurd.Project2
{
    /// <summary>
    /// Computes a destination for <see cref="Jurd.Project2.BasicEnemy.Destination"/>
    /// </summary>
    /// <remarks>
    /// This will eventually be a target chooser for an ai state machine.
    /// </remarks>
    [BurstCompile]
    [UpdateBefore(typeof(BasicEnemyMovementSystem))] // NOTE THIS ATTRIBUTE! https://docs.unity3d.com/Packages/com.unity.entities@1.3/manual/systems-update-order.html
    public partial struct BasicEnemyDestinationSystem : ISystem
    {
        // NOTE: it's problematic over time to store variables in a system.
        // Instead we would want to store fields such as these in components, too.
        // We can use singleton pattern for components:
        /// <see cref="SystemAPI.GetSingleton{T}"/>
 
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BasicEnemy>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // target is closest MovementTarget tagged entity
            foreach (var (basicEnemy, transform, localToWorld) in SystemAPI.Query<RefRW<BasicEnemy>, RefRO<LocalTransform>, RefRO<LocalToWorld>>())
            {
                float3 destination = new float3(math.INFINITY);

                // (warning double foreach blah blah)
                // move this over to a spatial hash map later
                foreach(var (target, targetLocalToWorld) in SystemAPI.Query<RefRO<MovementTarget>, RefRO<LocalToWorld>>())
                {
                    if(math.lengthsq(localToWorld.ValueRO.Position - targetLocalToWorld.ValueRO.Position) < math.lengthsq(localToWorld.ValueRO.Position - destination))
                    {
                        destination = targetLocalToWorld.ValueRO.Position;
                    }
                }

                // destination is self if no target is found
                bool3 isInf = destination == new float3(math.INFINITY);
                if (!isInf.x || !isInf.y || !isInf.z)
                {
                    basicEnemy.ValueRW.Destination = destination;
                }
                else
                {
                    basicEnemy.ValueRW.Destination = localToWorld.ValueRO.Position;
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    }
}
