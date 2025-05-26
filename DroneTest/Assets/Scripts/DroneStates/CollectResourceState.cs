using UnityEngine;

public class CollectResourceState : IDroneState
{
    private float collectionTime = 2f;
    private float timer;

    public void Enter(DroneStateMachine drone)
    {
        Debug.Log("Entering CollectResource State");
        timer = 0f;
        if (drone.CollectionEffect != null)
        {
            drone.CollectionEffect.Play();
        }
    }

    public void Update(DroneStateMachine drone)
    {
        timer += Time.deltaTime;
        if (timer >= collectionTime)
        {
            if (drone.CollectionEffect != null)
            {
                drone.CollectionEffect.Stop();
            }
            drone.SetState(new ReturnToBaseState());
        }
    }

    public void Exit(DroneStateMachine drone)
    {
    }
}
