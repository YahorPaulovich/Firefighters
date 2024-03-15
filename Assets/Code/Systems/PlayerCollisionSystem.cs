using Malevolent;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;

[DisableAutoCreation]
[BurstCompile]
public partial struct PlayerCollisionSystem : ISystem
{
    private ComponentLookup<PlayerTag> _playerGroup;
    private ComponentLookup<URPMaterialPropertyBaseColor> _colorGroup;
    private float4 _color;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
        _playerGroup = state.GetComponentLookup<PlayerTag>(true);
        _colorGroup = state.GetComponentLookup<URPMaterialPropertyBaseColor>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _colorGroup.Update(ref state);
        _playerGroup.Update(ref state);
        //var simulationSingleton = SystemAPI.GetSingletonRW<SimulationSingleton>().ValueRW;
        _color = DotsHelpers.GetRandomColor();
        //
        // var job = new PlayerTriggerJob 
        // {
        //     PlayerGroup = playerGroup,
        //     ColorGroup = colorGroup,
        //     color = color
        // };
        // state.Dependency = job.Schedule(simulationSingleton, state.Dependency);
        foreach (var triggerEvent in SystemAPI.GetSingletonRW<SimulationSingleton>()
                     .ValueRW.AsSimulation()
                     .TriggerEvents)
        {
            bool isEntityAPerson = _playerGroup.HasComponent(triggerEvent.EntityA);
            bool isEntityBPerson = _playerGroup.HasComponent(triggerEvent.EntityB);
            if (isEntityAPerson && isEntityBPerson)
            {
                ChangeMaterialColor(triggerEvent.EntityB);
            }
        }
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
    void ChangeMaterialColor(Entity entity)
    {
        _colorGroup[entity] = new URPMaterialPropertyBaseColor
        {
            Value = _color
        };
    }
}

// [BurstCompile]
// struct PlayerTriggerJob : ITriggerEventsJob
// {
//     [ReadOnly] public ComponentLookup<PlayerTag> PlayerGroup;
//     public ComponentLookup<URPMaterialPropertyBaseColor> ColorGroup;
//     public float4 color;
//     [BurstCompile]
//     public void Execute(TriggerEvent triggerEvent)
//     {
//         bool isEntityAPerson = PlayerGroup.HasComponent(triggerEvent.EntityA);
//         bool isEntityBPerson = PlayerGroup.HasComponent(triggerEvent.EntityB);
//         if (isEntityAPerson && isEntityBPerson)
//         {
//             ChangeMaterialColor(triggerEvent.EntityB);
//         }
//     }
//     void ChangeMaterialColor(Entity entity)
//     {
//         ColorGroup[entity] = new URPMaterialPropertyBaseColor
//         {
//             Value = color
//         };
//     }
// }