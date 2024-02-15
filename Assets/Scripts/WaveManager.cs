using UnityEngine;

public class WaveManager :MonoBehaviour
{
    [SerializeField] Transform enemyBase;
    [SerializeField] GameObject enemyPrefab;
    float timeBetweenWaves = 3f;

    private void Update()
    {
        timeBetweenWaves -= Time.deltaTime;
        if (timeBetweenWaves <= 0)
        {
            GenerateEnemy();
            timeBetweenWaves = 3f;
        }
    }
    void GenerateEnemy()
    {
        var enemy = Instantiate(enemyPrefab, enemyBase.position, Quaternion.identity);
    }
}
