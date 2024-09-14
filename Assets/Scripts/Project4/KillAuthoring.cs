using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace Jurd.Project4
{
    // tag
    [BurstCompile]
    public struct Kill : IComponentData
    {
    
    }

    /// <summary>
    /// Why would you need this are you just gonna kill your entity on the first frame???
    /// </summary>
    public class KillAuthoring : MonoBehaviour
    {
        class Baker : Baker<KillAuthoring>
        {
            public override void Bake(KillAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Kill());
            }
        }
    }
}
