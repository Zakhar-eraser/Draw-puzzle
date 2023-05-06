using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Vehicle : MonoBehaviour
{
    [SerializeField]
    private GoalType type = GoalType.NONE;
    [SerializeField]
    private Path roadSegment;

    private Camera cam;
    private Path currentPath;
    private float speed;
    private const float splineThr = 0.999f;
    private float currentSplinePos = 0;
    private bool riding = false;
    public bool anchored { get; private set; } = false;

    public static Action onCrashed;
    public static Action onVehicleCame;

    void Start()
    {
        cam = Camera.main;
        LevelResultHandler.onAllGoalsOccupied += StartRide;
    }

    private void FixedUpdate()
    {
        if (riding)
        {
            if (currentSplinePos < splineThr)
            {
                Vector3 newPos = currentPath.spline.GetPoint(currentSplinePos);
                Vector3 tangent = currentPath.spline.GetTangent(currentSplinePos);
                Quaternion rot = Quaternion.LookRotation(transform.forward, tangent);
                transform.rotation = rot;
                transform.localPosition = newPos;
                currentSplinePos += Time.deltaTime * speed;
            }
            else
            {
                transform.localPosition = currentPath.spline.GetPoint(1);
                riding = false;
                onVehicleCame?.Invoke();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Vehicle>() != null)
        {
            riding = false;
            onCrashed?.Invoke();
        }
    }

    public void StartRide(float speed)
    {
        LevelResultHandler.onAllGoalsOccupied -= StartRide;
        this.speed = speed;
        riding = true;
    }    

    public GoalType GetGoalType()
    {
        return type;
    }

    public void BeginDrag(BaseEventData data)
    {
        if (anchored == false)
        {
            Vector2 startPos = transform.position;
            currentPath = Instantiate<Path>(roadSegment, startPos,
                Quaternion.identity);
        }
    }

    public void Drag(BaseEventData data)
    {
        if (anchored == false)
        {
            PointerEventData pdata = data as PointerEventData;
            currentPath.UpdatePath(cam.ScreenToWorldPoint(pdata.position));
        }
    }

    public void EndDrag()
    {
        if (anchored == false)
            Destroy(currentPath.gameObject);
    }

    public void MakeAnchor(Vector2 anchor)
    {
        currentPath.UpdatePath(anchor);
        transform.SetParent(currentPath.transform);
        currentPath.UpdateSpline();
        anchored = true;
    }
}
