using UnityEngine;
using System.Collections;

public class PotionManager : MonoBehaviour
{
    [System.Serializable]
    public class PotionConfig
    {
        public GameObject potionPrefab;
        public float minSpawnTime = 3f;
        public float maxSpawnTime = 8f;
        public float minVisibleTime = 5f;
        public float maxVisibleTime = 10f;
    }

    [SerializeField] private PotionConfig[] potionConfigs = new PotionConfig[3];
    
    private GameObject[] currentPotions = new GameObject[3];

    private void Start()
    {
        // Start cycle for each potion type
        for (int i = 0; i < potionConfigs.Length; i++)
        {
            StartCoroutine(PotionCycle(i));
        }
    }

    private IEnumerator PotionCycle(int potionIndex)
    {
        while (true)
        {
            // Wait random time before spawning
            float spawnDelay = Random.Range(
                potionConfigs[potionIndex].minSpawnTime, 
                potionConfigs[potionIndex].maxSpawnTime
            );
            yield return new WaitForSeconds(spawnDelay);
            
            // Spawn the potion at random position
            Vector3 randomPosition = new Vector3(
                Random.Range(0, 15),
                0,
                Random.Range(0, 15)
            );
            
            currentPotions[potionIndex] = Instantiate(
                potionConfigs[potionIndex].potionPrefab, 
                randomPosition, 
                Quaternion.identity
            );

            // If potion isn't collected, destroy after random time
            float visibleTime = Random.Range(
                potionConfigs[potionIndex].minVisibleTime, 
                potionConfigs[potionIndex].maxVisibleTime
            );
            yield return new WaitForSeconds(visibleTime);

            if (currentPotions[potionIndex] != null)
            {
                Destroy(currentPotions[potionIndex]);
            }
        }
    }

    // Call this method when player collects the potion
    public void PotionCollected(int potionIndex)
    {
        if (currentPotions[potionIndex] != null)
        {
            Destroy(currentPotions[potionIndex]);
        }
    }
}
// using UnityEngine;
// using System.Collections;

// public class PotionManager : MonoBehaviour
// {
//     [SerializeField] private GameObject potionPrefab;
//     [SerializeField] private float minSpawnTime = 1f; // 3f
//     [SerializeField] private float maxSpawnTime = 2f; // 8f
//     [SerializeField] private float minVisibleTime = 5f; // 5f; 
//     [SerializeField] private float maxVisibleTime = 10f; // 10f;
        
//     private GameObject currentPotion;
//     private bool isWaitingToSpawn = false;

//     private void Start()
//     {
//         StartCoroutine(PotionCycle());
//     }

//     private IEnumerator PotionCycle()
//     {
//         while (true)
//         {
//             // Wait random time before spawning
//             float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
//             isWaitingToSpawn = true;
//             yield return new WaitForSeconds(spawnDelay);
            
//             // Spawn the potion at random position
//             Vector3 randomPosition = new Vector3(
//                 Random.Range(0, 14),
//                 0,
//                 Random.Range(0, 14)
//             );
            
//             currentPotion = Instantiate(potionPrefab, randomPosition, Quaternion.identity);
//             isWaitingToSpawn = false;

//             // If potion isn't collected, destroy after random time
//             float visibleTime = Random.Range(minVisibleTime, maxVisibleTime);
//             yield return new WaitForSeconds(visibleTime);

//             if (currentPotion != null)
//             {
//                 Destroy(currentPotion);
//             }
//         }
//     }

//     // Call this method when player collects the potion
//     public void PotionCollected()
//     {
//         if (currentPotion != null)
//         {
//             Destroy(currentPotion);
//         }
//     }
// }