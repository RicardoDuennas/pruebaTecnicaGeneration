using UnityEngine;
using System.Collections;

public class PotionManager : MonoBehaviour
{
    [SerializeField] private GameObject potionPrefab;
    [SerializeField] private float minSpawnTime = 3f;
    [SerializeField] private float maxSpawnTime = 8f;
    [SerializeField] private float minVisibleTime = 5f;
    [SerializeField] private float maxVisibleTime = 10f;
        
    private GameObject currentPotion;
    private bool isWaitingToSpawn = false;

    private void Start()
    {
        StartCoroutine(PotionCycle());
    }

    private IEnumerator PotionCycle()
    {
        while (true)
        {
            // Wait random time before spawning
            float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
            isWaitingToSpawn = true;
            yield return new WaitForSeconds(spawnDelay);
            
            // Spawn the potion at random position
            Vector3 randomPosition = new Vector3(
                Random.Range(0, 14),
                0,
                Random.Range(0, 14)
            );
            
            currentPotion = Instantiate(potionPrefab, randomPosition, Quaternion.identity);
            isWaitingToSpawn = false;

            // If potion isn't collected, destroy after random time
            float visibleTime = Random.Range(minVisibleTime, maxVisibleTime);
            yield return new WaitForSeconds(visibleTime);

            if (currentPotion != null)
            {
                Destroy(currentPotion);
            }
        }
    }

    // Call this method when player collects the potion
    public void PotionCollected()
    {
        if (currentPotion != null)
        {
            Destroy(currentPotion);
        }
    }
}