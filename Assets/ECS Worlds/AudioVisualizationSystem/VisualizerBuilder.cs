using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public enum SphereArea
{
    I,   
    II,
    III,
    IV,
    V,
    VI,
    VII,
    VIII
}


public class VisualizerBuilder
{

    // whole matrix offset by speed calculated on  specturmValue Gain % per second 
    // Sub Base  Push  slightly WHole SPhere 

    // Great thanks to Mr. Marko Rodin  Teachings
    private static VisualizerBuilder _instance;
    public static VisualizerBuilder Instance { get { return _instance; } }

    public int NumberOfStickPerSphericalRow = 60;
    public float BeetweenVisualSticksAngleStep { get { return Mathf.PI / NumberOfStickPerSphericalRow; } }
    public int SphericalRowsCount = 9;
    public float SphericalRowPhaseOffset { get { return (Mathf.PI*2) / SphericalRowsCount; } }

    public float DistanceFormOriginMultiplier = 4f;
    public float3 BaseSize = new float3(2f,2f,2f);

    private float3[] _visualizerSticksPositions;
    public float3[] VisualizerSticksPositions
    { get
        { if (_visualizerSticksPositions != null) { return _visualizerSticksPositions; }
            else
            {
                GenerateTemplateVisualizerSphereParameters();
                return _visualizerSticksPositions;
            }
        } }
    private quaternion[] _visualizerSticksRotations;
    public quaternion[] VisualizerSticksRotations
    {
        get
        {
            if (_visualizerSticksPositions != null) { return _visualizerSticksRotations; }
            else
            {
                GenerateTemplateVisualizerSphereParameters();
                return _visualizerSticksRotations;
            }
        }
    }

    public float3 FindPositionForSphericalVisualizerForStickAtIndex(int i)
    {
        return VisualizerSticksPositions[i];
    }

    public quaternion FindRotationForSphericalVisualizerForStickAtIndex(int i)
    {
        return VisualizerSticksRotations[i];
    }

