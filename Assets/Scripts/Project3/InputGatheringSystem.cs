using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jurd.Project3
{
    /// <summary>
    /// System that accepts callbacks from generated C# input system class <see cref="InputActions"/>
    /// and maps them to input component(s)
    /// </summary>
    // TODO: consider methods to support multiple player inputs
    [BurstCompile]
    public partial class InputGatheringSystem : SystemBase, InputActions.IProject3Actions
    {
        protected InputActions _inputActions;
        protected EntityQuery _inputEntityQuery;
        protected float2 _inputAxisMove;

        protected override void OnStartRunning()
        {
            _inputActions.Enable();
        }

        protected override void OnStopRunning()
        {
            _inputActions.Disable();
        }

        [BurstCompile]
        protected override void OnCreate()
        {
            _inputActions = new();
            _inputActions.Project3.SetCallbacks(this); /// <see cref="InputActions.IProject3Actions"/>

            _inputEntityQuery = GetEntityQuery(typeof(Input));
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            if(_inputEntityQuery.CalculateEntityCount() == 0)
            {
                EntityManager.CreateEntity(typeof(Input));
            }
            
            // System variable receives input callbacks, writes to the Input component for use in ECS
            _inputEntityQuery.SetSingleton(new Input
            {
                InputAxisMove = _inputAxisMove, 
            });
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _inputAxisMove = context.ReadValue<Vector2>();
            //Debug.Log($"input axis move: {_inputAxisMove}");
        }
    }
}
