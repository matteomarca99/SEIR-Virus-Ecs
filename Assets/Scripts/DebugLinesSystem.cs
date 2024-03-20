using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(TargetingSystem))]
[BurstCompile]
public partial struct DebugLinesSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Infected>();
        state.RequireForUpdate<Settings>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        Settings settings = SystemAPI.GetSingleton<Settings>();

        foreach (var (transform, target) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<Target>>().WithAll<Infected>())
        {
            if (SystemAPI.Exists(target.ValueRO.Value))
            {
                var targetTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Value);
                Debug.DrawLine(transform.ValueRO.Position, targetTransform.Position, Color.green);
            }
        }

        // Calcola i punti del contorno dell'area
        Vector3 bottomLeft = new Vector3(-settings.simulationArea.x / 2, 0.5f, -settings.simulationArea.y / 2);
        Vector3 bottomRight = new Vector3(settings.simulationArea.x / 2, 0.5f, -settings.simulationArea.y / 2);
        Vector3 topLeft = new Vector3(-settings.simulationArea.x / 2, 0.5f, settings.simulationArea.y / 2);
        Vector3 topRight = new Vector3(settings.simulationArea.x / 2, 0.5f, settings.simulationArea.y / 2);

        // Traccia le linee del contorno dell'area
        Debug.DrawLine(bottomLeft, bottomRight, Color.red);
        Debug.DrawLine(bottomRight, topRight, Color.red);
        Debug.DrawLine(topRight, topLeft, Color.red);
        Debug.DrawLine(topLeft, bottomLeft, Color.red);
    }
}


