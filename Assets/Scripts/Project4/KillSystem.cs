using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Jurd.Project4
{
    [BurstCompile]
    public partial struct KillSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Kill>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            var killJob = new KillJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
            };

            // note we don't query. The job Execute() method generates the query by magic...
            killJob.Schedule();
        }
    }

    [BurstCompile]
    public partial struct KillJob : IJobEntity
    {
        public EntityCommandBuffer ECB;

        // Source generation with IJobEntity will execute on entities matching the query specified by fn arguments
        // hence this method will execute on entities with Kill tag
        // We use "in" to specify readonly
        // Alternatively, "ref" can be used for read/write
        void Execute(Entity entity, in Kill kill)
        {
            // can place further logic here later...

            ECB.DestroyEntity(entity); // entity command buffer records the destroy command and replays it later
        }
    }
}
