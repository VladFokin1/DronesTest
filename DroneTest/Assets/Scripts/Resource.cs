using UnityEngine;
using System;

public class Resource : MonoBehaviour
{
    public event Action OnCollected;

    [SerializeField] private int scoreValue = 1;

    public DroneStateMachine TargetedDrone { get; set; }

    public int ScoreValue => scoreValue;

    public void Collect()
    {
        OnCollected?.Invoke();
        Destroy(this.gameObject);
    }

}