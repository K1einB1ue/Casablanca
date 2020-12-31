using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math
{
    public static float Distance(Vector3 in1,Vector3 in2) {
        return Mathf.Sqrt((Mathf.Pow(in1.x - in2.x, 2) + Mathf.Pow(in1.y - in2.y, 2) + Mathf.Pow(in1.z - in2.z, 2)));
    }

    public static float scalarization(Vector3 V3) {
        return Mathf.Sqrt(V3.x * V3.x + V3.y * V3.y + V3.z * V3.z);
    }

    public static Vector3 normalize(Vector3 V3) {
        return V3 / scalarization(V3);
    }
}
