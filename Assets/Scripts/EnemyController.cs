
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackDuration = 2f;
    public Material sandMaterial;
    public Grid gridObject;

    private AudioSource enemyAudioSource;
    public float maxHealth = 100f;
    private float currentHealth;
    public float trenchWeakenEffect = 10f; // Cantidad de daño por segundo en la trinchera
    private bool isInTrench = false;
    private EnemySpawner spawner;





    [SerializeField] AudioClip eatingAudioClip, dyingAudioClip, treeFallingAudioClip, hittingTrench;


    private Grid3DPlayerController playerController;
    public Cell[,] gridCells;

    void Start()
    {
        enemyAudioSource = GetComponent<AudioSource>();

        playerController = FindObjectOfType<Grid3DPlayerController>();
        if (playerController == null)
        {
            Debug.LogError("No se encontró el Grid3DPlayerController en la escena.");
            return;
        }

        gridObject = GameObject.Find("Grid").GetComponent<Grid>();
        gridCells = gridObject.grid;

        currentHealth = maxHealth;
        StartCoroutine(CheckTrenchCollision());
        StartCoroutine(EnemyRoutine());
    }

    private IEnumerator CheckTrenchCollision()
    {
        while (true)
        {
            isInTrench = false;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Trench"))
                {
                    isInTrench = true;
                    break;
                }
            }

            if (isInTrench)
            {
                HitMetal();
                TakeDamage(trenchWeakenEffect * Time.deltaTime);
            }

            yield return null;
        }

    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (spawner != null)
        {
            spawner.EnemyDied();
        }

        AudioManager.Instance.PlaySFX(dyingAudioClip, 0.6f);
        Destroy(gameObject);

    }



    private IEnumerator EnemyRoutine()
    {
        while (true)
        {
            Vector3? treePosition = FindNearestTree();
            if (treePosition.HasValue)
            {
                yield return StartCoroutine(MoveToTree(treePosition.Value));
                yield return StartCoroutine(AttackTree(treePosition.Value));
            }
            else
            {
                //Debug.Log("No se encontraron árboles. Esperando...");
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private Vector3? FindNearestTree()
    {
        float nearestDistance = float.MaxValue;
        Vector3? nearestTreePos = null;

        foreach (var tree in playerController.PlantedTrees)
        {
            float distance = Vector3.Distance(transform.position, tree.Key);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTreePos = tree.Key;
            }
        }

        return nearestTreePos;
    }

    private IEnumerator MoveToTree(Vector3 treePosition)
    {
        while (Vector3.Distance(transform.position, treePosition) > 0.1f && !isInTrench)
        {    // Mover hacia el árbol
            transform.position = Vector3.MoveTowards(transform.position, treePosition, moveSpeed * Time.deltaTime);

            // Calcular la direccion hacia el árbol
            Vector3 directionToTree = (treePosition - transform.position).normalized;

            // Ignorar la componente y para que el enemigo no se mueva en el eje y
            directionToTree.y = 0;


            // Rotar hacia el arbol 
            if (directionToTree != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTree);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }

            yield return null;
        }

    }

    private IEnumerator AttackTree(Vector3 treePosition)
    {
        while (!isInTrench)
        {

            //Debug.Log("Iniciando ataque al árbol en: " + treePosition);

            transform.LookAt(new Vector3(treePosition.x, transform.position.y, treePosition.z));

            if (!playerController.PlantedTrees.ContainsKey(treePosition))
            {
                //Debug.Log("El árbol ya no está aquí.");
                yield break;
            }

            //Debug.Log("Enemigo está destruyendo un árbol!");
            PlayEnemySFX(eatingAudioClip);
            yield return new WaitForSeconds(attackDuration);

            if (playerController.PlantedTrees.ContainsKey(treePosition))
            {
                GameObject tree = playerController.PlantedTrees[treePosition];
                playerController.PlantedTrees.Remove(treePosition);
                Destroy(tree);
                PlayEnemySFX(treeFallingAudioClip);
                //Debug.Log("Árbol destruido en: " + treePosition);
                ChangeGroundMaterial(treePosition);
                gridCells[(int)treePosition.x, (int)treePosition.z].isActive = true;                  // Changes the state of the cell to soil
                gridObject.swapSquare(gridCells, (int)treePosition.x, (int)treePosition.z, true);   // Changes the cell texture to soil

            }
            else
            {
                //Debug.Log("El árbol fue removido durante el ataque.");
            }
        }
    }

    private void ChangeGroundMaterial(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out hit))
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = sandMaterial;
                //Debug.Log("Terreno cambiado a arena en: " + position);
            }
        }
    }

    public void SetSpawner(EnemySpawner newSpawner)
    {
        spawner = newSpawner;
    }

    private void PlayEnemySFX(AudioClip clip)
    {
        enemyAudioSource.PlayOneShot(clip, 1f);
    }

    private void HitMetal()
    {
        if (enemyAudioSource.isPlaying) return;

        enemyAudioSource.pitch = (Random.Range(0.8f, 1.2f));
        enemyAudioSource.PlayOneShot(hittingTrench, 0.6f);
    }
}