using UnityEngine;

public class SearchState : IDroneState
{
    public void Enter(DroneStateMachine drone)
    {
        Debug.Log("Entering Search State");
    }

    public void Update(DroneStateMachine drone)
    {
        var resource = drone.FindNearestResource();
        if (resource != null)
        {
            drone.CurrentTarget = resource;
            resource.GetComponent<Resource>().TargetedDrone = drone;
            drone.Agent.SetDestination(resource.position);
            drone.SetState(new MoveToResourceState());
        }
    }
    public void Exit(DroneStateMachine drone)
    {
    }
}
