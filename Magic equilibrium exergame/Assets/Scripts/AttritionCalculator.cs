using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttritionCalculator 
{
    public static float flatDecel = -01f;
    public static float maxSlope = -45f;

    // Returns the deceleration/acceleration to be applied to player based on the ramp's slope
    public static float SlopeAttrition(float angleDegrees)
    {
        float acceleration = angleDegrees / maxSlope;
        return acceleration;
    }

    // Returns the deceleration/acceleration to be applied to player based on terrain properties
    public static float FlatAttrition(int difficulty)
    {
        float acceleration = difficulty * flatDecel;
        return acceleration;
    }
}
