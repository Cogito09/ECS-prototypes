using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public static class PhaseManager
{
    private static float basePhaseDuration = 300f;
    private static float phaseStartTime;
    private static float phaseFinishTime;

    private static float phaseTme;
    private static float phaseAngleStepAtThisFrame;

    public static void UpdatePhaser()
    {
        if (Time.time > phaseFinishTime)
        {
            phaseStartTime = Time.time;
            phaseFinishTime = phaseStartTime + basePhaseDuration;
        }
        // one cycle is represented as 360 degrees  

        phaseTme = ((Time.time - phaseStartTime) * 360) / basePhaseDuration;
        phaseAngleStepAtThisFrame = (Time.deltaTime * 360) / basePhaseDuration;

    }
    public static float GetPhaserAngleStepAtThisFrame()
    {
        return phaseAngleStepAtThisFrame;
    }
    public static float GetPhaserTimeData()
    {
        return phaseTme;
    }

}
