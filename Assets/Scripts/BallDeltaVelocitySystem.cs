using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

[AlwaysSynchronizeSystem]
public class BallDeltaVelocitySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deltaTime = Time.DeltaTime;

        Entities
            .WithName("Ball_Delta_Velocity")
            .ForEach((ref PhysicsVelocity physicsVelocity, in BallDeltaVelocityData velocityData) =>
            {
                var delta = new float2(velocityData.deltaVelocityPerDeltaTime * deltaTime);

                var newVelocity = physicsVelocity.Linear.xy;
                newVelocity += math.lerp(-delta, delta, math.sign(newVelocity));

                physicsVelocity.Linear.xy = newVelocity;
            })
            .Run();

        return default;
    }
}