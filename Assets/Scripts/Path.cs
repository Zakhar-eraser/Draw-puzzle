using UnityEngine;
using CurveLib.Curves;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Path : MonoBehaviour
{
    [SerializeField]
    private float segmentLength = 5.0f;

    private float segLenSq;
    private LineRenderer rend;
    public SplineCurve spline { get; private set; }

    public void UpdateSpline()
    {
        Vector3[] positions = new Vector3[rend.positionCount];
        rend.GetPositions(positions);
        spline = new SplineCurve(positions);
    }

    void Awake()
    {
        segLenSq = segmentLength * segmentLength;
        rend = GetComponent<LineRenderer>();
    }

    public void UpdatePath(Vector2 waypoint)
    {
        int lastIdx = rend.positionCount - 1;
        Vector2 updatedPos = transform.InverseTransformPoint(waypoint);
        Vector2 lastWaypoint = rend.GetPosition(lastIdx - 1);
        if (Vector2.SqrMagnitude(lastWaypoint - updatedPos) > segLenSq)
            rend.positionCount++;
        rend.SetPosition(rend.positionCount - 1, updatedPos);
    }
}
