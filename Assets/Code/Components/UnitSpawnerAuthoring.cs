using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Components
{
    public class UnitSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public float3 Size = new float3(1f, 0f, 1f);
        public float SpawnRate;
        public int MaxCount = 1000;
        public Transform Destination;
        
        private class UnitSpawnerAuthoringBaker : Baker<UnitSpawnerAuthoring>
        {
            public override void Bake(UnitSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new UnitSpawner
                {
                    // By default, each authoring GameObject turns into an Entity.
                    // Given a GameObject (or authoring component), GetEntity looks up the resulting Entity.
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                    Size = authoring.Size,
                    SpawnPosition = authoring.transform.position,
                    NextSpawnTime = 0.0f,
                    SpawnRate = authoring.SpawnRate,
                    Count = 0,
                    MaxCount = authoring.MaxCount,
                    Destination = authoring.Destination.position
                });
            }
        }
    }
}