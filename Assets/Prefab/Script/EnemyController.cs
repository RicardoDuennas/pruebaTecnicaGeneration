
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackDuration = 2f;
    public Material sandMaterial;

    private Grid3DPlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<Grid3DPlayerController>();
        if (playerController == null)
        {
            Debug.LogError("No se encontró el Grid3DPlayerController en la escena.");
            return;
        }

        StartCoroutine(EnemyRoutine());
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
                Debug.Log("No se encontraron árboles. Esperando...");
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
        while (Vector3.Distance(transform.position, treePosition) > 0.1f)
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
        Debug.Log("Iniciando ataque al árbol en: " + treePosition);

        transform.LookAt(new Vector3(treePosition.x, transform.position.y, treePosition.z));

        if (!playerController.PlantedTrees.ContainsKey(treePosition))
        {
            Debug.Log("El árbol ya no está aquí.");
            yield break;
        }

        Debug.Log("Enemigo está destruyendo un árbol!");
        yield return new WaitForSeconds(attackDuration);

        if (playerController.PlantedTrees.ContainsKey(treePosition))
        {
            GameObject tree = playerController.PlantedTrees[treePosition];
            playerController.PlantedTrees.Remove(treePosition);
            Destroy(tree);
            Debug.Log("Árbol destruido en: " + treePosition);
            ChangeGroundMaterial(treePosition);
        }
        else
        {
            Debug.Log("El árbol fue removido durante el ataque.");
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
                Debug.Log("Terreno cambiado a arena en: " + position);
            }
        }
    }
}