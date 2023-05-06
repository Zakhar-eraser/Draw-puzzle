using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private GoalType type = GoalType.NONE;

    public bool occupied { get; private set; } = false;

    public static Action onOccupied;

    public void Drop(BaseEventData data)
    {
        if (occupied == false)
        {
            PointerEventData pdata = data as PointerEventData;
            Vehicle vehicle;
            if (pdata.pointerDrag.TryGetComponent<Vehicle>(out vehicle))
            {
                if (type == GoalType.STORAGE || vehicle.GetGoalType() == type)
                {
                    occupied = true;
                    onOccupied?.Invoke();
                    vehicle.MakeAnchor(transform.position);
                }
            }
        }
    }
}
