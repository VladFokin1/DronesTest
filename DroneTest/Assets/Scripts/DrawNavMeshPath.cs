using UnityEngine;
using UnityEngine.AI;

public class DrawNavMeshPath : MonoBehaviour
{
    private NavMeshAgent agent;
    private LineRenderer lineRenderer; // �����������: ��� ��������� ���������� ����

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // �����������: ��������� LineRenderer ��� ������������ ����
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = Color.white };
    }

    void Update()
    {
        if (agent.hasPath)
        {

            RenderPathWithLineRenderer(agent.path);
        }
    }

    // �������������: �������� ���� ����� LineRenderer (����� � � Game View)
    private void RenderPathWithLineRenderer(NavMeshPath path)
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = path.corners.Length;
            lineRenderer.SetPositions(path.corners);
        }
    }

    private void OnDisable()
    {
        lineRenderer.positionCount = 0;
    }
}