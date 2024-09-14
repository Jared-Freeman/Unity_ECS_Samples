using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Jurd.Project4
{
    [BurstCompile]
    [UpdateBefore(typeof(KillSystem))]
    public partial struct GlobalDamageOverTimeSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ActorVitals>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            var damageOverTimeJob = new DamageOverTimeJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
                DeltaTime = SystemAPI.Time.DeltaTime,
                DamagePerSecond = 1,
            };

            damageOverTimeJob.Schedule();
        }
    }

    public partial struct DamageOverTimeJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        public float DeltaTime;
        public float DamagePerSecond;

        public void Execute(Entity entity, ref ActorVitals actorVitals) 
        {
            actorVitals.Health -= DamagePerSecond * DeltaTime;

            if(actorVitals.Health < 0)
            {
                ECB.AddComponent<Kill>(entity);
            }
        }
    }
}
