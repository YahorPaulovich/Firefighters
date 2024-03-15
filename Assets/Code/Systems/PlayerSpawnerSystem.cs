using Malevolent;
using Unity.Entities;

[DisableAutoCreation]
public partial class PlayerSpawnerSystem : SystemBase 
{
    protected override void OnUpdate() 
    {
        const int spawnAmount = 2;
            
        var query = EntityManager.CreateEntityQuery(typeof(PlayerTag));
        var playerSpawner = SystemAPI.GetSingleton<PlayerSpawnerComponent>();

        var buffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
            
        if (query.CalculateEntityCount() < spawnAmount) 
        {
            var spawnedEntity = buffer.Instantiate(playerSpawner.PlayerPrefab);
            buffer.SetComponent(spawnedEntity, new Speed 
            {
                Value = DotsHelpers.GetRandomFloat(1, 10)
            });
        }
    }
}