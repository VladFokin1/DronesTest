using UnityEngine;

public class ReturnToBaseState : IDroneState
{
    public void Enter(DroneStateMachine drone)
    {
        // ����������� waypoint
        drone.ReservedWaypoint = drone.WaypointManager.ReserveWaypoint();

        if (drone.ReservedWaypoint != null)
        {
            drone.Agent.SetDestination(drone.ReservedWaypoint.position);
        }
        else
        {
            // ���� ��� ����� ������, ����� � ���� (����� �������� ��������)
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
        // ����������� waypoint ��� ������ �� ���������
        if (drone.ReservedWaypoint != null)
        {
            drone.WaypointManager.ReleaseWaypoint(drone.ReservedWaypoint);
            drone.ReservedWaypoint = null;
        }
    }
}
