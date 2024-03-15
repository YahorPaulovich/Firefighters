using Code.Components;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
//using ProjectDawn.Navigation;

namespace Code.Systems
{
    [BurstCompile]
    public partial struct UnitSpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);

        // Creates a new instance of the job, assigns the necessary data, and schedules the job in parallel.
        new ProcessUnitSpawnerJob
        {
            ElapsedTime = SystemAPI.Time.ElapsedTime,
            Ecb = ecb,
            Random = new Random(1)
            
        }.ScheduleParallel();
    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }
}

[BurstCompile]
public partial struct ProcessUnitSpawnerJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Ecb;
    public double ElapsedTime;
    public Random Random;

    // IJobEntity generates a component data query based on the parameters of its `Execute` method.
    // This example queries for all Spawner components and uses `ref` to specify that the operation
    // requires read and write access. Unity processes `Execute` for each entity that matches the
    // component data query.
    private void Execute([ChunkIndexInQuery] int chunkIndex, ref UnitSpawner spawner)
    {
        // If the next spawn time has passed.
        if (spawner.NextSpawnTime < ElapsedTime && spawner.Count <= spawner.MaxCount)
        {
            float3 offset = Random.NextFloat3(-spawner.Size, spawner.Size);
            float3 position = spawner.SpawnPosition + offset;

            // Spawns a new entity and positions it at the spawner.
            Entity newEntity = Ecb.Instantiate(chunkIndex, spawner.Prefab);            
            Ecb.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(position));

            // AgentBody body = AgentBody.Default;
            // body.SetDestination(spawner.Destination);
            // Ecb.SetComponent(chunkIndex, newEntity, body);

            // Resets the next spawn time.
            spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
            spawner.Count++;
        }
    }
}
}