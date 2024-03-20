using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct InfectionSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Settings>();
        state.RequireForUpdate<Infected>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Settings settings = SystemAPI.GetSingleton<Settings>();

        var ecbSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();

        var processEntityStatus = new ProcessEntityStatus
        {
            ECB = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            deltaTime = SystemAPI.Time.DeltaTime,
            exposedToInfectedMultiplier = UnityEngine.Random.Range(settings.exposedToInfectedRangeMultiplier.x, settings.exposedToInfectedRangeMultiplier.y),
            infectedToRecoveringMultiplier = UnityEngine.Random.Range(settings.infectedToRecoveringRangeMultiplier.x, settings.infectedToRecoveringRangeMultiplier.y),
            recoveringToSusceptibleMultiplier = UnityEngine.Random.Range(settings.recoveringToSusceptibleRangeMultiplier.x, settings.recoveringToSusceptibleRangeMultiplier.y)
        };

        processEntityStatus.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ProcessEntityStatus : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public float deltaTime;
    public float exposedToInfectedMultiplier;
    public float infectedToRecoveringMultiplier;
    public float recoveringToSusceptibleMultiplier;

    [BurstCompile]
    public void Execute([ChunkIndexInQuery] int entityIndexInQuery, Entity entity, ref Target target, ref Infection infection)
    {
        switch (infection.currentState)
        {
            case CurrentState.Susceptible:
                ECB.SetComponentEnabled<Exposed>(entityIndexInQuery, entity, false);
                break;
            case CurrentState.Exposed:
                if (infection.infectionValue <= 100)
                {
                    ECB.SetComponent(entityIndexInQuery, entity, new Infection
                    {
                        infectionValue = infection.infectionValue + (exposedToInfectedMultiplier * deltaTime),
                        currentState = CurrentState.Exposed
                    });
                }
                else
                {
                    ECB.SetComponent(entityIndexInQuery, entity, new Infection
                    {
                        infectionValue = 100,
                        currentState = CurrentState.Infected
                    });
                    ECB.SetComponentEnabled<Infected>(entityIndexInQuery, entity, true);
                    ECB.SetComponentEnabled<Exposed>(entityIndexInQuery, entity, false);
                }
                break;
            case CurrentState.Infected:
                if (target.Value != Entity.Null)
                {
                    ECB.SetComponent(entityIndexInQuery, target.Value, new Infection
                    {
                        currentState = CurrentState.Exposed
                    });
                    ECB.SetComponentEnabled<Exposed>(entityIndexInQuery, target.Value, true);
                }

                if (infection.infectionValue >= 0)
                {
                    ECB.SetComponent(entityIndexInQuery, entity, new Infection
                    {
                        infectionValue = infection.infectionValue - (infectedToRecoveringMultiplier * deltaTime),
                        currentState = CurrentState.Infected
                    });
                }
                else
                {
                    ECB.SetComponent(entityIndexInQuery, entity, new Infection
                    {
                        infectionValue = 0,
                        currentState = CurrentState.Recovering
                    });
                }

                break;
            case CurrentState.Recovering:
                ECB.SetComponentEnabled<Infected>(entityIndexInQuery, entity, false);
                if (infection.infectionValue <= 100)
                {
                    ECB.SetComponent(entityIndexInQuery, entity, new Infection
                    {
                        infectionValue = infection.infectionValue + (recoveringToSusceptibleMultiplier * deltaTime),
                        currentState = CurrentState.Recovering
                    });
                    ECB.SetComponentEnabled<Exposed>(entityIndexInQuery, entity, true);
                }
                else
                {
                    ECB.SetComponent(entityIndexInQuery, entity, new Infection
                    {
                        infectionValue = 0,
                        currentState = CurrentState.Susceptible
                    });
                }
                break;
            default:
                break;
        }
    }
}