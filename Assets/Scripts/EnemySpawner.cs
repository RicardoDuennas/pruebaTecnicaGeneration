using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDelay = 5f;
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;


    private int currentEnemyCount = 0;
    public int maxEnemies = 5;

    void Start()
    {
        SpawnEnemy();
    }

    public void EnemyDied()
    {
        currentEnemyCount--;
        StartCoroutine(SpawnEnemyWithDelay());
    }

    private IEnumerator SpawnEnemyWithDelay()
    {
        float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(randomDelay);
        SpawnEnemy();
    }


    private void SpawnEnemy()
    {
        if (currentEnemyCount < maxEnemies)
        {
            Vector3 spawnPosition = transform.position + new Vector3(
                Random.Range(1f, 14.5f),
                0,
                Random.Range(1f, 14.5f)
            );

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetSpawner(this);
            }

            currentEnemyCount++;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }


}
