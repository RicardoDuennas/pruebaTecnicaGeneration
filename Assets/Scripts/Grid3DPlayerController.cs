using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid3DPlayerController : MonoBehaviour
{
    public float cellSize = 1f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public GameObject treePrefab;
    public GameObject trenchPrefab;
    public Material grassMaterial;
    // Variables privadas para el movimiento
    private bool isMoving = false;          // Indica si el jugador está en movimiento
    private Vector3  targetPos;            // Posición objetivo del movimiento
    private Quaternion  targetRot;         // Rotación objetivo del movimiento
    
    private Animator animator;
    // Diccionarios para almacenar árboles y trincheras
    public Dictionary<Vector3, GameObject> PlantedTrees { get; private set; } = new Dictionary<Vector3, GameObject>();
    public Dictionary<Vector3, GameObject> BuiltTrenches { get; private set; } = new Dictionary<Vector3, GameObject>();
    void Start()
    {
        animator = GetComponent<Animator>();
         if (animator == null)
        {
            Debug.LogError("No se encontró el componente Animator en el jugador.");
        }
        
        targetPos = transform.position;
        targetRot = transform.rotation;
    
    }    
    
    void Update()
    {
        if (!isMoving)
        {
            Vector3 movement = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W)) movement += Vector3.forward;
            if (Input.GetKeyDown(KeyCode.S)) movement += Vector3.back;
            if (Input.GetKeyDown(KeyCode.A)) movement += Vector3.left;
            if (Input.GetKeyDown(KeyCode.D)) movement += Vector3.right;

            if (movement != Vector3.zero)
            {
                SetMoveTarget(movement.normalized);
            }
        }

        MoveTowardsTarget();

        if (Input.GetKeyDown(KeyCode.T))
            PlantTree();
        else if (Input.GetKeyDown(KeyCode.R))
            BuildTrench();
        
        // Manejo de animaciones adicionales
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Roll_Anim", !animator.GetBool("Roll_Anim"));
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetBool("Open_Anim", !animator.GetBool("Open_Anim"));
        } 
    }
     /// <summary>
    /// Establece el objetivo de movimiento basado en la dirección dada.
    /// </summary>
    /// <param name="direction">Dirección normalizada del movimiento.</param>

     private void SetMoveTarget(Vector3 direction)
    {
        isMoving = true;
        targetPos = transform.position + direction * cellSize;
        targetPos = new Vector3(
            Mathf.Round(targetPos.x / cellSize) * cellSize,
            transform.position.y,
            Mathf.Round(targetPos.z / cellSize) * cellSize
        );
        targetRot = Quaternion.LookRotation(direction);
    }
    /// <summary>
    /// Mueve al jugador hacia la posición objetivo y lo rota hacia la dirección del movimiento.
    /// </summary>

    private void MoveTowardsTarget()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            animator.SetBool("Walk_Anim", true);

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;
                transform.rotation = targetRot;
                isMoving = false;
                animator.SetBool("Walk_Anim", false);
            }
        }
    }
    /// <summary>
    /// Planta un árbol en la posición actual del jugador si no hay uno ya plantado.
    /// </summary>
    private void PlantTree()
    {
         Vector3 position = GetGridPosition();
    
    if (!PlantedTrees.ContainsKey(position))
    {
        GameObject tree = Instantiate(treePrefab, position, Quaternion.identity);
        PlantedTrees[position] = tree;
        
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

    /// <summary>
    /// Construye una trinchera en la posición actual del jugador si no hay una ya construida.
    /// </summary>
    private void BuildTrench()
    {
        Vector3 position = GetGridPosition();
        
        if (!BuiltTrenches.ContainsKey(position))
        {
            GameObject trench = Instantiate(trenchPrefab, position, Quaternion.identity);
            BuiltTrenches[position] = trench;
        }
    }

    /// <summary>
    /// Obtiene la posición del jugador ajustada a la cuadrícula.
    /// </summary>
    /// <returns>La posición ajustada a la cuadrícula.</returns>
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
