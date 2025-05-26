using UnityEngine;

public class ReturnToBaseState : IDroneState
{
    public void Enter(DroneStateMachine drone)
    {
        // Резервируем waypoint
        drone.ReservedWaypoint = drone.WaypointManager.ReserveWaypoint();

        if (drone.ReservedWaypoint != null)
        {
            drone.Agent.SetDestination(drone.ReservedWaypoint.position);
        }
        else
        {
            // Если все точки заняты, летим к базе (можно добавить ожидание)
            drone.Agent.SetDestination(drone.BaseTransform.position);
        }
    }

    public void Update(DroneStateMachine drone)
    {
        if (!drone.Agent.pathPending &&
            drone.Agent.remainingDistance <= drone.Agent.stoppingDistance)
        {
            drone.SetState(new DeliverResourceState());
        }
    }

    public void Exit(DroneStateMachine drone)
    {
        // Освобождаем waypoint при выходе из состояния
        if (drone.ReservedWaypoint != null)
        {
            drone.WaypointManager.ReleaseWaypoint(drone.ReservedWaypoint);
            drone.ReservedWaypoint = null;
        }
    }
}
