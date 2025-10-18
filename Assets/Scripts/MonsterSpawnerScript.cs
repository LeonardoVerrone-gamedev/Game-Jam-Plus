using UnityEngine;

public class MonsterSpawnerScript : MonoBehaviour
{
    [SerializeField] TrademillScript trademill;
    [SerializeField] GameObject MonsterPrefab;
    [SerializeField] float objectDistance = 2f;

    [SerializeField] float spawnTimer;
    [SerializeField] Transform spawnPos;

    float requiredSpawnInterval;
    float TrademillVelocity;
    
    void Start()
    {
        Invoke("SpawnObject", 3f);
    }

    void Update()
    {
        if (trademill == null) return;

        TrademillVelocity = trademill.currentForce;

        if (TrademillVelocity <= 0.001f)
        {
            spawnTimer = 0f;
            return;
        }

        requiredSpawnInterval = objectDistance / TrademillVelocity;
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= requiredSpawnInterval)
        {
            SpawnObject();
        }
    }
    
    void SpawnObject()
    {
        Instantiate(MonsterPrefab, spawnPos.position, Quaternion.identity);
        spawnTimer -= requiredSpawnInterval;
    }
}
