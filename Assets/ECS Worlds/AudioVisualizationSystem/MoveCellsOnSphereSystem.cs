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
    private int sphericalRowCapacity = 60;
    private AudioSource mainAudioSource;

    float[] spectrumData = new float[256];

    
    NativeArray<float> audioSpectrumData;

 
    public struct VisualizeAudioJob : IJobProcessComponentData<Scale,SpectrumCellIdentityData>
    {
        [DeallocateOnJobCompletion]
        [ReadOnly]
        public NativeArray<float> AudioSpectrumData;

        public void Execute(ref Scale scale,ref SpectrumCellIdentityData spectrumCellIdentityData)
        {
            int SpectrumValueForThisCell = (spectrumCellIdentityData.RowIndex * 60) + spectrumCellIdentityData.CellIndexInRow;
            if(SpectrumValueForThisCell < AudioSpectrumData.Length)
            {
                scale.Value.z = 1 + 1000 * AudioSpectrumData[SpectrumValueForThisCell];
            }
            
            
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        
        mainAudioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Triangle);
        audioSpectrumData = new NativeArray<float>(spectrumData,Allocator.TempJob);


        VisualizeAudioJob visualizeAudioJob = new VisualizeAudioJob()
        {
            AudioSpectrumData = audioSpectrumData
        };
        
        return visualizeAudioJob.Schedule(this, inputDeps);
    }
    
    

    protected override void OnCreateManager()
    {
        


    }
    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        mainAudioSource = Camera.main.GetComponent<AudioSource>();
    }
   
}
