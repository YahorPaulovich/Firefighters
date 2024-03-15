using Unity.Entities;
using Unity.Mathematics;

namespace Code.Components
{
    public struct UnitSpawner : IComponentData
    {
        public Entity Prefab;
        public float3 SpawnPosition;
        public float NextSpawnTime;
        public float SpawnRate;
        public float3 Size;
        public int Count;
        public int MaxCount;
        public float3 Destination;
    }
}