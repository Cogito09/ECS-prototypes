using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;


public class ParticleMoveSystem : JobComponentSystem
{
#pragma warning disable 649
    struct ParticleMoveData
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<ParticleData> particleDatas;
        public ComponentDataArray<Position> particlePositions;
    }
#pragma warning restore 649

    [Inject] ParticleMoveData m_particleMoveData;

    [BurstCompile]
    struct MoveParticles : IJobParallelFor
    {
        public ComponentDataArray<Position> particlePositions;
        [ReadOnly]
        public ComponentDataArray<ParticleData> particleDatas;
        public float phaserTimeData;
        public float phaserFrameTimeStepData;

        public void Execute(int index)
        {
            double phaseTime = phaserTimeData + particleDatas[index].phaseOffset;
            double phaseTimeX = phaseTime + phaserFrameTimeStepData + particleDatas[index].xyzPhaseSpeed.x;
            double phaseTimeY = phaseTime + phaserFrameTimeStepData + particleDatas[index].xyzPhaseSpeed.y;
            double phaseTimeZ = phaseTime + phaserFrameTimeStepData + particleDatas[index].xyzPhaseSpeed.z;
           /*
            if(phaseTimeX > 360) { phaseTimeX -= 360; }
            if(phaseTimeX < 0) { phaseTimeX += 360; }
            if(phaseTimeY > 360) { phaseTimeY -= 360; }
            if(phaseTimeY < 0) { phaseTimeY += 360; }
            if (phaseTimeZ > 360) { phaseTimeZ -= 360; }
            if(phaseTimeZ < 0) { phaseTimeZ += 360; }
            */
            double x = math.sin(phaseTime) * particleDatas[index].xyzPolarizationValujeParticleCloudPatternOffset.x * particleDatas[index].xyzPhasePositionsWidth.x ;
            double y = math.sin(phaseTime) * particleDatas[index].xyzPolarizationValujeParticleCloudPatternOffset.y * particleDatas[index].xyzPhasePositionsWidth.y ;
            double z = math.cos(phaseTime) * particleDatas[index].xyzPolarizationValujeParticleCloudPatternOffset.z * particleDatas[index].xyzPhasePositionsWidth.z ;
            float3 position = new float3((float)x, (float)y, (float)z);
            particlePositions[index] = new Position { Value = position };
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        MoveParticles MoveParticles = new MoveParticles()
        {
            particlePositions = m_particleMoveData.particlePositions,
            particleDatas = m_particleMoveData.particleDatas,
            phaserTimeData = PhaseManager.GetPhaserTimeData(),
            phaserFrameTimeStepData = PhaseManager.GetPhaserAngleStepAtThisFrame()
        };
        JobHandle MoveParticlesJobHandle = MoveParticles.Schedule(m_particleMoveData.Length,1023, inputDeps);
        return MoveParticlesJobHandle;
    }
}
