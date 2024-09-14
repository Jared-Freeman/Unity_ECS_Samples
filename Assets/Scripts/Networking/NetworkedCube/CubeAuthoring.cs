using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace Jurd.Networking.NetworkedCube
{
    [BurstCompile]
    public struct Cube : IComponentData
    {
        
    }

    [DisallowMultipleComponent]
    public class CubeAuthoring : MonoBehaviour
    {
        class Baker : Baker<CubeAuthoring>
        {
            public override void Bake(CubeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Cube>(entity);
            }
        }
    }
}
