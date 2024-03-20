using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial class SpawnCubesSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<Settings>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        Settings settings = SystemAPI.GetSingleton<Settings>();

        if (settings.simulationStarted)
        {
            this.Enabled = false;

            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(WorldUpdateAllocator);

            for (int i = 0; i < settings.amountToSpawn; i++)
            {
                Entity spawnedEntity = entityCommandBuffer.Instantiate(settings.cubePrefabEntity);

                // Generiamo posizioni casuali all'interno dell'area della simulazione
                float randomX = UnityEngine.Random.Range(-settings.simulationArea.x / 2f, settings.simulationArea.x / 2f);
                float randomZ = UnityEngine.Random.Range(-settings.simulationArea.y / 2f, settings.simulationArea.y / 2f);

                entityCommandBuffer.SetComponent(spawnedEntity, new LocalTransform
                {
                    Position = new float3(randomX, 0.6f, randomZ),
                    Rotation = quaternion.identity,
                    Scale = 1f
                });

                entityCommandBuffer.SetComponent(spawnedEntity, new Movement
                {
                    movementVector = new float3(UnityEngine.Random.Range(-1f, +1f), 0, UnityEngine.Random.Range(-1f, +1f))
                });

                // Calcoliamo la percentuale di apparire gia' infetto
                float randomValue = UnityEngine.Random.Range(0f, 1f);

                if (randomValue < settings.infectionChanceOnSpawn)
                {
                    entityCommandBuffer.SetComponent(spawnedEntity, new Infection
                    {
                        infectionValue = 100,
                        currentState = CurrentState.Infected
                    });
                    entityCommandBuffer.SetComponentEnabled<Infected>(spawnedEntity, true);
                    entityCommandBuffer.SetComponentEnabled<Exposed>(spawnedEntity, false);
                }
                else
                {
                    entityCommandBuffer.SetComponent(spawnedEntity, new Infection
                    {
                        infectionValue = 0,
                        currentState = CurrentState.Susceptible
                    });
                    entityCommandBuffer.SetComponentEnabled<Exposed>(spawnedEntity, false);
                }
            }

            // Infine generiamo anche il terreno dove verranno posizionati i cubi, con le dimensioni dell'area
            Entity terrainEntity = entityCommandBuffer.Instantiate(settings.terrainPrefabEntity);

            entityCommandBuffer.SetComponent(terrainEntity, new LocalTransform
            {
                Position = float3.zero,
                Rotation = quaternion.identity,
                Scale = 1f
            });

            // E lo adattiamo alle dimensioni dell'area
            entityCommandBuffer.AddComponent(terrainEntity, new PostTransformMatrix
            {
                Value = float4x4.Scale(settings.simulationArea.x / 10, 0, settings.simulationArea.y / 10)
            });

            entityCommandBuffer.Playback(EntityManager);
        }
    }
}