    private void GenerateTemplateVisualizerSphereParameters()
    {
        float3[] linearMatrixPositions = new float3[SphericalRowsCount * NumberOfStickPerSphericalRow];
        quaternion[] linearMatrixQuaternions = new quaternion[SphericalRowsCount * NumberOfStickPerSphericalRow];

        Dictionary<int, Dictionary<int, float3>> SphericalRowsPositions = new Dictionary<int, Dictionary<int, float3>>();
        Dictionary<int, Dictionary<int, quaternion>> SphericalRowsQuaternions = new Dictionary<int, Dictionary<int, quaternion>>();

        for (int i = 0; i < SphericalRowsCount; i++)
        {
            SphericalRowsPositions[i] = GenerateRow(1, i * SphericalRowPhaseOffset);
            SphericalRowsQuaternions[i] = CalculateRotations(SphericalRowsPositions[i]);
        }

        int visualizerStickIndex = 0;
        for (int i = 0; i < SphericalRowsCount; i++)
        {
            for (int j = 0; j < NumberOfStickPerSphericalRow; j++)
            {
                linearMatrixPositions[visualizerStickIndex] = SphericalRowsPositions[i][j];
                linearMatrixQuaternions[visualizerStickIndex] = SphericalRowsQuaternions[i][j];
                visualizerStickIndex++;
            }
        }

        _visualizerSticksPositions = linearMatrixPositions;
        _visualizerSticksRotations = linearMatrixQuaternions;
    }
    public Dictionary<int, quaternion> CalculateRotations(Dictionary<int, float3> rowOfPositions)
    {
        Dictionary<int, quaternion> rotations = new Dictionary<int, quaternion>();
        for (int i = 0; i < rowOfPositions.Count; i++)
        {
            float3 position = rowOfPositions[i];

            //One Day im gonna make it perfect and rewrrite eveything on 8 Areas 

            GetRotationTowardsPointZeroFor(position);

            //float yrX = position.x;
            //float yrZ = position.z;
            //float rotationY;
            //rotationY = Mathf.Atan2(Mathf.Abs(yrX), Mathf.Abs(yrZ) );

            //float xrY = position.y;
            //float xrZ = position.z;
            //float rotationX;

            //rotationX = Mathf.Atan(xrZ / xrY);
            //rotations[i] = quaternion.LookRotation((float3.zero - position), new float3(0,1,0));
            rotations[i] = GetRotationTowardsPointZeroFor(position);
        }
        return rotations;
    }

private Dictionary<int, float3> GenerateRow(int numberofOrbitationsPerCycle, float offsetAtXZPhase)
    {
        Dictionary<int, float3> sphericalRow = new Dictionary<int, float3>();
        float yPosition;
        float zPosition;
        float xPosition;

        for (int i = 0; i < NumberOfStickPerSphericalRow; i++)
        {
            float alpha = i * BeetweenVisualSticksAngleStep;

            xPosition = Mathf.Sin(alpha ) * Mathf.PI * (Mathf.Cos(offsetAtXZPhase + i) ) ;
            zPosition = Mathf.Sin(alpha ) * Mathf.PI * (Mathf.Sin(offsetAtXZPhase + i) );
            yPosition = Mathf.Cos(alpha ) * Mathf.PI;

            float3 visualizerStickPosition = new float3(xPosition, yPosition, zPosition);
            sphericalRow[i] = visualizerStickPosition  * DistanceFormOriginMultiplier;
        }
        return sphericalRow;
    }
    public quaternion GetRotationTowardsPointZeroFor(float3 localPosition)
    {

        float y = localPosition.y;
        float z = localPosition.z;
        float x = localPosition.x;

        if(y < 1 && y > -1) { y = 1; }
        if(x < 1 && x > -1) { y = 1; }
        if(z < 1 && z > -1) { z = 1; }

        float rotationZ = math.atan(math.abs(y) / math.abs(x));
        float rotationY = math.atan(math.abs(z) / math.abs(x));


        SphereArea area = GetArea(localPosition);
        if(area == SphereArea.I)
        {
            rotationY *= -1;
        }
        else if(area == SphereArea.II)
        {
            rotationY = -180 + rotationY;
        }
        else if (area == SphereArea.III)
        {
            rotationY = -260 + rotationY;
        }
        else if (area == SphereArea.IV)
        {
            rotationY = -360 + rotationY;
        }
        else if (area == SphereArea.V)
        {
            rotationZ *= -1;
        }
        else if (area == SphereArea.VI)
        {
            rotationY -= 90;
            rotationZ *= -1;
        }
        else if (area == SphereArea.VII)
        {
            rotationY -= 180;
            rotationZ *= -1;
        }
        else if (area == SphereArea.VIII)
        {
            rotationY -= 260;
            rotationZ *= -1;
        }

        
        quaternion rotation = quaternion.Euler(0, rotationY, rotationZ, math.RotationOrder.ZYX);

        return rotation;
    }
    public SphereArea GetArea(float3 localPosition)
    {
        SphereArea area = new SphereArea();
        if (localPosition.x >= 0 && localPosition.y >= 0 && localPosition.z >= 0) { area = SphereArea.I; }
        else if (localPosition.x < 0 && localPosition.y >= 0 && localPosition.z >= 0) { area = SphereArea.II; }
        else if (localPosition.x < 0 && localPosition.y >= 0 && localPosition.z < 0) { area = SphereArea.III; }
        else if (localPosition.x >= 0 && localPosition.y >= 0 && localPosition.z < 0) { area = SphereArea.IV; }
        else if (localPosition.x >= 0 && localPosition.y < 0 && localPosition.z >= 0) { area = SphereArea.V; }
        else if (localPosition.x < 0 && localPosition.y < 0 && localPosition.z >= 0) { area = SphereArea.VI; }
        else if (localPosition.x < 0 && localPosition.y < 0 && localPosition.z < 0) { area = SphereArea.VII; }
        else if (localPosition.x >= 0 && localPosition.y < 0 && localPosition.z < 0) { area = SphereArea.VIII; }
        return area;
    }
    public VisualizerBuilder()
    {
        _instance = this;
    }
}
