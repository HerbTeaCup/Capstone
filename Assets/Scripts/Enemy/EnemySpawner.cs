using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // ������ �� ������
    public float spawnInterval = 5f; // �� ���� ����
    public float spawnRadius = 10f; // ������ �ֺ� �� ���� �ݰ�
    public float minSpawnDistanceFromPlayer = 20f; // �÷��̾���� �ּ� �Ÿ�
    public Transform player; // �÷��̾��� Transform

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
            // ������ �ֺ��� ���� ��ġ ����
            spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = transform.position.y; // ���� ����
        }
        while (Vector3.Distance(spawnPosition, player.position) < minSpawnDistanceFromPlayer);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
