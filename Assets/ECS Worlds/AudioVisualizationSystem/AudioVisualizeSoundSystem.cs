using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;

//[BurstCompile]
//public class AudioVisualizeSoundSystem : JobComponentSystem
//{
//    protected override void OnCreateManager()
//    {
//        base.OnCreateManager();
//    }
//    public struct VisualizeAudioJob : IJobProcessComponentData<Scale>
//    {
//        public void Execute(ref Scale scale)
//        {
//            scale.Value.z += 0.001f;
//        }
//    }

//    protected override JobHandle OnUpdate(JobHandle inputDeps)
//    {
//        VisualizeAudioJob visualizeAudioJob = new VisualizeAudioJob()
//        { };

//        return visualizeAudioJob.Schedule(this, inputDeps);
//    }
//}
