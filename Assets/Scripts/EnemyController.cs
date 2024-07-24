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
        

        StartCoroutine(EnemyRoutine());
    }

    private IEnumerator EnemyRoutine()
    {
        while (true)
        {   // Busca el árbol más cercano
            Vector3? treePosition = FindNearestTree();
            if (treePosition.HasValue)
            {   
                // Si se encuentra un árbol, mueve al enemigo hacia él y lo ataca
                yield return StartCoroutine(MoveToTree(treePosition.Value));
                yield return StartCoroutine(AttackTree(treePosition.Value));
            }
            else
            {
                // Si no hay árboles, espera un segundo antes de buscar de nuevo
                yield return new WaitForSeconds(1f);
            }
        }
    }

     /// <summary>
    /// Encuentra la posición del árbol más cercano al enemigo.
    /// </summary>
    /// <returns>La posición del árbol más cercano, o null si no hay árboles.</returns>
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

     /// <summary>
    /// Mueve al enemigo hacia la posición del árbol especificado.
    /// </summary>
    /// <param name="treePosition">La posición del árbol objetivo.</param>

    
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

    /// <summary>
    /// Realiza el ataque al árbol en la posición especificada.
    /// </summary>
    /// <param name="treePosition">La posición del árbol a atacar.</param>


    private IEnumerator AttackTree(Vector3 treePosition)
    {
    
        // Asegura que el enemigo mire directamente al árbol durante el ataque
        transform.LookAt(new Vector3(treePosition.x, transform.position.y, treePosition.z));

        // Verifica si el árbol aún existe
        if (!playerController.PlantedTrees.ContainsKey(treePosition))
        {
        
            yield break;
        }

        
        yield return new WaitForSeconds(attackDuration);

        if (playerController.PlantedTrees.ContainsKey(treePosition))
        {   
            // Destruye el árbol y cambia el material del terreno
            GameObject tree = playerController.PlantedTrees[treePosition];
            playerController.PlantedTrees.Remove(treePosition);
            Destroy(tree);
            Debug.Log("Árbol destruido en: " + treePosition);
           // ChangeGroundMaterial(treePosition);
        }
       
    }

    /// <summary>
    /// Cambia el material del terreno en la posición especificada.
    /// </summary>
    /// <param name="position">La posición donde cambiar el material del terreno.</param>

   
   // private void ChangeGroundMaterial(Vector3 position)
   // {
    //    RaycastHit hit;
    //    if (Physics.Raycast(position + Vector3.up, Vector3.down, out hit))
    //    {
    //        Renderer renderer = hit.collider.GetComponent<Renderer>();
    //        if (renderer != null)
     //       {
     //           renderer.material = sandMaterial;
                
      //      }
      //  }
   // }
}
