using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using Unity.Mathematics;
using Unity.Entities;
using UnityEngine.Jobs;
using Unity.Rendering;
using Unity.Transforms;


[UpdateBefore(typeof(ParticleMoveSystem))]
public class AbstractiveParticleSystem : MonoBehaviour
{
    public int ParticleSystemCapacity;
    public Mesh mesh;
    public Material material;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AfterScene()
    {
        AbstractiveParticleSystem abstractiveParticleSystem = GameObject.Find("AbstractiveParticleSystem").GetComponent<AbstractiveParticleSystem>();
        MeshInstanceRenderer ParticleRenderer = new MeshInstanceRenderer
        {
            mesh = abstractiveParticleSystem.mesh,
            material = abstractiveParticleSystem.material,
            subMesh = 0,
            castShadows = UnityEngine.Rendering.ShadowCastingMode.Off,
            receiveShadows = false
        };

        EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
        EntityArchetype particleArchetype = entityManager.CreateArchetype(
            typeof(Position),
            typeof(ParticleData)
        );  

        for(int i = 0; i < abstractiveParticleSystem.ParticleSystemCapacity; i++)
        {
            Entity particleEntity = entityManager.CreateEntity(particleArchetype);

            entityManager.SetComponentData(particleEntity, new ParticleData {
                phaseOffset = UnityEngine.Random.Range(1f, 360f),
                xyzPhaseSpeed = new float3(1f, 2f, 1f),
                xyzPhasePositionsWidth = new float3(UnityEngine.Random.Range(5,20), UnityEngine.Random.Range(5, 20), UnityEngine.Random.Range(5, 15)),
                xyzPolarizationValujeParticleCloudPatternOffset = new float3(UnityEngine.Random.Range(-1,-0.8f), UnityEngine.Random.Range(-1f,-0.8f), UnityEngine.Random.Range(-0.6f, -1f))
            });
            entityManager.AddSharedComponentData<MeshInstanceRenderer>(particleEntity, ParticleRenderer);
        }
    }

    private void Update()
    {
        PhaseManager.UpdatePhaser();
    }

}
 