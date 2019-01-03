using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
public class MoveCellsOnSphereSystem : JobComponentSystem
{


  
    public struct AttachToCellsToParentsJob : IJobProcessComponentDataWithEntity<SpectrumCellAttachmentData>
    {
        public BufferFromEntity<BufferWithDataOfAttachment> DataAttachmentBuffer;

        public void Execute(Entity entity, int index, ref SpectrumCellAttachmentData attachmentData)
        {
            DynamicBuffer<BufferWithDataOfAttachment> dataAttachmentBuffer = DataAttachmentBuffer[entity];

        }
    }

    public struct VisualizeAudioJob : IJobProcessComponentData<Scale>
    {
        public void Execute(ref Scale scale)
        {
            scale.Value.x += 0.001f;
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        VisualizeAudioJob visualizeAudioJob = new VisualizeAudioJob()
        { };

        return visualizeAudioJob.Schedule(this, inputDeps);
    }

    protected override void OnCreateManager()
    {



    }
}
