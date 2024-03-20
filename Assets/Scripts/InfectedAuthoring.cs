using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class InfectedAuthoring : MonoBehaviour
{
    public class Baker : Baker<InfectedAuthoring>
    {
        public override void Bake(InfectedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Infected());
            SetComponentEnabled<Infected>(entity, false);
        }
    }
}

public struct Infected : IComponentData, IEnableableComponent
{

}
