using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnDistance;
    [SerializeField] private int spawnersToActivateCount;
    [SerializeField] private float spawnRate;
    private float spawnTimer = 0f;
    public bool spawnEnabled = false;

    void Update()
    {
        if (spawnEnabled && Time.time >= spawnTimer)
        {
            for (int i = 0; i < spawnersToActivateCount; i++)
            {
                int randEnemy = Random.Range(0, enemyPrefabs.Length);
                int randSpawnPoint = findPointsFarFromPlayer(spawnPoints);

                EnemyMovement newEnemyMovement = Instantiate(enemyPrefabs[randEnemy], spawnPoints[randSpawnPoint].position, transform.rotation).GetComponent<EnemyMovement>();
                newEnemyMovement.isPatrolling = false;
                newEnemyMovement.target = SpawnSystemScript.instance.player.transform;
                newEnemyMovement.isEnemyNight = true;
                newEnemyMovement.moveToLightHouse = true;
                newEnemyMovement.GoToTarget();
            }
            spawnTimer = Time.time + 1f / spawnRate;
        }
    }

    private int findPointsFarFromPlayer(Transform[] spawnPoints)
    {
        List<int> excludedPoints = new List<int>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Vector2.Distance(SpawnSystemScript.instance.player.transform.position, spawnPoints[i].position) < spawnDistance)
            {
                excludedPoints.Add(i);
            }
        }

        return RandomFromRangeWithExceptions(0, spawnPoints.Length, excludedPoints);

    }

    private int RandomFromRangeWithExceptions(int rangeMin, int rangeMax, List<int> exclude)
    {
        var range = Enumerable.Range(rangeMin, rangeMax).Where(i => !exclude.Contains(i));
        int index = Random.Range(rangeMin, rangeMax - exclude.Count());
        return range.ElementAt(index);
    }

}

