using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace Jurd.Project2
{
    [BurstCompile]
    public struct MovementTarget : IComponentData
    {

    }

    public class MovementTargetAuthoring : MonoBehaviour
    {
        class Baker : Baker<MovementTargetAuthoring>
        {
            public override void Bake(MovementTargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new MovementTarget
                {
                });
            }
        }
    }
}
