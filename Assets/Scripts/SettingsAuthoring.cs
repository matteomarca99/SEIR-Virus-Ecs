using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SettingsAuthoring : MonoBehaviour
{
    public bool simulationStarted;
    public GameObject cubePrefab;
    public GameObject terrainPrefab;
    public int amountToSpawn;
    public float infectionChanceOnSpawn;
    public float2 exposedToInfectedRangeMultiplier;
    public float2 infectedToRecoveringRangeMultiplier;
    public float2 recoveringToSusceptibleRangeMultiplier;
    public float2 simulationArea;

    public class Baker : Baker<SettingsAuthoring>
    {
        public override void Bake(SettingsAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new Settings
            {
                simulationStarted = authoring.simulationStarted,
                cubePrefabEntity = GetEntity(authoring.cubePrefab, TransformUsageFlags.Dynamic),
                terrainPrefabEntity = GetEntity(authoring.terrainPrefab, TransformUsageFlags.None),
                amountToSpawn = authoring.amountToSpawn,
                infectionChanceOnSpawn = authoring.infectionChanceOnSpawn,
                exposedToInfectedRangeMultiplier = authoring.exposedToInfectedRangeMultiplier,
                infectedToRecoveringRangeMultiplier = authoring.infectedToRecoveringRangeMultiplier,
                recoveringToSusceptibleRangeMultiplier = authoring.recoveringToSusceptibleRangeMultiplier,
                simulationArea = authoring.simulationArea
            });
        }
    }
}

public struct Settings : IComponentData
{
    public bool simulationStarted;
    public Entity cubePrefabEntity;
    public Entity terrainPrefabEntity;
    public int amountToSpawn;
    public float infectionChanceOnSpawn;
    public float2 exposedToInfectedRangeMultiplier;
    public float2 infectedToRecoveringRangeMultiplier;
    public float2 recoveringToSusceptibleRangeMultiplier;
    public float2 simulationArea;
}
