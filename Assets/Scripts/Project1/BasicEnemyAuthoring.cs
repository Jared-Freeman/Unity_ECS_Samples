using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using NUnit.Framework.Internal;

namespace Jurd.Project1
{
    public class BasicEnemyAuthoring : MonoBehaviour
    {
        public float3 Destination;
        public float Speed;

        class Baker : Baker<BasicEnemyAuthoring>
        {
            public override void Bake(BasicEnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new BasicEnemy
                {
                    // component data assignments go here
                    Destination = authoring.Destination,
                    Speed = authoring.Speed,
                });
            }
        }
    }
}