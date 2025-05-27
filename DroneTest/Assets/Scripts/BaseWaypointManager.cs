using System.Collections.Generic;
using UnityEngine;

public class BaseWaypointManager : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    private Dictionary<Transform, bool> waypointAvailability = new Dictionary<Transform, bool>();

    private void Awake()
    {
        InitializeWaypoints();
    }

    private void InitializeWaypoints()
    {
        foreach (var waypoint in waypoints)
        {
            waypointAvailability[waypoint] = true;
        }
    }

    public void ClearAvailability()
    {
        InitializeWaypoints();
    }

    public Transform ReserveWaypoint()
    {
        foreach (var waypoint in waypoints)
        {
            if (waypointAvailability[waypoint])
            {
                waypointAvailability[waypoint] = false;
                return waypoint;
            }
        }
        return null; // Все точки заняты
    }

    public void ReleaseWaypoint(Transform waypoint)
    {
        if (waypoint != null && waypointAvailability.ContainsKey(waypoint))
        {
            waypointAvailability[waypoint] = true;
        }
    }
}