using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace Jurd.Project3
{
    [BurstCompile]
    public struct PlayerMovement : IComponentData
    {
        public float Speed;
    }

    public class PlayerMovementAuthoring : MonoBehaviour
    {
        public float Speed;
        
        class Baker : Baker<PlayerMovementAuthoring>
        {
            public override void Bake(PlayerMovementAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PlayerMovement
                {
                    Speed = authoring.Speed,
                });
            }
        }
    }
}
