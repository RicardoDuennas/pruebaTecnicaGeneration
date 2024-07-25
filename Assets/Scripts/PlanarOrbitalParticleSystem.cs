using UnityEngine;

public class PlanarOrbitalParticleSystem : MonoBehaviour
{
    [Header("Particle Settings")]
    public GameObject particlePrefab;
    public int particleCount = 100;
    public float minOrbitSpeed = 1f;
    public float maxOrbitSpeed = 5f;
    public float minOrbitRadius = 5f;
    public float maxOrbitRadius = 20f;

    [Header("System Settings")]
    public Vector3 systemCenter = Vector3.zero;
    public float planeHeight = 10f; // Height of the plane above the system center

    private GameObject[] particles;
    private float[] radii;
    private float[] speeds;
    private float[] angles;

    void Start()
    {
        InitializeParticles();
    }

    void Update()
    {
        MoveParticles();
    }

    void InitializeParticles()
    {
        particles = new GameObject[particleCount];
        radii = new float[particleCount];
        speeds = new float[particleCount];
        angles = new float[particleCount];

        for (int i = 0; i < particleCount; i++)
        {
            particles[i] = Instantiate(particlePrefab, RandomPosition(), Quaternion.identity, transform);
            radii[i] = Random.Range(minOrbitRadius, maxOrbitRadius);
            speeds[i] = Random.Range(minOrbitSpeed, maxOrbitSpeed);
            angles[i] = Random.Range(0f, 360f);
        }
    }

    void MoveParticles()
    {
        for (int i = 0; i < particleCount; i++)
        {
            angles[i] += speeds[i] * Time.deltaTime;
            if (angles[i] > 360f) angles[i] -= 360f;

            float x = Mathf.Cos(angles[i] * Mathf.Deg2Rad) * radii[i];
            float z = Mathf.Sin(angles[i] * Mathf.Deg2Rad) * radii[i];
            
            particles[i].transform.position = new Vector3(
                systemCenter.x + x,
                systemCenter.y + planeHeight,
                systemCenter.z + z
            );
        }
    }

    Vector3 RandomPosition()
    {
        float angle = Random.Range(0f, 360f);
        float radius = Random.Range(minOrbitRadius, maxOrbitRadius);
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        return new Vector3(
            systemCenter.x + x,
            systemCenter.y + planeHeight,
            systemCenter.z + z
        );
    }
}