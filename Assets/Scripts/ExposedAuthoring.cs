using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ExposedAuthoring : MonoBehaviour
{
    public class Baker : Baker<ExposedAuthoring>
    {
        public override void Bake(ExposedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Exposed());
            SetComponentEnabled<Exposed>(entity, false);
        }
    }
}

public struct Exposed : IComponentData, IEnableableComponent
{

}
