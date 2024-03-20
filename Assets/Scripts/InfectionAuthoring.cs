using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class InfectionAuthoring : MonoBehaviour
{
    public float infectionValue;
    public CurrentState currentState;

    public class Baker : Baker<InfectionAuthoring>
    {
        public override void Bake(InfectionAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Infection
            {
                infectionValue = authoring.infectionValue,
                currentState = authoring.currentState,
            });
        }
    }
}

public struct Infection : IComponentData
{
    public float infectionValue;
    public CurrentState currentState;
}

public enum CurrentState
{
    Susceptible,
    Exposed,
    Infected,
    Recovering
}