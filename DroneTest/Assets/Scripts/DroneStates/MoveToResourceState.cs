using UnityEngine;

public class MoveToResourceState : IDroneState
{
    public void Enter(DroneStateMachine drone)
    {
        Debug.Log("Entering MoveToResource State");
    }

    public void Update(DroneStateMachine drone)
    {
        if (drone.CurrentTarget == null) // ���� ������ ����� (��������, ������ ������ ������)
        {
            drone.SetState(new SearchState());
            return;
        }
        if (!drone.Agent.pathPending && drone.Agent.remainingDistance <= drone.Agent.stoppingDistance)
        {
            drone.SetState(new CollectResourceState());
        }
    }

    public void Exit(DroneStateMachine drone)
    {
    }
}