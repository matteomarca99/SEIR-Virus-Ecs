using UnityEngine;
using Unity.Entities;
using System.ComponentModel;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using TMPro;
using System;

public class SettingsInitializer : MonoBehaviour
{
    public GameObject performanceUI;
    public FlyCamera flyCamera;
    public TMP_InputField amountToSpawn;
    public TMP_InputField infectionChanceOnSpawn;
    public TMP_InputField exposedToInfectedRangeMultiplier_x;
    public TMP_InputField exposedToInfectedRangeMultiplier_y;
    public TMP_InputField infectedToRecoveringRangeMultiplier_x;
    public TMP_InputField infectedToRecoveringRangeMultiplier_y;
    public TMP_InputField recoveringToSusceptibleRangeMultiplier_x;
    public TMP_InputField recoveringToSusceptibleRangeMultiplier_y;
    public TMP_InputField simulationArea_x;
    public TMP_InputField simulationArea_y;

    public void StartSimulation()
    {
        EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityQuery q = em.CreateEntityQuery(new EntityQueryDesc()
        {
            All = new ComponentType[] { ComponentType.ReadWrite<Settings>() }
        });
        if (q.TryGetSingleton<Settings>(out var newSettings))
        {
            Entity settingsEntity = q.GetSingletonEntity();

            newSettings.amountToSpawn = int.Parse( amountToSpawn.text);
            newSettings.infectionChanceOnSpawn = float.Parse(infectionChanceOnSpawn.text);

            newSettings.exposedToInfectedRangeMultiplier = new float2(
                float.Parse(exposedToInfectedRangeMultiplier_x.text),
                float.Parse(exposedToInfectedRangeMultiplier_y.text)
                );

            newSettings.infectedToRecoveringRangeMultiplier = new float2(
                float.Parse(infectedToRecoveringRangeMultiplier_x.text),
                float.Parse(infectedToRecoveringRangeMultiplier_y.text)
                );

            newSettings.recoveringToSusceptibleRangeMultiplier = new float2(
                float.Parse(recoveringToSusceptibleRangeMultiplier_x.text),
                float.Parse(recoveringToSusceptibleRangeMultiplier_y.text)
                );

            newSettings.simulationArea = new float2(
                float.Parse(simulationArea_x.text),
                float.Parse(simulationArea_y.text)
                );

            newSettings.simulationStarted = true;

            em.SetComponentData(settingsEntity, newSettings);

            flyCamera.enabled = true;
            performanceUI.SetActive(true);

            Destroy(gameObject);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
