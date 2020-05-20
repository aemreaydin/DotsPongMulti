using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PlayerMovementData : IComponentData
{
    public float direction;
    public float speed;
}