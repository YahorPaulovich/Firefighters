using Unity.Entities;
using UnityEngine;

public class SpeedAuthoring : MonoBehaviour 
{
    public float Value;

    public class Baker : Baker<SpeedAuthoring> 
    {
        public override void Bake(SpeedAuthoring authoring) 
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var data = new Speed 
            {
                Value = authoring.Value
            };
            AddComponent(entity, data);
        }
    }
}