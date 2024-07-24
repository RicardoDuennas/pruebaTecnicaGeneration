using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid3DPlayerController : MonoBehaviour
{
    public float cellSize = 1f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public GameObject treePrefab;
    public GameObject trenchPrefab;
    public Material grassMaterial;
    [SerializeField] AudioClip pathBlockedAudioClip, plantAudioClip, drillAudioClip, walkAudioClip;
    private bool isMoving = false;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private float timeToMove = 0.2f;

    public Dictionary<Vector3, GameObject> PlantedTrees { get; private set; } = new Dictionary<Vector3, GameObject>();
    public Dictionary<Vector3, GameObject> BuiltTrenches { get; private set; } = new Dictionary<Vector3, GameObject>();



    void Update()
    {
        if (!isMoving)
        {
            Vector3 movement = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W))
                movement = Vector3.forward;
            else if (Input.GetKeyDown(KeyCode.S))
                movement = Vector3.back;
            else if (Input.GetKeyDown(KeyCode.A))
                movement = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.D))
                movement = Vector3.right;

            if (movement != Vector3.zero)
            {
                StartCoroutine(MovePlayer(movement));
            }


            else if (Input.GetKeyDown(KeyCode.T))
                PlantTree();
            else if (Input.GetKeyDown(KeyCode.R))
                BuildTrench();
        }
    }
    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        targetPos = transform.position + direction * cellSize;
        targetPos = new Vector3(
            Mathf.Round(targetPos.x / cellSize) * cellSize,
            transform.position.y,
            Mathf.Round(targetPos.z / cellSize) * cellSize
        );

        targetRot = Quaternion.LookRotation(direction);

        AudioManager.Instance.PlaySFX(walkAudioClip, 1.0f);

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        isMoving = false;
    }
    private void PlantTree()
    {
        Vector3 position = GetGridPosition();

        if (!PlantedTrees.ContainsKey(position))
        {
            GameObject tree = Instantiate(treePrefab, position, Quaternion.identity);
            PlantedTrees[position] = tree;
            AudioManager.Instance.PlaySFX(plantAudioClip, 1.0f);

            // Asegurarse de que el árbol no se destruya automáticamente
            DontDestroyOnLoad(tree);

            ChangeGroundMaterial(position, grassMaterial);

            Debug.Log("Árbol plantado en: " + position);
        }
        else
        {
            Debug.Log("Ya hay un árbol en esta posición: " + position);
        }
    }

    private void BuildTrench()
    {
        Vector3 position = GetGridPosition();

        if (!BuiltTrenches.ContainsKey(position))
        {
            GameObject trench = Instantiate(trenchPrefab, position, Quaternion.identity);
            BuiltTrenches[position] = trench;
            AudioManager.Instance.PlaySFX(drillAudioClip, 1.0f);
        }
    }

    private Vector3 GetGridPosition()
    {
        return new Vector3(Mathf.Round(transform.position.x / cellSize) * cellSize,
                           0,
                           Mathf.Round(transform.position.z / cellSize) * cellSize);
    }

    private void ChangeGroundMaterial(Vector3 position, Material material)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out hit))
        {
            hit.collider.GetComponent<Renderer>().material = material;
        }
    }
}
