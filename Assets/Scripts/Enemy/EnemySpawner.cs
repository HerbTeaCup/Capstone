using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 생성할 적 프리팹
    public float spawnInterval = 5f; // 적 생성 간격
    public float spawnRadius = 10f; // 스포너 주변 적 생성 반경
    public float minSpawnDistanceFromPlayer = 20f; // 플레이어와의 최소 거리
    public Transform player; // 플레이어의 Transform

    private float currentTime = 0f;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= spawnInterval)
        {
            SpawnEnemy();
            currentTime = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;

        do
        {
            // 스포너 주변의 랜덤 위치 선택
            spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = transform.position.y; // 높이 고정
        }
        while (Vector3.Distance(spawnPosition, player.position) < minSpawnDistanceFromPlayer);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
