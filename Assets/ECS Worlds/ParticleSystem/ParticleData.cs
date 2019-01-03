using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;


public struct ParticleData : IComponentData
{
    public float phaseOffset;
    public float3 xyzPhaseSpeed;
    public float3 xyzPhasePositionsWidth;
    public float3 xyzPolarizationValujeParticleCloudPatternOffset;
}
