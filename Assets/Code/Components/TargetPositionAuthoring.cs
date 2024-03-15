using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class TargetPositionAuthoring : MonoBehaviour 
{
    public float3 Value;

    public class Baker : Baker<TargetPositionAuthoring>
    {
        public override void Bake(TargetPositionAuthoring authoring) 
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var data = new TargetPosition 
            {
                Value = authoring.Value
            };
            AddComponent(entity, data);
        }
    }
}