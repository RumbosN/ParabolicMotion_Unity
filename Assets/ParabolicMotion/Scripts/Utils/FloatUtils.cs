using System;
using UnityEngine;

public static class FloatUtils
{
    public static float Floor(float n, int digits) {
	    var m = Math.Pow(10, digits);
        return ((int) (n * m)) / (float)m;
    }
}
