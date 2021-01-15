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
}
