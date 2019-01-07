using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;


public class VisualizerBuilder
{
    // Sub Base  Push  slightly WHole SPhere 

    private static VisualizerBuilder _instance;
    public static VisualizerBuilder Instance { get { return _instance; } }

    public int NumberOfStickPerSphericalRow = 60;
    public float BeetweenVisualSticksAngleStep { get { return Mathf.PI / NumberOfStickPerSphericalRow; } }
    public int SphericalRowsCount = 9;
    public float SphericalRowPhaseOffset { get { return (Mathf.PI*2) / SphericalRowsCount; } }

    public float DistanceFormOriginMultiplier = 4f;
    public float3 BaseVisualiserCellSize = new float3(2f,2f,2f);

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
    private SpectrumCellIdentityData[] _visualizerSticksIDDatas;
    public SpectrumCellIdentityData[] visualizerSticksIDDatas
    {
        get
        {
            if (_visualizerSticksIDDatas != null) { return _visualizerSticksIDDatas; }
            else
            {
                GenerateTemplateVisualizerSphereParameters();
                return _visualizerSticksIDDatas;
            }
        }
    }

    public float3 FindPositionForVisualizerForCellAtIndex(int i)
    {
        return VisualizerSticksPositions[i];
    }

    public quaternion FindRotationForVisualizerForCellAtIndex(int i)
    {
        return VisualizerSticksRotations[i];
    }
    public SpectrumCellIdentityData GetSpectrumCellIdentityDataForStickAtIndex(int i)
    {
        return visualizerSticksIDDatas[i];
    }
    private void GenerateTemplateVisualizerSphereParameters()
    {
        float3[] linearMatrixPositions = new float3[SphericalRowsCount * NumberOfStickPerSphericalRow];
        quaternion[] linearMatrixQuaternions = new quaternion[SphericalRowsCount * NumberOfStickPerSphericalRow];
        SpectrumCellIdentityData[] spectrumCellIdentityDatas = new SpectrumCellIdentityData[SphericalRowsCount * NumberOfStickPerSphericalRow];

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
                spectrumCellIdentityDatas[visualizerStickIndex] = new SpectrumCellIdentityData
                {
                    RowIndex = DefineRowTypeFromGivenRowIndex(i),
                    CellIndexInRow = j
                };
                visualizerStickIndex++;
            }
        }

        _visualizerSticksPositions = linearMatrixPositions;
        _visualizerSticksRotations = linearMatrixQuaternions;
        _visualizerSticksIDDatas = spectrumCellIdentityDatas;
    }

    private Dictionary<int, quaternion> CalculateRotations(Dictionary<int, float3> rowOfPositions)
    {
        Dictionary<int, quaternion> rotations = new Dictionary<int, quaternion>();
        for (int i = 0; i < rowOfPositions.Count; i++)
        {
            float3 position = rowOfPositions[i];
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

            xPosition = Mathf.Sin(alpha ) * Mathf.PI * (Mathf.Cos(offsetAtXZPhase + i) );
            zPosition = Mathf.Sin(alpha ) * Mathf.PI * (Mathf.Sin(offsetAtXZPhase + i) );
            yPosition = Mathf.Cos(alpha ) * Mathf.PI;

            float3 visualizerStickPosition = new float3(xPosition, yPosition, zPosition);
            sphericalRow[i] = visualizerStickPosition  * DistanceFormOriginMultiplier;
        }
        return sphericalRow;
    }
    private quaternion GetRotationTowardsPointZeroFor(float3 localPosition)
    {
        quaternion rotation = quaternion.LookRotationSafe((localPosition - float3.zero), new float3(0, 1, 0));
        return rotation;
    }
    private int DefineRowTypeFromGivenRowIndex(int givenIndex)
    {
        // there are 3 types of rows , visualizeing lows , mids , and highs .    Bass  spreads across all 
        // this is just definening 1 ,2 ,3  and for the next on loopng back to 1, .. and again  
        for(int i = 1; i <= SphericalRowsCount/3; i++)
        {
            if (givenIndex > i * 3 && givenIndex <= i * 3 + 3) { return givenIndex - 3 * i; }
        }
        return givenIndex;
    }
    public VisualizerBuilder()
    {
        _instance = this;
    }
}
