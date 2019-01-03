using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine.Jobs;
using Unity.Rendering;
using Unity.Transforms;

public class AudioVisualizationSystemBootstrap : MonoBehaviour
{
    public VisualizerBuilder VisualizerBuilder = new VisualizerBuilder();
    public int SubVisualizarsCapacity = 300;
    public Mesh mesh;
    public Material material;





    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AfterScene()
    {
        AudioVisualizationSystemBootstrap audioVisualizationSystemBootstrap = GameObject.Find("AudioVisualizationSystemBootstrap").GetComponent<AudioVisualizationSystemBootstrap>();
        MeshInstanceRenderer VisualizerCellsRenderer = new MeshInstanceRenderer
        {
            mesh = audioVisualizationSystemBootstrap.mesh,
            material = audioVisualizationSystemBootstrap.material,
            subMesh = 0,
            castShadows = UnityEngine.Rendering.ShadowCastingMode.Off,
            receiveShadows = false
        };




        EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
        EntityArchetype VisualizerCellArchetype = entityManager.CreateArchetype(
            typeof(Position),
            typeof(Rotation),
            typeof(Scale),
            typeof(SpectrumCellValueData),
            typeof(SpectrumCellAttachmentData)
            
        );
        //EntityArchetype VisualizerCellArchetypeParented = entityManager.CreateArchetype(
        //   typeof(Position),
        //   typeof(Rotation),
        //   typeof(Scale),
        //   typeof(SpectrumCellValueData),
        //   typeof(SpectrumCellAttachmentData),  
        //   typeof(Attached)
        //);
        EntityArchetype Attach = entityManager.CreateArchetype(
          typeof(Attach)
        );



        EntityArchetype VisualizerInstanceArchetype = entityManager.CreateArchetype(
            typeof(VisualizerIDData),
            typeof(Position),
            typeof(Rotation)
        );

        Entity VisualizerInstance = entityManager.CreateEntity(VisualizerInstanceArchetype); 
            entityManager.SetComponentData(VisualizerInstance, new VisualizerIDData { VizualizerID = VisualizerInstance.Index });
            entityManager.SetComponentData(VisualizerInstance, new Position { Value = new float3(-5, 10, -10 ) });
            entityManager.SetComponentData(VisualizerInstance, new Rotation { Value = quaternion.identity });

        int loadindex = 0;
        int VisualizerInstanceLoadingIndex = 0;
        for (int i = 0; i < audioVisualizationSystemBootstrap.SubVisualizarsCapacity; i++)
        {
            loadindex++;
            Entity singleVisualizaerCellEntity = entityManager.CreateEntity(VisualizerCellArchetype);

            entityManager.SetComponentData(singleVisualizaerCellEntity, new Position
            {
                Value = VisualizerBuilder.Instance.FindPositionForSphericalVisualizerForStickAtIndex(i)//new float3(0, 0, 0)
            });
            entityManager.SetComponentData(singleVisualizaerCellEntity, new Rotation
            {
                Value = VisualizerBuilder.Instance.FindRotationForSphericalVisualizerForStickAtIndex(i)
            });
            entityManager.SetComponentData(singleVisualizaerCellEntity, new Scale
            {
                Value = VisualizerBuilder.Instance.BaseSize
            });
            entityManager.SetComponentData(singleVisualizaerCellEntity, new SpectrumCellValueData
            {
                Value = 0f
            });


            if (i < loadindex)
            {
                entityManager.SetComponentData<SpectrumCellAttachmentData>(singleVisualizaerCellEntity, new SpectrumCellAttachmentData
                {
                    VisualizerInstanceEntityIndex = VisualizerInstanceLoadingIndex
                });
            }
            else VisualizerInstanceLoadingIndex++;

            Entity attachment = entityManager.CreateEntity(Attach);
            entityManager.SetComponentData(attachment, new Attach { Parent = VisualizerInstance, Child = singleVisualizaerCellEntity });

            entityManager.AddSharedComponentData<MeshInstanceRenderer>(singleVisualizaerCellEntity, VisualizerCellsRenderer);
        }


    }

    private void Update()
    {
        //PhaseManager.UpdatePhaser();
    }


}

