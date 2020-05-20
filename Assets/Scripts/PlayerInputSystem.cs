using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class PlayerInputSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .ForEach((ref PlayerMovementData movement, in PlayerInputData input) =>
            {
                movement.direction = 0;
                
                movement.direction += Input.GetKey(input.up) ? 1 : 0;
                movement.direction -= Input.GetKey(input.down) ? 1 : 0;
            })
            .Run();

        return default;
    }
}