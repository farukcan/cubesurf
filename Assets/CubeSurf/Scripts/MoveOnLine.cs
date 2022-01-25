using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnLine : MonoBehaviour
{
    public Transform line;
    public Transform movingObject;

    // find closest point on infinite line
    private Vector3 ClosestPointOnInfiniteLine()
    {
        Vector3 lsh = movingObject.position - line.position;
        float dotP = Vector3.Dot(lsh, line.forward);
        return line.position + line.forward * dotP;
    }

    // set ClosestPointOnInfiniteLine as position every frame
    private void Update()
    {
        transform.position = ClosestPointOnInfiniteLine();
    }

    // draw gizmos line with line.forward and line.position
    private void OnDrawGizmos()
    {
        var closestPoint = ClosestPointOnInfiniteLine();
        Gizmos.color = Color.green;
        Gizmos.DrawLine(closestPoint, closestPoint + line.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(closestPoint, closestPoint - line.forward);
    }
}
