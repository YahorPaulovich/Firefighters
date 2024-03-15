using Malevolent;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct MoveToPositionAspect : IAspect 
{
    private readonly RefRW<LocalTransform> _localTransform;
    private readonly RefRO<Speed> _speed;
    private readonly RefRW<TargetPosition> _targetPosition;
    
    public MoveToPositionAspect(RefRW<LocalTransform> localTransform, RefRO<Speed> speed, RefRW<TargetPosition> targetPosition) 
    {
        _localTransform = localTransform;
        _speed = speed;
        _targetPosition = targetPosition;
    }
    
    public void Move(float deltaTime, float3 newTargetPosition)
    {
        var direction = math.normalize(_targetPosition.ValueRW.Value - _localTransform.ValueRW.Position);
        _localTransform.ValueRW.Position += direction * deltaTime * _speed.ValueRO.Value;
        
        if (DotsHelpers.IsCloseTo(_localTransform.ValueRW.Position, _targetPosition.ValueRW.Value, 0.5f)) 
        {
            _targetPosition.ValueRW.Value = newTargetPosition;
        }
    }
}