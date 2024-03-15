using Malevolent;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[DisableAutoCreation]
[BurstCompile]
public partial struct MovingSystem : ISystem 
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) { }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state) 
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        var targetPosition = DotsHelpers.GetRandomPosition();
        var moveJob = new MoveJob
        {
            DeltaTime = deltaTime,
            TargetPosition = targetPosition
        };
        
        //var b = moveJob.ScheduleParallel();
        
        //state.Dependency = moveJob.Schedule(simulationSingleton, state.Dependency);
    }
    
    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }
}

[BurstCompile]
public partial struct MoveJob : IJobEntity
{
    public float DeltaTime;
    public float3 TargetPosition;

    [BurstCompile]
    void Execute(MoveToPositionAspect aspect)
    {
        aspect.Move(DeltaTime, TargetPosition);
    }
}