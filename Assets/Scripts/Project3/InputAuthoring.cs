using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using System;

namespace Jurd.Project3
{
    // For now we use this as a singleton
    [BurstCompile]
    public struct Input : IComponentData
    {
        public float2 InputAxisMove;
    }

    // No need to write one of these
    //public class InputAuthoring : MonoBehaviour
    //{
    //    class Baker : Baker<InputAuthoring>
    //    {
    //        public override void Bake(InputAuthoring authoring)
    //        {
    //            var entity = GetEntity(TransformUsageFlags.Dynamic);
    //        }
    //    }
    //}
}
