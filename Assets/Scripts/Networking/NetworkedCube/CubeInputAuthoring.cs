using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.NetCode;

namespace Jurd.Networking.NetworkedCube
{
    [BurstCompile]
    public struct CubeInput : IInputComponentData
    {
        public int Horizontal;
        public int Vertical;
    }

    [DisallowMultipleComponent]
    public class CubeInputAuthoring : MonoBehaviour
    {
        class Baking : Baker<CubeInputAuthoring>
        {
            public override void Bake(CubeInputAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<CubeInput>(entity);
            }
        }
    }

    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial struct SampleCubeInput : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            // Yeah Unity also hasn't implemented ECS support for the new input system...
            bool left = UnityEngine.Input.GetKey("left");
            bool right = UnityEngine.Input.GetKey("right");
            bool down = UnityEngine.Input.GetKey("down");
            bool up = UnityEngine.Input.GetKey("up");

            foreach (var playerInput in SystemAPI.Query<RefRW<CubeInput>>().WithAll<GhostOwnerIsLocal>())
            {
                playerInput.ValueRW = default;
                if (left)
                {
                    playerInput.ValueRW.Horizontal -= 1;
                }
                if (right)
                {
                    playerInput.ValueRW.Horizontal += 1;
                }
                if (down)
                {
                    playerInput.ValueRW.Vertical -= 1;
                }
                if (up)
                {
                    playerInput.ValueRW.Vertical += 1;
                }
            }
        }
    }
}