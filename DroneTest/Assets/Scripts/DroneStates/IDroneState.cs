public interface IDroneState
{
    void Enter(DroneStateMachine drone);
    void Update(DroneStateMachine drone);
    void Exit(DroneStateMachine drone);
}
