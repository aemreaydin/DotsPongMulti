using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class CheckScoredSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
        Entities
            .WithAll<BallTag>()
            .WithoutBurst()
            .ForEach((Entity entity, in Translation translation) =>
            {
                if (translation.Value.x > GameManager.Main.XBounds)
                {
                    entityCommandBuffer.DestroyEntity(entity);
                    GameManager.Main.PlayerScored(0);
                }
                else if (translation.Value.x < -GameManager.Main.XBounds)
                {
                    entityCommandBuffer.DestroyEntity(entity);
                    GameManager.Main.PlayerScored(1);
                }
            }).Run();

        entityCommandBuffer.Playback(EntityManager);
        entityCommandBuffer.Dispose();
        
        return default;
    }
}