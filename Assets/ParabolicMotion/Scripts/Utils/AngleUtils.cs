using UnityEngine;

public static class AngleUtils
{
    public static float RadiansToDegree(float radians)
    {
        return radians * 180 / Mathf.PI;
    } 
}
