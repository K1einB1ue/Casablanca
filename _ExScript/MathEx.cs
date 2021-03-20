using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathEx
{
    public static Vector3 GetIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal, Vector3 planePoint) {
        float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);

        return d * direct.normalized + point;
    }

    public static Vector3 GetIntersectWithLineAndPlane(this Ray ray, Plane plane) {
        float d = Vector3.Dot(plane.ClosestPointOnPlane(Vector3.zero) - ray.origin, plane.normal) / Vector3.Dot(ray.direction.normalized, plane.normal);

        return d * ray.direction.normalized + plane.ClosestPointOnPlane(Vector3.zero);
    }

    public static Vector3 GetIntersectWithLineAndPlane(this Plane plane,  Ray ray) {
        float d = Vector3.Dot(plane.ClosestPointOnPlane(Vector3.zero) - ray.origin, plane.normal) / Vector3.Dot(ray.direction.normalized, plane.normal);

        return d * ray.direction.normalized + plane.ClosestPointOnPlane(Vector3.zero);
    }
    public static float Distance(this Vector3 in1, Vector3 in2) {
        return Mathf.Sqrt((Mathf.Pow(in1.x - in2.x, 2) + Mathf.Pow(in1.y - in2.y, 2) + Mathf.Pow(in1.z - in2.z, 2)));
    }
    public static float Distance2(this Vector3 in1,Vector3 in2) {
        return Mathf.Pow(in1.x - in2.x, 2) + Mathf.Pow(in1.y - in2.y, 2) + Mathf.Pow(in1.z - in2.z, 2);
    }
    public static float scalarization(this Vector3 V3) {
        return Mathf.Sqrt(V3.x * V3.x + V3.y * V3.y + V3.z * V3.z);
    }

    public static Vector3 normalize(Vector3 V3) {
        return V3 / scalarization(V3);
    }

    public static float RemapTo(float In, float Inmin, float Inmax, float OutMin, float OutMax) {
        if (In < Inmin) {
            return OutMin;
        }
        else if (In > Inmax) {
            return OutMax;
        }

        return ((In - Inmin) / (Inmax - Inmin)) * (OutMax - OutMin) + OutMin;
    }
    public static float RemapTo(this AnimationCurve curve, float In, float OutMin, float OutMax) {
        if (In < 0) {
            return OutMin;
        }
        else if(In > 1) {
            return OutMax;
        }
        return curve.Evaluate(In) * (OutMax - OutMin) + OutMin;
    }
}
