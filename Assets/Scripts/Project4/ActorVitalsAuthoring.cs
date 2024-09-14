using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace Jurd.Project4
{
    [BurstCompile]
    public struct ActorVitals : IComponentData
    {
        public float Health;
    }

    public class ActorVitalsAuthoring : MonoBehaviour
    {
        public float Health;

        class Baker : Baker<ActorVitalsAuthoring>
        {
            public override void Bake(ActorVitalsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new ActorVitals
                {
                    Health = authoring.Health,
                });
            }
        }
    }
}
