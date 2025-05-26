using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private GameObject resourcePrefab;

    [Min(0.1f)]
    [SerializeField] private float spawnFrequency = 1f; // 1 ресурс/сек

    [SerializeField] private TMPro.TMP_InputField frequencyText;

    [Min(1)]
    [SerializeField] private int maxResources = 20;

    [SerializeField] private Bounds spawnArea = new Bounds(Vector3.zero, new Vector3(16f, 0f, 10f));

    private int currentResourceCount = 0;
    private Coroutine spawningCoroutine;

    private void Start() => StartSpawning();
    private void OnDisable() => StopSpawning();

    public void StartSpawning()
    {
        frequencyText.text = spawnFrequency + "";
        if (spawningCoroutine == null)
            spawningCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
            spawningCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentResourceCount < maxResources)
                SpawnResource();

            // Интервал = 1 / частота (например, для 2 ресурсов/сек ждем 0.5 сек)
            yield return new WaitForSeconds(1f / spawnFrequency);
        }
    }

    private void SpawnResource()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(spawnArea.min.x, spawnArea.max.x),
            spawnArea.center.y,
            Random.Range(spawnArea.min.z, spawnArea.max.z)
        );

        if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            var resource = Instantiate(resourcePrefab, hit.position, Quaternion.identity, transform);
            var resourceComp = resource.GetComponent<Resource>();

            if (resourceComp != null)
                resourceComp.OnCollected += () => currentResourceCount--;
            else
                Debug.LogError("У ресурса нет компонента Resource!");

            currentResourceCount++;
        }
    }

    public void SetFrequency()
    {
        spawnFrequency = float.Parse(frequencyText.text, System.Globalization.NumberStyles.Any);
    }
}