using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSpawnerAuthoring : MonoBehaviour 
{
    public GameObject PlayerPrefab;
    
    public class Baker : Baker<PlayerSpawnerAuthoring> 
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Renderable);
            var prefabEntity = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Renderable);
                
            var data = new PlayerSpawnerComponent 
            {
                PlayerPrefab = prefabEntity
            };
            AddComponent(entity, data);
        }
    }
}