using Unity.Entities;

[GenerateAuthoringComponent]
public struct BallDeltaVelocityData : IComponentData
{
    public float deltaVelocityPerDeltaTime;
}