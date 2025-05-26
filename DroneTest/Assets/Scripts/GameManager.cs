using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Team Settings")]
    [SerializeField] private Transform redBase;
    [SerializeField] private Transform greenBase;
    [SerializeField] private GameObject greenDronePrefab;
    [SerializeField] private GameObject redDronePrefab;

    [Header("Waypoints")]
    [SerializeField] private BaseWaypointManager redBaseWaypoints;
    [SerializeField] private BaseWaypointManager greenBaseWaypoints;

    [Header("UI References")]
    [SerializeField] private Slider droneCountSlider;
    [SerializeField] private Slider droneSpeedSlider;
    [SerializeField] private Toggle showPathsToggle;
    [SerializeField] private TMPro.TextMeshProUGUI redScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI greenScoreText;

    [Header("Settings")]
    [Range(1, 5)] public int dronesPerTeam = 3;
    [Range(1, 10)] public float droneSpeed = 5f;
    public bool drawPaths = false;

    private List<DroneStateMachine> redDrones = new List<DroneStateMachine>();
    private List<DroneStateMachine> greenDrones = new List<DroneStateMachine>();
    private int redScore = 0;
    private int greenScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitializeUI();
        SpawnDrones();
    }

    private void InitializeUI()
    {
        droneCountSlider.minValue = 1;
        droneCountSlider.maxValue = 5;
        droneCountSlider.value = dronesPerTeam;
        droneCountSlider.onValueChanged.AddListener(OnDroneCountChanged);

        droneSpeedSlider.minValue = 1;
        droneSpeedSlider.maxValue = 10;
        droneSpeedSlider.value = droneSpeed;
        droneSpeedSlider.onValueChanged.AddListener(OnDroneSpeedChanged);

        showPathsToggle.isOn = drawPaths;
        showPathsToggle.onValueChanged.AddListener(OnShowPathsChanged);

        UpdateScoreUI();
    }

    private void SpawnDrones()
    {
        ClearExistingDrones();

        // Spawn red team drones
        for (int i = 0; i < dronesPerTeam; i++)
        {
            Vector3 spawnPos = redBase.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            GameObject drone = Instantiate(redDronePrefab, spawnPos, Quaternion.identity);
            var droneSM = drone.GetComponent<DroneStateMachine>();
            droneSM.Initialize(Team.Red, redBase, redBaseWaypoints);
            redDrones.Add(droneSM);
  
        }

        // Spawn green team drones
        for (int i = 0; i < dronesPerTeam; i++)
        {
            Vector3 spawnPos = greenBase.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            GameObject drone = Instantiate(greenDronePrefab, spawnPos, Quaternion.identity);
            var droneSM = drone.GetComponent<DroneStateMachine>();
            droneSM.Initialize(Team.Green, greenBase, greenBaseWaypoints);
            greenDrones.Add(droneSM);
        }

        UpdateAllDronesSpeed();
        UpdateAllDronesPathDraw(drawPaths);
    }

    private void ClearExistingDrones()
    {
        foreach (var drone in redDrones) Destroy(drone.gameObject);
        foreach (var drone in greenDrones) Destroy(drone.gameObject);
        redDrones.Clear();
        greenDrones.Clear();
    }

    private void UpdateAllDronesSpeed()
    {
        foreach (var drone in redDrones) drone.Agent.speed = droneSpeed;
        foreach (var drone in greenDrones) drone.Agent.speed = droneSpeed;
    }

    private void UpdateAllDronesPathDraw(bool draw)
    {
        foreach (var drone in redDrones)
        {
            DrawNavMeshPath path = drone.GetComponent<DrawNavMeshPath>();
            path.enabled = draw;
        }

        foreach (var drone in greenDrones)
        {
            DrawNavMeshPath path = drone.GetComponent<DrawNavMeshPath>();
            path.enabled = draw;
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawPaths) return;

        // Draw red team paths
        foreach (var drone in redDrones)
        {
            if (drone.Agent.hasPath)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < drone.Agent.path.corners.Length - 1; i++)
                {
                    Gizmos.DrawLine(drone.Agent.path.corners[i], drone.Agent.path.corners[i + 1]);
                }
            }
        }

        // Draw green team paths
        foreach (var drone in greenDrones)
        {
            if (drone.Agent.hasPath)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < drone.Agent.path.corners.Length - 1; i++)
                {
                    Gizmos.DrawLine(drone.Agent.path.corners[i], drone.Agent.path.corners[i + 1]);
                }
            }
        }
    }

    // UI Event Handlers
    private void OnDroneCountChanged(float value)
    {
        dronesPerTeam = Mathf.RoundToInt(value);
        SpawnDrones();
    }

    private void OnDroneSpeedChanged(float value)
    {
        droneSpeed = value;
        UpdateAllDronesSpeed();
    }

    private void OnShowPathsChanged(bool value)
    {
        drawPaths = value;
        UpdateAllDronesPathDraw(drawPaths);
    }

    // Score Management
    public void AddScore(Team team, int points)
    {
        if (team == Team.Red) redScore += points;
        else greenScore += points;

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        redScoreText.text = redScore + "";
        greenScoreText.text = greenScore + "";
        
    }
}

public enum Team
{
    Red,
    Green
}