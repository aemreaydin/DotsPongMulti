using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class PlayerMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deltaTime = Time.DeltaTime;
        var yBounds = GameManager.Main.YBounds;
        
        Entities
            .ForEach((ref Translation translation, in PlayerMovementData movement) =>
            {
                translation.Value = 
                    new float3(translation.Value.x,
                               math.clamp(translation.Value.y + movement.speed * movement.direction * deltaTime, -yBounds, yBounds),
                               translation.Value.z);
            })
            .Run();

        return default;
    }
}