using UnityEngine;
using UnityEngine.AI;

public class DroneStateMachine : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private ParticleSystem collectionEffect;
    [SerializeField] private ParticleSystem deliveryEffect;


    private Transform reservedWaypoint;
    private BaseWaypointManager waypointManager;

    public Transform ReservedWaypoint 
    { 
        get { return reservedWaypoint; }
        set { reservedWaypoint = value; }
    }
    
    public BaseWaypointManager WaypointManager => waypointManager;

    public NavMeshAgent Agent { get; private set; }
    public Transform CurrentTarget { get; set; }
    public Team Team { get; private set; }
    public Transform BaseTransform { get; private set; }
    public ParticleSystem CollectionEffect => collectionEffect;
    public ParticleSystem DeliveryEffect => deliveryEffect;

    private IDroneState currentState;

    public void Initialize(Team team, Transform baseTransform, BaseWaypointManager wpManager)
    {
        Team = team;
        BaseTransform = baseTransform;
        waypointManager = wpManager;

        // Set team color
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = team == Team.Red ? Color.red : Color.green;
        }

        Agent = GetComponent<NavMeshAgent>();
        if (Agent == null)
        {
            Agent = gameObject.AddComponent<NavMeshAgent>();
        }

        SetState(new SearchState());
    }

    private void Update()
    {
        currentState?.Update(this);
    }

    public void SetState(IDroneState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public Transform FindNearestResource()
    {
        // Поиск ближайшего ресурса с учетом команды
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20f);
        Transform closest = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Resource"))
            {
                Resource resource = hit.GetComponent<Resource>();
                if (resource.TargetedDrone == null || resource.TargetedDrone.Team != this.Team  )
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closest = hit.transform;
                    }
                }
            }
        }
        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState is MoveToResourceState)
        {
            var resource = other.GetComponent<Resource>();
            if (resource != null && other.transform == CurrentTarget)
            {
                resource.Collect();
                CurrentTarget = null;
                SetState(new CollectResourceState());
            }
        }
    }

    public void DeliverResource()
    {
        GameManager.Instance.AddScore(Team, 1);
    }


}