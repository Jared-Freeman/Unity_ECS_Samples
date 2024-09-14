using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Jurd.Project1
{
    [BurstCompile]
    public struct BasicEnemy : IComponentData
    {
        public float3 Destination;
        public float Speed;
    }
}