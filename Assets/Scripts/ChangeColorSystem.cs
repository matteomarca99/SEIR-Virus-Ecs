using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial class ChangeColorSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<Infection>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        ChangeColorJob changeColorSystemJob = new ChangeColorJob();

        changeColorSystemJob.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct ChangeColorJob : IJobEntity
    {
        public void Execute(ref URPMaterialPropertyBaseColor materialBaseColor, in Infection infection)
        {
            float4 colorValue = default;

            switch (infection.currentState)
            {
                case CurrentState.Susceptible:
                    colorValue = new float4(Color.blue.r, Color.blue.g, Color.blue.b, Color.blue.a);
                    break;
                case CurrentState.Exposed:
                    colorValue = new float4(Color.yellow.r, Color.yellow.g, Color.yellow.b, Color.yellow.a);
                    break;
                case CurrentState.Infected:
                    colorValue = new float4(Color.red.r, Color.red.g, Color.red.b, Color.red.a);
                    break;
                case CurrentState.Recovering:
                    colorValue = new float4(Color.green.r, Color.green.g, Color.green.b, Color.green.a);
                    break;
                default:
                    break;
            }

            materialBaseColor.Value = colorValue;
        }
    }
}