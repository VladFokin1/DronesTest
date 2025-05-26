using UnityEngine;

public class DeliverResourceState : IDroneState
{
    private float deliveryTime = 1f;
    private float timer;

    public void Enter(DroneStateMachine drone)
    {
        Debug.Log("Entering DeliverResource State");
        timer = 0f;
        if (drone.DeliveryEffect != null)
        {
            drone.DeliveryEffect.Play();
        }
    }

    public void Update(DroneStateMachine drone)
    {
        timer += Time.deltaTime;
        if (timer >= deliveryTime)
        {
            drone.CurrentTarget = null;
            drone.DeliverResource();
            drone.SetState(new SearchState());
        }
    }

    public void Exit(DroneStateMachine drone)
    {
        if (drone.DeliveryEffect != null)
        {
            drone.DeliveryEffect.Stop();
        }
    }
}
